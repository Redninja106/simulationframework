using SimulationFramework.Drawing.Shaders.Compiler.Expressions;
using SimulationFramework.Drawing.Shaders.Compiler.ILDisassembler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler;
internal abstract class CompilerStage
{
    protected readonly CompilerContext context;

    protected CompilerStage(CompilerContext context)
    {
        this.context = context;
    }

    public abstract void Run(ShaderCompilation compilation);
}

abstract class MethodCompilerStage : CompilerStage
{
    protected MethodCompilerStage(CompilerContext context) : base(context)
    {
    }

    public override void Run(ShaderCompilation compilation)
    {
        Run(compilation, compilation.GetMethod(context.CurrentMethod));
    }

    public abstract void Run(ShaderCompilation compilation, ShaderMethod method);
}

class DisassemblyStage : MethodCompilerStage
{
    public DisassemblyStage(CompilerContext context) : base(context)
    {
    }

    public override void Run(ShaderCompilation compilation, ShaderMethod method)
    {
        method.SetDisassembly(new MethodDisassembly(context.CurrentMethod));
    }
}

class CFGStage : MethodCompilerStage
{
    public CFGStage(CompilerContext context) : base(context)
    {
    }

    public override void Run(ShaderCompilation compilation, ShaderMethod method)
    {
        method.SetControlFlowGraph(new(method.Disassembly));
    }
}

class ExpressionBuilderStage : MethodCompilerStage
{
    public ExpressionBuilderStage(CompilerContext context) : base(context)
    {
    }

    public override void Run(ShaderCompilation compilation, ShaderMethod method)
    {
        method.SetBody(ExpressionBuilder.BuildExpressions(method));
    }
}

class DependencyResolverStage : MethodCompilerStage
{
    public DependencyResolverStage(CompilerContext context) : base(context)
    {
    }

    public override void Run(ShaderCompilation compilation, ShaderMethod method)
    {
        Visitor visitor = new(context, compilation);
        method.VisitBody(visitor);
    }

    private class Visitor : ExpressionVisitor
    {
        private readonly CompilerContext context;
        private readonly ShaderCompilation compilation;

        public Visitor(CompilerContext context, ShaderCompilation compilation)
        {
            this.context = context;
            this.compilation = compilation;
        }

        public override Expression VisitCallExpression(CallExpression expression)
        {
            AddDependency(expression.Callee);
            return base.VisitCallExpression(expression);
        }

        public override Expression VisitNewExpression(NewExpression expression)
        {
            AddDependency(expression.Constructor);
            return base.VisitNewExpression(expression);
        }

        private void AddDependency(MethodBase dependency)
        {
            if (compilation.Methods.Any(m => m.Method == dependency))
                return;

            if (context.Intrinsics.IsIntrinsic(dependency))
                return;

            if (context.CompilationQueue.ContainsMethod(dependency))
                return;

            context.CompilationQueue.EnqueueMethod(dependency);
        }
    }

}

class InterceptResolverStage : MethodCompilerStage
{
    Dictionary<MethodBase, MethodInfo> intercepts = new();

    public InterceptResolverStage(CompilerContext context) : base(context)
    {
        foreach (var method in typeof(ShaderIntrinsics).GetMethods(BindingFlags.Static | BindingFlags.Public))
        {
            var attrs = method.GetCustomAttributes<ReplaceAttribute>();
            
            var parameterTypes = method.GetParameters().Select(p => p.ParameterType).ToArray();
            foreach (var attr in attrs)
            {
                intercepts.Add(GetInterceptTarget(attr, parameterTypes), method);
            }
        }

        MethodBase GetInterceptTarget(ReplaceAttribute attribute, Type[] parameterTypes)
        {
            var type = attribute.MethodType;
            var name = attribute.MethodName;

            if (name == ReplaceAttribute.ConstructorName)
            {
                return type.GetConstructor(parameterTypes) ?? throw new();
            }
            else
            {
                return type.GetMethod(name, BindingFlags.Static | BindingFlags.Public, parameterTypes) ?? type.GetMethod(name, BindingFlags.Instance | BindingFlags.Public, parameterTypes.Skip(1).ToArray()) ?? throw new();
            }
        }
    }

    public override void Run(ShaderCompilation compilation, ShaderMethod method)
    {
        method.VisitBody(new Visitor(this));
    }

    private class Visitor : ExpressionVisitor
    {
        readonly InterceptResolverStage interceptResolver;

        public Visitor(InterceptResolverStage interceptResolver)
        {
            this.interceptResolver = interceptResolver;
        }

        public override Expression VisitCallExpression(CallExpression expression)
        {
            if (interceptResolver.intercepts.TryGetValue(expression.Callee, out MethodInfo? source))
            {
                return base.VisitCallExpression(new CallExpression(null, source, expression.Instance is null ? expression.Arguments : expression.Arguments.Prepend(expression.Instance).ToList()));
            }

            return base.VisitCallExpression(expression);
        }

        public override Expression VisitNewExpression(NewExpression expression)
        {
            if (interceptResolver.intercepts.TryGetValue(expression.Constructor, out MethodInfo? source))
            {
                return base.VisitCallExpression(new CallExpression(null, source, expression.Arguments));
            }

            return base.VisitNewExpression(expression);
        }
    }
}