using SimulationFramework.Drawing.Shaders.Compiler.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Translation.OLD;

internal class CallSubstitutions : CompilerPass
{
    private CompilationContextOLD context;

    // public override void CheckMethod(CompilationContext context, ShaderMethod compiledMethod)
    // {
    //     this.context = context;
    //     // compiledMethod.TransformBody(this);
    // 
    //     base.CheckMethod(context, compiledMethod);
    // }

    //protected override Expression VisitExtension(Expression node)
    //{
    //    if (node is ConstructorCallExpression constructorCall)
    //    {
    //        var callee = FindCompiledMethod(constructorCall.Constructor);
    //        return new CompiledMethodCallExpression(callee, Visit(new System.Collections.ObjectModel.ReadOnlyCollection<Expression>(constructorCall.Arguments.ToList())));
    //    }

    //    return base.VisitExtension(node);
    //}

    //protected override Expression VisitMethodCall(CallExpression node)
    //{
    //    if (CanvasShaderCompiler.IsMethodIntrinsic(node.Method))
    //    {
    //        IEnumerable<Expression> args = Visit(node.Arguments);

    //        var instance = node.Object;
    //        if (instance is not null)
    //        {
    //            args = args.Prepend(Visit(instance));
    //        }

    //        return new IntrinsicCallExpression(node.Method, args);
    //    }
    //    else
    //    {
    //        var callee = FindCompiledMethod(node.Method);

    //        IEnumerable<Expression> args = Visit(node.Arguments);

    //        if (node.Object is not null)
    //        {
    //            args = args.Prepend(Visit(node.Object));
    //        }

    //        return new CompiledMethodCallExpression(callee, args.ToArray());
    //    }
    //}

    //protected override Expression VisitNew(NewExpression node)
    //{
    //    var callee = FindCompiledMethod(node!.Constructor);
    //    return new CompiledMethodCallExpression(callee, Visit(node.Arguments));
    //}

    //private ShaderMethod FindCompiledMethod(MethodBase method)
    //{
    //    return context.methods.Single(m => m.Method == method);
    //}
}
