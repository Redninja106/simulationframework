using SimulationFramework.Shaders.Compiler;
using SimulationFramework.Shaders.Compiler.ILDisassembler;
using SimulationFramework.Shaders.Compiler.Rules;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders.Compiler;

public class ShaderCompiler
{
    private List<CompilerRule> Rules = new();

    public ShaderCompiler()
    {
        Rules.Add(new VariableAccessReplacements());
        Rules.Add(new ShaderTypeParameterRules());
        // Rules.Add(new ShaderTypeRestrictions());
        Rules.Add(new ShaderIntrinsicSubstitutions());
        Rules.Add(new PrintILRule());
        Rules.Add(new DependencyResolver());
        Rules.Add(new CallSubstitutions());
    }

    public ShaderCompilation? Compile(Type shaderType, ShaderSignature? signature, ShaderKind targetShaderKind)
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

        if (!shaderType.IsValueType)
        {
            throw new ArgumentException("shaderType must be a struct!", nameof(shaderType));
        }

        var entryPoint = GetShaderEntryPoint(shaderType);

        var context = new CompilationContext(this);
        context.ShaderType = shaderType;
        context.kind = targetShaderKind;

        CompileShaderType(context, shaderType, signature);
        CompileMethod(context, entryPoint);

        context.EntryPoint = context.methods.Single(m => m.Method == entryPoint);

        return context.GetResult();
    }

    private MethodInfo GetShaderEntryPoint(Type shaderType)
    {
        if (!shaderType.GetInterfaces().Contains(typeof(IShader)))
        {
            throw new ArgumentException("shaderType must implement IShader!", nameof(shaderType));
        }

        var interfaceMap = shaderType.GetInterfaceMap(typeof(IShader));

        return interfaceMap.TargetMethods.Single(m => m.Name is nameof(IShader.Main));
    }

    internal void CompileMethod(CompilationContext context, MethodBase method)
    {
        CompiledMethod m = new CompiledMethod(method);

        foreach (var rule in this.Rules)
        {
            rule.CheckMethod(context, m);
        }

        context.methods.Add(m);
    }

    internal void CompileStruct(CompilationContext context, Type structType)
    {
        CompiledStruct s = new CompiledStruct(structType);

        foreach (var rule in this.Rules)
        {
            rule.CheckStruct(context, s);
        }

        context.structs.Add(s);
    }

    private void CompileShaderType(CompilationContext context, Type shaderType, ShaderSignature? signature)
    {
        foreach (var field in shaderType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            var variable = new CompiledVariable(field);
            if (variable.IsInput)
            {
                context.inputs.Add(variable);
            }
            else if (variable.IsOutput)
            {
                context.outputs.Add(variable);
            }
            else if (variable.IsUniform)
            {
                context.uniforms.Add(variable);
            }
            else
            {
                context.statics.Add(variable);
            }

            foreach (var rule in Rules)
            {
                rule.CheckVariable(context, variable);
            }
        }

        if (signature is not null)
        {
            ArrangeInputs(context, signature);
        }
    }

    void ArrangeInputs(CompilationContext context, ShaderSignature signature)
    {
        List<CompiledVariable> inputsSorted = new();

        foreach (var (type, name) in signature.Fields)
        {
            var variable = context.inputs.Single(variable => name == variable.InputName);
            Debug.Assert(variable.VariableType == type);
            inputsSorted.Add(variable);
            context.inputs.Remove(variable);
        }

        Debug.Assert(context.inputs.Where(v => v.InputSemantic is InputSemantic.None).Count() == 0);
        context.inputs = inputsSorted;
    }

    class Translator : ExpressionVisitor
    {
        private TranslationProfile profile;

        public Translator(TranslationProfile profile)
        {
            this.profile = profile;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (profile.methods.ContainsKey(node.Method))
            {
                var replacement = profile.methods[node.Method] as MethodInfo;
                return Expression.Call(node.Object, replacement!, node.Arguments);
            }

            return base.VisitMethodCall(node);
        }

        protected override Expression VisitNew(NewExpression node)
        {
            if (profile.methods.ContainsKey(node.Constructor!))
            {
                var replacement = profile.methods[node.Constructor!];
                if (replacement is ConstructorInfo constructor)
                {
                    return Expression.New(constructor, node.Arguments);
                }
                else if (replacement is MethodInfo method)
                {
                    return Expression.Call(method, node.Arguments);
                }
                else
                {
                    throw new Exception();
                }
            }

            return base.VisitNew(node);
        }
    }
}