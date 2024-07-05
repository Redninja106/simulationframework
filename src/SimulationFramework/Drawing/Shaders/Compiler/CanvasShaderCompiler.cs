using SimulationFramework.Drawing.Shaders;
using SimulationFramework.Drawing.Shaders.Compiler.ControlFlow;
using SimulationFramework.Drawing.Shaders.Compiler.Expressions;
using SimulationFramework.Drawing.Shaders.Compiler.ILDisassembler;
using SimulationFramework.Drawing.Shaders.Compiler.Translation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

public class CanvasShaderCompiler
{
    /*
     * TODO:
     * - arrays (buffers)
     * - blending
     * - texture load/stores
     * - switches
     * - fix if bug
     * - text rendering
     */


    private Dictionary<Type, ShaderPrimitive> primitiveTypeMap = [];

    public CanvasShaderCompiler()
    {
        primitiveTypeMap[typeof(void)] = ShaderPrimitive.Void;
        primitiveTypeMap[typeof(bool)] = ShaderPrimitive.Bool;
        primitiveTypeMap[typeof(int)] = ShaderPrimitive.Int;
        primitiveTypeMap[typeof(float)] = ShaderPrimitive.Float;
        primitiveTypeMap[typeof(Vector2)] = ShaderPrimitive.Float2;
        primitiveTypeMap[typeof(Vector3)] = ShaderPrimitive.Float3;
        primitiveTypeMap[typeof(Vector4)] = ShaderPrimitive.Float4;
        primitiveTypeMap[typeof(ColorF)] = ShaderPrimitive.Float4;
        primitiveTypeMap[typeof(Matrix4x4)] = ShaderPrimitive.Matrix4x4;
        primitiveTypeMap[typeof(Matrix3x2)] = ShaderPrimitive.Matrix3x2;
        primitiveTypeMap[typeof(ITexture)] = ShaderPrimitive.Texture;
    }

    public ShaderCompilation Compile(CanvasShader shader)
    {
        /* 
         * be able to:
         * compile methods
         * compile structs
         *
         * to compile:
         * - compile shader type
         *      - fields of shader type are variables
         *          - input variable if has [ShaderIn]
         *          - output variable if has [ShaderOut]
         *          - uniform variable otherwise
         *      - method implementing IShader.Main() is EntryPoint
         *          - compile main method, and any methods it uses
         *          - don't compile methods marked with [ShaderInstrinsic]
         *          - keep track of all structs required structs
         *      - compile structs
         * 
         * to compile a method:
         * - generate CompiledMethod object
         * - apply rules
         * - enqueue dependencies
         *
         * rules:
         * - convert 'this.*' in shader type to a shader variable access expression
         * - convert constructor calls & method calls to compiledMethod calls
         * - convert common methods to their intrinsics
         * - disallow use of shader type (don't compile it)
         * - disallow recursive calls
         * - disallow reference types (except ITexture and IBuffer as shader variables)
         * - disallow ref/in/out parameters (& ref vars)
         * 
         * todo:
         * - allow branching
         * - allow out parameters
         * - linkage names
         * - interpolation control
         * - Access of shader ins/outs/uniforms from outside of shader struct (through some api like 'ShaderVars.Uniform("variable")').
         *
         */

        /*
         * Two phases:
         *      method discovery (decomp->intercepts)
         *      translation (everything after)
         *      
         * 
         */

        var shaderType = shader.GetType();
        var entryPoint = shaderType.GetMethod(
            nameof(CanvasShader.GetPixelColor),
            [typeof(Vector2)]
            ) ?? throw new();

        CompilerContext context = new(shaderType, entryPoint, primitiveTypeMap);

        foreach (var field in shaderType.GetFields())
        {
            ShaderVariable variable = new(context.CompileType(field.FieldType), new(field.Name), field);
            context.Uniforms.Add(field, variable);
            context.Compilation.Uniforms.Add(variable);
        }

        context.EnqueueMethod(entryPoint);

        while (context.MethodQueue.TryDequeue(out MethodBase? method))
        {
            var shaderMethod = context.Methods[method];
            CompileMethod(shaderMethod, context, method);
            context.Compilation.Methods.Add(shaderMethod);
        }

        return context.Compilation;
    }


    private void CompileMethod(ShaderMethod shaderMethod, CompilerContext context, MethodBase method)
    {
        MethodDisassembly disassembly = new(method);
        ControlFlowGraph graph = new ControlFlowGraph(disassembly);

        var locals = disassembly.MethodBody.LocalVariables.Select(l => new ShaderVariable(context.CompileType(l.LocalType), new("var" + l.LocalIndex), null)).ToArray();
        var parameters = method.GetParameters().Select(p => new ShaderVariable(context.CompileType(p.ParameterType), new(p.Name!), null)).ToArray();

        var parametersWithThis = parameters;

        ShaderVariable? self = null;
        if (!method.IsStatic) 
        {
            self = new ShaderVariable(context.CompileType(method.DeclaringType!), new("self"), null);
            
            parametersWithThis = parameters.Prepend(self).ToArray();
        }

        var expressionBuilder = new ExpressionBuilder(context, graph, disassembly, parametersWithThis, locals);
        var expression = expressionBuilder.BuildExpressions();
        
        if (!method.IsStatic && method is ConstructorInfo)
        {
            locals = locals.Append(self).ToArray();
        }

        ShaderName name;
        if (method.DeclaringType == context.ShaderType)
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

        shaderMethod.Body = expression;
        shaderMethod.Name = name;
        shaderMethod.Locals =  locals;
        shaderMethod.Parameters = parameters;
        shaderMethod.ReturnType = context.CompileType(expressionBuilder.ReturnType);
    }

