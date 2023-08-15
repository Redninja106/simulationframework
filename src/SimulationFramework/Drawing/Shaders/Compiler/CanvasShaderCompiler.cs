using SimulationFramework.Drawing.Shaders;
using SimulationFramework.Drawing.Shaders.Compiler.Passes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler;

public class CanvasShaderCompiler
{
    private List<CompilerPass> Rules = new()
    {
        new ConstructorPass(),
        new VariableAccessReplacements(),
        new ShaderTypeParameterPass(),
        new ShaderTypeRestrictions(),
        new ShaderIntrinsicSubstitutions(),
        new IntrinsicTypeVariableInlines(),
        new DependencyResolver(),
        new CallSubstitutions(),
        new GlobalMethodCall(),
        new InlineSourceInsertion(),
    };

    public CanvasShaderCompiler()
    {

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

        var entryPoint = GetShaderEntryPoint(shader);

        var context = new CompilationContext(this);

        context.ShaderType = shader.GetType();

        CompileShaderType(context, context.ShaderType);
        CompileMethod(context, entryPoint, true);

        return context.GetResult();
    }

    private MethodInfo GetShaderEntryPoint(CanvasShader shader)
    {
        var shaderType = shader.GetType();
        return shaderType.GetMethod(nameof(CanvasShader.GetPixelColor), new Type[] { typeof(Vector2) });
    }

    internal void CompileMethod(CompilationContext context, MethodBase method, bool isEntryPoint = false)
    {
        if (IsMethodIntrinsic(method))
            return;

        CompiledMethod m = new CompiledMethod(method);

        if (isEntryPoint)
        {
            context.EntryPoint = m;
        }

        foreach (var rule in this.Rules)
        {
            rule.CheckMethod(context, m);
        }

        context.methods.Add(m);
    }

    public static bool IsMethodIntrinsic(MethodBase method)
    {
        if (method.GetCustomAttribute<ShaderIntrinsicAttribute>() is not null)
            return true;

        var property = PropertyFromAccessor(method);

        if (property is not null && property.GetCustomAttribute<ShaderIntrinsicAttribute>() is not null)
            return true;

        return false;
    }

    private static PropertyInfo PropertyFromAccessor(MethodBase info)
    {
        var type = info.DeclaringType ?? throw new Exception();
        foreach (var property in type.GetProperties())
        {
            if (property.GetAccessors().Contains(info))
                return property;
        }
        return null;
    }

    public static bool IsTypeIntrinsic(Type type) => type.GetCustomAttribute<ShaderIntrinsicAttribute>() is not null;

    internal void CompileStruct(CompilationContext context, Type structType)
    {
        if (IsTypeIntrinsic(structType))
        {
            foreach (var genericArg in structType.GetGenericArguments())
            {
                if (context.structs.Any(s => s.StructType == genericArg))
                    continue;

                if (DependencyResolver.intrinsicTypes.Contains(genericArg))
                    continue;

                CompileStruct(context, genericArg);
            }

            return;
        }

        CompiledStruct s = new CompiledStruct(structType);

        foreach (var rule in this.Rules)
        {
            rule.CheckStruct(context, s);
        }

        context.structs.Add(s);
    }

    private void CompileShaderType(CompilationContext context, Type shaderType)
    {
        foreach (var field in shaderType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            var variable = new ShaderVariable(field);
            context.uniforms.Add(variable);

            foreach (var rule in Rules)
            {
                rule.CheckVariable(context, variable);
            }
        }

        // if (signature is not null)
        // {
        //     ArrangeInputs(context, signature);
        // }
    }

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