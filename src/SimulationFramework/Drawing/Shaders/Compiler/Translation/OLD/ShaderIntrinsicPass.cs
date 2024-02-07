using SimulationFramework.Drawing.Shaders.Compiler.Expressions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Translation.OLD;

// replaces common methods with their shader intrinsic counterparts
internal class ShaderIntrinsicPass
{
    private static readonly Dictionary<MethodBase, MethodInfo> substitutions = new();

    public ShaderIntrinsicPass()
    {
        if (substitutions.Count is not 0)
            return;

        foreach (var method in typeof(ShaderIntrinsics).GetMethods())
        {
            var attribute = method.GetCustomAttribute<InterceptsAttribute>();

            if (attribute is null)
                continue;

            Type[] signature = method.GetParameters().Select(p => p.ParameterType).ToArray();

            MethodBase targetMethod;

            if (attribute.MethodName is InterceptsAttribute.ConstructorName)
            {
                targetMethod = attribute.MethodType.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, signature);
            }
            else
            {
                targetMethod = attribute.MethodType.GetMethod(attribute.MethodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance, signature);
            }

            if (targetMethod is null)
            {
                Console.WriteLine("Error with intrinsic method: " + method.ToString());
                continue;
            }
            substitutions.Add(targetMethod, method);
        }
    }


    class Visitor : ExpressionVisitor
    {
        public override Expression VisitCallExpression(CallExpression expression)
        {
            if (substitutions.TryGetValue(expression.Callee, out MethodInfo? intrinsic))
                return new CallExpression(null, intrinsic, expression.Arguments);
            return base.VisitCallExpression(expression);
        }

        public override Expression VisitNewExpression(NewExpression expression)
        {
            if (substitutions.TryGetValue(expression.Constructor, out MethodInfo? intrinsic))
                return new CallExpression(null, intrinsic, expression.Arguments);
            return base.VisitNewExpression(expression);
        }
    }


    //public override void CheckMethod(CompilationContext context, ShaderMethod compiledMethod)
    //{
    //    var visitor = new ShaderIntrinsicReplacementVisitor();
    //    compiledMethod.TransformBody(visitor);

    //    base.CheckMethod(context, compiledMethod);
    //}

    //private class ShaderIntrinsicReplacementVisitor : ExpressionVisitor
    //{
    //    protected override Expression VisitNew(NewExpression node)
    //    {
    //        if (substitutions.ContainsKey(node.Constructor))
    //            return Expression.Call(substitutions[node.Constructor], Visit(node.Arguments));

    //        return base.VisitNew(node);
    //    }

    //    protected override Expression VisitMethodCall(MethodCallExpression node)
    //    {
    //        if (substitutions.ContainsKey(node.Method))
    //            return Expression.Call(substitutions[node.Method], Visit(node.Arguments));

    //        return base.VisitMethodCall(node);
    //    }
    //}
}
