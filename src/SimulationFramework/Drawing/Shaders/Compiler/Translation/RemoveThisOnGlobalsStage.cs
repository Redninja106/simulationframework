using SimulationFramework.Drawing.Shaders.Compiler.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Translation;
internal class RemoveThisOnGlobalsStage : CompilerStage
{
    public RemoveThisOnGlobalsStage(CompilerContext context) : base(context)
    {
    }

    public override void Run(ShaderCompilation compilation)
    {
        foreach (var method in compilation.Methods)
        {
            if (method.IsStatic)
                continue;

            var param = method.Parameters.First();
            if (param.ParameterType == context.ShaderType && param.ParameterInfo is null)
            {
                method.Parameters.Remove(param);
                RemoveThisFromCallSitesVisitor visitor = new(param);
                method.VisitBody(visitor);
            }
        }
    }

    class RemoveThisFromCallSitesVisitor : ExpressionVisitor
    {
        private MethodParameter param;

        public RemoveThisFromCallSitesVisitor(MethodParameter param)
        {
            this.param = param;
        }

        public override Expression VisitShaderCallExpression(ShaderCallExpression expression)
        {
            if (expression.Arguments[0] is MethodParameterExpression paramExpr && paramExpr.Parameter == param)
            {
                return base.VisitShaderCallExpression(new(expression.Callee, expression.Arguments.Skip(1).ToList()));
            }

            return base.VisitShaderCallExpression(expression);
        }
    }
}
