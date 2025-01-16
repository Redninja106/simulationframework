using SimulationFramework.Drawing.Shaders;
using SimulationFramework.Drawing.Shaders.Compiler.ControlFlow;
using SimulationFramework.Drawing.Shaders.Compiler.Disassembler;
using SimulationFramework.Drawing.Shaders.Compiler.Expressions;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler;

public class ShaderCompiler
{
    public static ImmutableDictionary<Type, ShaderType> primitiveTypeMap = new Dictionary<Type, ShaderType>()
    {
        [typeof(void)] = ShaderType.Void,
        [typeof(bool)] = ShaderType.Bool,
        [typeof(sbyte)] = ShaderType.Int,
        [typeof(short)] = ShaderType.Int,
        [typeof(int)] = ShaderType.Int,
        [typeof(byte)] = ShaderType.UInt,
        [typeof(ushort)] = ShaderType.UInt,
        [typeof(uint)] = ShaderType.UInt,
        [typeof(float)] = ShaderType.Float,
        [typeof(Vector2)] = ShaderType.Float2,
        [typeof(Vector3)] = ShaderType.Float3,
        [typeof(Vector4)] = ShaderType.Float4,
        [typeof(ColorF)] = ShaderType.Float4,
        [typeof(Color)] = ShaderType.UInt,
        [typeof(Matrix4x4)] = ShaderType.Matrix4x4,
        [typeof(Matrix3x2)] = ShaderType.Matrix3x2,
        [typeof(ITexture)] = ShaderType.Texture,
        [typeof(IDepthMask)] = ShaderType.DepthMask,
    }.ToImmutableDictionary();

    public static bool DumpShaders { get; set; } = false;

    // TODO: direct texture reads
    // TODO: add method caching here
    // TODO: implicit uniforms (for Mouse.Position, Time.TotalTime, etc)
    // TODO: Random APIs (and maybe intercept System.Random)

    public ShaderCompiler()
    {
    }

    public ShaderCompilation Compile(Shader shader)
    {
        var shaderType = shader.GetType();
        ShaderKind kind;
        
        MethodInfo entryPoint;
        if (shader is CanvasShader)
        {
            entryPoint = shaderType.GetMethod(
                nameof(CanvasShader.GetPixelColor),
                [typeof(Vector2)]
                ) ?? throw new();
            kind = ShaderKind.Canvas;
        }
        else if (shader is VertexShader)
        {
            entryPoint = shaderType.GetMethod(
                nameof(VertexShader.GetVertexPosition),
                []
                ) ?? throw new();
            kind = ShaderKind.Vertex;
        }
        else if (shader is ComputeShader)
        {
            entryPoint = shaderType.GetMethod(
                nameof(ComputeShader.RunThread),
                [typeof(int), typeof(int), typeof(int)]
                ) ?? throw new();
            kind = ShaderKind.Compute;
        }
        else
        {
            throw new NotSupportedException(shader.GetType().Name);
        }

        CompilerContext context = new(shaderType, entryPoint);
        context.Compilation.Kind = kind;

        foreach (var f in shaderType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
        {
            // we need to get the field using the type that declared it so dictionary lookups don't fail (different ReflectedType values cause issues)
            var field = f.DeclaringType!.GetField(f.Name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)!;

            ShaderVariableKind varKind = ShaderVariableKind.Uniform;

            if (field.GetCustomAttribute<VertexDataAttribute>() != null)
            {
                varKind = ShaderVariableKind.VertexData;
            }
            else if (field.GetCustomAttribute<InstanceDataAttribute>() != null)
            {
                varKind = ShaderVariableKind.InstanceData;
            }
            else if (field.GetCustomAttribute<VertexShaderOutputAttribute>() != null)
            {
                varKind = ShaderVariableKind.VertexShaderOutput;
            }

            ShaderVariable variable = new(context.CompileType(field.FieldType), new(field.Name), field, varKind);
            context.Uniforms.Add(field, variable);
            context.Compilation.Variables.Add(variable);
        }

        context.EnqueueMethod(entryPoint);

        while (context.MethodQueue.TryDequeue(out MethodBase? method))
        {
            var shaderMethod = context.Methods[method];
            CompileMethod(shaderMethod, context, method);
            context.Compilation.Methods.Add(shaderMethod);
            if (method == entryPoint)
            {
                context.Compilation.EntryPoint = shaderMethod;
            }
        }

        return context.Compilation;
    }


    private void CompileMethod(ShaderMethod shaderMethod, CompilerContext context, MethodBase method)
    {
        shaderMethod.BackingMethod = method;

        MethodDisassembly disassembly = new(method);
        ControlFlowGraph graph = new ControlFlowGraph(disassembly);

        var locals = disassembly.MethodBody.LocalVariables.Select(l => new ShaderVariable(context.CompileType(l.LocalType), new("var" + l.LocalIndex), null, ShaderVariableKind.Local)).ToArray();
        var parameters = method.GetParameters().Select(p => new ShaderVariable(context.CompileType(p.ParameterType), new(p.Name!), null, ShaderVariableKind.Parameter)).ToArray();

        var parametersWithThis = parameters;

        ShaderVariable? self = null;
        if (!method.IsStatic)
        {
            self = new ShaderVariable(context.CompileType(method.DeclaringType!), new("self"), null, ShaderVariableKind.Parameter);
            
            parametersWithThis = parameters.Prepend(self).ToArray();
        }

        var expressionBuilder = new ExpressionBuilder(context, graph, disassembly, parametersWithThis, locals);
        var expressions = expressionBuilder.BuildExpressions();

        // post process steps
        expressions = RedundantVariableReplacer.ReplaceRedundantVariables(expressions, ref locals);
        expressions = TernaryBlockRemover.RemoveBlocksFromTernaryExpressions(expressions);
        expressions = ConditionalCleanup.CleaupConditionals(expressions);

        if (!method.IsStatic && method is ConstructorInfo)
        {
            locals = locals.Append(self!).ToArray();
        }

        ShaderName name;
        if (context.IsSelfType(method.DeclaringType))
        {
            name = new(method.Name);
        }
        else
        {
            name = new(method.DeclaringType!.FullName + "_" + method.Name);

            if (method is not ConstructorInfo)
            {
                parameters = parametersWithThis;
            }
        }

        shaderMethod.Body = expressions;
        shaderMethod.Name = name;
        shaderMethod.Locals =  locals;
        shaderMethod.Parameters = parameters;
        shaderMethod.ReturnType = context.CompileType(expressionBuilder.ReturnType);
    }

    public static void LogShaderSource(string kind, string source)
    {
        if (DumpShaders)
        {
            Console.WriteLine($"{new string('=', 20)} {kind} {new string('=', 20)}");
            Console.WriteLine(string.Join("\n", source.Split('\n').Select((s, i) => $"{i + 1,-3:d}|{s}")));
        }
    }

}
