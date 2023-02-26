using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders.Compiler.Passes;
internal class ConstructorPass : CompilerPass
{
    CompiledMethod currentMethod;
    ParameterExpression thisParam;

    public override void CheckMethod(CompilationContext context, CompiledMethod compiledMethod)
    {
        if (compiledMethod.Method is ConstructorInfo)
        {
            var thisParam = compiledMethod.Parameters[0];
            compiledMethod.Parameters.Remove(thisParam);
            compiledMethod.Body = compiledMethod.Body.Update(compiledMethod.Body.Variables.Prepend(thisParam), compiledMethod.Body.Expressions);

            this.thisParam = thisParam;
            this.currentMethod = compiledMethod;
            compiledMethod.TransformBody(this);

        }
        
        base.CheckMethod(context, compiledMethod);
    }

    protected override Expression VisitLabel(LabelExpression node)
    {
        return node.Update(node.Target, thisParam);
    }

    protected override Expression VisitNew(NewExpression node)
    {
        var currentType = currentMethod.Method.DeclaringType;
        if (node.Type == currentType && currentMethod.Body.Expressions.Contains(node))
        {
            return Expression.Assign(currentMethod.Body.Variables[0], node);
        }

        return base.VisitNew(node);
    }
}