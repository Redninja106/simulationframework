using SimulationFramework.Shaders.Compiler.Expressions;
using SimulationFramework.Shaders;
using SimulationFramework.Shaders.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders.Compiler.Rules;

internal class CallSubstitutions : CompilerPass
{
    private CompilationContext context;

    public override void CheckMethod(CompilationContext context, CompiledMethod compiledMethod)
    {
        this.context = context;
        compiledMethod.TransformBody(this);

        base.CheckMethod(context, compiledMethod);
    }

    protected override Expression VisitExtension(Expression node)
    {
        if (node is ConstructorCallExpression constructorCall)
        {
            var callee = FindCompiledMethod(constructorCall.Constructor);
            return new CompiledMethodCallExpression(callee, this.Visit(new System.Collections.ObjectModel.ReadOnlyCollection<Expression>(constructorCall.Arguments.ToList())));
        }

        return base.VisitExtension(node);
    }

    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
        if (ShaderCompiler.IsMethodIntrinsic(node.Method))
        {
            IEnumerable<Expression> args = this.Visit(node.Arguments);
            
            var instance = node.Object;
            if (instance is not null)
            {
                args = args.Prepend(Visit(instance));
            }

            return new IntrinsicCallExpression(node.Method, args);
        }
        else
        {
            var callee = FindCompiledMethod(node.Method);
            
            IEnumerable<Expression> args = this.Visit(node.Arguments);

            if (node.Object is not null)
            {
                args = args.Prepend(this.Visit(node.Object));
            }

            return new CompiledMethodCallExpression(callee, args.ToArray());
        }
    }

    protected override Expression VisitNew(NewExpression node)
    {
        var callee = FindCompiledMethod(node!.Constructor);
        return new CompiledMethodCallExpression(callee, this.Visit(node.Arguments));
    }

    private CompiledMethod FindCompiledMethod(MethodBase method)
    {
        return context.methods.Single(m => m.Method == method);
    }
}
