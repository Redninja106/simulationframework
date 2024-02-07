using System;

using System.Reflection;

namespace SimulationFramework.Drawing.Shaders.Compiler.Translation.OLD;

class ReplacementsRule : CompilerPass
{
    private TranslationProfile profile;

    public ReplacementsRule(TranslationProfile profile)
    {
        this.profile = profile;
    }

    //public override void CheckMethod(CompilationContext context, ShaderMethod compiledMethod)
    //{
    //    this.Visit(compiledMethod.Body);
    //}

    //protected override Expression VisitMethodCall(MethodCallExpression node)
    //{
    //    if (profile.methods.TryGetValue(node.Method, out MethodBase? value))
    //    {
    //        var replacement = value as MethodInfo ?? throw new Exception();
    //        return Expression.Call(node.Object, replacement, node.Arguments);
    //    }

    //    return base.VisitMethodCall(node);
    //}

    //protected override Expression VisitNew(NewExpression node)
    //{
    //    if (profile.methods.TryGetValue(node.Constructor!, out MethodBase? replacement))
    //    {
    //        if (replacement is ConstructorInfo constructor)
    //        {
    //            return Expression.New(constructor, node.Arguments);
    //        }
    //        else if (replacement is MethodInfo method)
    //        {
    //            return Expression.Call(method, node.Arguments);
    //        }
    //        else
    //        {
    //            throw new Exception();
    //        }
    //    }

    //    return base.VisitNew(node);
    //}
}
