using SimulationFramework.Drawing.Shaders;
using SimulationFramework.Drawing.Shaders.Compiler.Expressions;
using SimulationFramework.Drawing.Shaders.Compiler.ILDisassembler;
using SimulationFramework.Drawing.Shaders.Compiler.Translation;
using SimulationFramework.Drawing.Shaders.Compiler.Translation.OLD;
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
     *             |------------------------------------------------------------------
     * stages:     V                                                                 |
     * decompilation -> control flow -> expression builder -> dependency resolver -> intercept resolver -> translation passes -> shader code emitter 
     *             ^                                          |      ^                           ^
     *             |-------------------------------------------       \                         /
     *                                                                 \-- intrinsic manager --/
     *             
     * decompilation:
     * turns byte[] into MethodDecompilation
     * 
     * control flow:
     * MethodDecompilation to control flow graph with basicBlockNodes
     * 
     * expression builder:
     * cfg of basicblocks into expression tree
     * 
     * dependency resolver:
     * - figures out what else needs to be compiled and restarts the process for those (recursively)
     * 
     * translation passes:
     * - translate C# ast into language agnostic shader ast  
     * 
     * shader code emitter:
     * - shader ast to final shader source
     */

    //private List<CompilerPass> Rules = new()
    //{
    //    new ConstructorPass(),
    //    new VariableAccessReplacements(),
    //    new ShaderTypeParameterPass(),
    //    new ShaderTypeRestrictions(),
    //    new ShaderIntrinsicSubstitutions(),
    //    new IntrinsicTypeVariableInlines(),
    //    new CallSubstitutions(),
    //    new GlobalMethodCall(),
    //    new InlineSourceInsertion(),
    //};

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


        /*
         * Two phases:
         *      method discovery (decomp->intercepts)
         *      translation (everything after)
         *      
         * 
         */

        CompilerContext context = new(shader.GetType());
        ShaderCompilation compilation = new();

        CompileVariables(context, compilation);
        CompileMethods(context, compilation);

        CompilerPipeline translationPass = new(new CompilerStage[]
        {
            new CallReplacementsStage(context),
            new RemoveThisOnGlobalsStage(context),
            new UniformAccessReplacementStage(context),
            new ConstructorFixStage(context),
            new DoubleEqualityFixStage(context),
        });

        translationPass.Run(compilation);
        return compilation; // .GetResult() ?
    }

    private void CompileVariables(CompilerContext context, ShaderCompilation compilation)
    {
        foreach (var field in context.ShaderType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
        {
            compilation.AddUniform(field);
        }
    }

    private MethodInfo GetShaderEntryPoint(CompilerContext context)
    {
        return context.ShaderType.GetMethod(nameof(CanvasShader.GetPixelColor), new Type[] { typeof(Vector2) });
    }

    private void CompileMethods(CompilerContext context, ShaderCompilation compilation)
    {
        var entryPoint = GetShaderEntryPoint(context);
        context.CompilationQueue.EnqueueMethod(entryPoint);

        CompilerPipeline methodPass = new(new MethodCompilerStage[]
        {
            new DisassemblyStage(context),
            new CFGStage(context),
            new ExpressionBuilderStage(context),
            new InterceptResolverStage(context),
            new DependencyResolverStage(context),
        });

        while (context.CompilationQueue.TryDequeueMethod(out MethodBase method))
        {
            context.SetCurrentMethod(method);
            compilation.AddMethod(method);
            methodPass.Run(compilation);
        }

        compilation.EntryPoint = compilation.GetMethod(entryPoint);
    }

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