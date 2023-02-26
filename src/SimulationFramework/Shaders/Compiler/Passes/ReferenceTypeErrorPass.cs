using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders.Compiler.Passes;
internal class ReferenceTypeErrorPass : CompilerPass
{
    public override void CheckMethod(CompilationContext context, CompiledMethod compiledMethod)
    {
        CheckType(context, compiledMethod.ReturnType);

        foreach (var parameter in compiledMethod.Method.GetParameters())
        {
            CheckType(context, parameter.ParameterType);
        }

        var visitor = new ReferenceTypeErrorVisitor(context);
        visitor.Visit(compiledMethod.Body);

        base.CheckMethod(context, compiledMethod);
    }

    private static void CheckType(CompilationContext context, Type type)
    {
        Debug.Assert(type.IsValueType, "Reference Types not allowed!");
    }

    private sealed class ReferenceTypeErrorVisitor : ExpressionVisitor
    {
        private CompilationContext context;

        public ReferenceTypeErrorVisitor(CompilationContext context)
        {
            this.context = context;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            CheckType(context, node.Type);

            return base.VisitParameter(node);
        }
    }
}
