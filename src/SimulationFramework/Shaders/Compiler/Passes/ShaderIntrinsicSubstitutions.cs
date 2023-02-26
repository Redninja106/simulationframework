using SimulationFramework.Shaders;
using SimulationFramework.Shaders.Compiler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders.Compiler.Passes;

// replaces common methods with their shader intrinsic counterparts
internal class ShaderIntrinsicSubstitutions : CompilerPass
{
    private static readonly Dictionary<MethodBase, MethodInfo> substitutions = new();

    public ShaderIntrinsicSubstitutions()
    {
        if (substitutions.Count is not 0)
            return;

        foreach (var method in typeof(ShaderIntrinsics).GetMethods())
        {
            var attribute = method.GetCustomAttribute<ReplaceAttribute>();

            if (attribute is null)
                continue;

            Type[] signature = method.GetParameters().Select(p => p.ParameterType).ToArray();

            MethodBase targetMethod;

            if (attribute.MethodName is ReplaceAttribute.Constructor)
            {
                targetMethod = attribute.MethodType.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, signature);
            }
            else
            {
                targetMethod = attribute.MethodType.GetMethod(attribute.MethodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance, signature);
            }

            if (targetMethod is null)
            {
                Debug.Warn("Error with intrinsic method: " + method.ToString());
                continue;
            }
            substitutions.Add(targetMethod, method);
        }
    }

    public override void CheckMethod(CompilationContext context, CompiledMethod compiledMethod)
    {
        var visitor = new ShaderIntrinsicReplacementVisitor();
        compiledMethod.TransformBody(visitor);

        base.CheckMethod(context, compiledMethod);
    }

    private class ShaderIntrinsicReplacementVisitor : ExpressionVisitor
    {
        protected override Expression VisitNew(NewExpression node)
        {
            if (substitutions.ContainsKey(node.Constructor))
                return Expression.Call(substitutions[node.Constructor], Visit(node.Arguments));

            return base.VisitNew(node);
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (substitutions.ContainsKey(node.Method))
                return Expression.Call(substitutions[node.Method], Visit(node.Arguments));

            return base.VisitMethodCall(node);
        }
    }
}