    // private void CompileMethods(CompilerContext context, ShaderCompilation compilation)
    // {

        // CompilerPipeline methodPass = new(new MethodCompilerStage[]
        // {
        //     new DisassemblyStage(context),
        //     new CFGStage(context),
        //     new ExpressionBuilderStage(context),
        //     new InterceptResolverStage(context),
        //     new DependencyResolverStage(context),
        // });

    // }

    //internal void CompileMethod(CompilationContextOLD context, MethodBase method, bool isEntryPoint = false)
    //{
    //    if (intrinsicManager.IsIntrinsic(method))
    //        return;

    //    ShaderMethod shaderMethod = new ShaderMethod(method);

    //    if (isEntryPoint)
    //    {
    //        context.EntryPoint = shaderMethod;
    //    }
    //    var interceptionsVisitor = new InterceptionsVisitor();
    //    interceptionsVisitor.RegisterType(typeof(ShaderIntrinsics));
    //    shaderMethod.TransformBody(interceptionsVisitor);

    //    var constructorFixer = new ConstructorFixer();
    //    constructorFixer.FixMethod(context, shaderMethod);

    //    dependencyResolver.AddDependencies(context, shaderMethod);
    //    context.methods.Add(shaderMethod);
    //}

    //class InterceptionsVisitor : ExpressionVisitor
    //{
    //    private Dictionary<MethodBase, MethodBase> interceptions = new();

    //    public override Expression VisitCallExpression(CallExpression expression)
    //    {
    //        if (interceptions.TryGetValue(expression.Callee, out MethodBase? value))
    //        {
    //            var replacement = value as MethodInfo ?? throw new Exception("constructors may only intercept other constructors.");
    //            return new CallExpression(null, replacement, expression.Arguments);
    //        }

    //        return base.VisitCallExpression(expression);
    //    }

    //    public override Expression VisitNewExpression(NewExpression expression)
    //    {
    //        if (interceptions.TryGetValue(expression.Constructor!, out MethodBase? replacement))
    //        {
    //            if (replacement is ConstructorInfo constructor)
    //            {
    //                return new NewExpression(constructor, expression.Arguments);
    //            }
    //            else if (replacement is MethodInfo method)
    //            {
    //                return new CallExpression(null, method, expression.Arguments);
    //            }
    //            else
    //            {
    //                throw new Exception();
    //            }
    //        }

    //        return base.VisitNewExpression(expression);
    //    }

    //    internal void RegisterType(Type type)
    //    {
    //        foreach (var method in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
    //        {
    //            var attr = method.GetCustomAttribute<InterceptsAttribute>();
    //            if (attr is null)
    //                continue;

    //            MethodBase target;
    //            if (attr.MethodName is InterceptsAttribute.ConstructorName)
    //            {
    //                target = attr.MethodType.GetConstructor(method.GetParameters().Select(p => p.ParameterType).ToArray());
    //            }
    //            else
    //            {
    //                target = attr.MethodType.GetMethod(attr.MethodName, method.GetParameters().Select(p => p.ParameterType).ToArray());
    //            }

    //            interceptions.Add(target, method);
    //        }
    //    }
    //}

    //internal void CompileStructure(CompilationContextOLD context, Type structType)
    //{
    //    if (context.ContainsType(structType))
    //        return;

    //    if (!structType.IsValueType)
    //        throw new Exception($"Type '{structType}' may not be used in a shaders because it is not value type!");

    //    if (intrinsicManager.IsIntrinsic(structType))
    //    {
    //        foreach (var genericArg in structType.GetGenericArguments())
    //        {
    //            if (!intrinsicManager.IsIntrinsic(genericArg))
    //            {
    //                context.AddType(genericArg);
    //            }
    //        }

    //        return;
    //    }

    //    ShaderStructure structure = new ShaderStructure(structType);
    //    context.structs.Add(structure);

    //    dependencyResolver.AddDependencies(context, structure);
    //}

    //private void CompileShaderType(CompilationContextOLD context, Type shaderType)
    //{
    //    foreach (var field in shaderType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
    //    {
    //        var variable = new ShaderVariable(field);
    //        context.uniforms.Add(variable);

    //        if (!intrinsicManager.IsIntrinsic(variable.VariableType))
    //        {
    //            CompileStructure(context, variable.VariableType);
    //        }
    //    }
    //}

    // void ArrangeInputs(CompilationContext context, ShaderSignature signature)
    // {
    //     List<ShaderVariable> inputsSorted = new();
    // 
    //     foreach (var (type, name) in signature.Fields)
    //     {
    //         var variable = context.inputs.Single(variable => name == variable.InputName);
    //         Debug.Assert(variable.VariableType == type);
    //         inputsSorted.Add(variable);
    //         context.inputs.Remove(variable);
    //     }
    // 
    //     Debug.Assert(context.inputs.Where(v => v.InputSemantic is InputSemantic.None).Count() == 0);
    //     context.inputs = inputsSorted.Concat(context.inputs).ToList();
    // }
}
