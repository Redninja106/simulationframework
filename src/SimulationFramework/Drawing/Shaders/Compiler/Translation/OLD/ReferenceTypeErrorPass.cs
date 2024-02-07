using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Translation.OLD;
internal class ReferenceTypeErrorPass : CompilerPass
{
    //    public override void CheckMethod(CompilationContext context, ShaderMethod compiledMethod)
    //    {
    //        CheckType(context, compiledMethod.ReturnType);

    //        foreach (var parameter in compiledMethod.Method.GetParameters())
    //        {
    //            CheckType(context, parameter.ParameterType);
    //        }

    //        var visitor = new ReferenceTypeErrorVisitor(context);
    //        visitor.Visit(compiledMethod.Body);

    //        base.CheckMethod(context, compiledMethod);
    //    }

    //    private static void CheckType(CompilationContext context, Type type)
    //    {
    //        Debug.Assert(type.IsValueType, "Reference Types not allowed!");
    //    }

    //    private sealed class ReferenceTypeErrorVisitor : ExpressionVisitor
    //    {
    //        private CompilationContext context;

    //        public ReferenceTypeErrorVisitor(CompilationContext context)
    //        {
    //            this.context = context;
    //        }

    //        protected override Expression VisitParameter(ParameterExpression node)
    //        {
    //            CheckType(context, node.Type);

    //            return base.VisitParameter(node);
    //        }
    //    }
}
