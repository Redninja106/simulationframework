using SimulationFramework.Drawing.Shaders.Compiler.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Translation;
internal class CallReplacementsStage : CompilerStage
{
    public CallReplacementsStage(CompilerContext context) : base(context)
    {
    }

    public override void Run(ShaderCompilation compilation)
    {
        Visitor visitor = new(context, compilation);
        foreach (var method in compilation.Methods)
        {
            method.VisitBody(visitor);
        }
    }

    class Visitor : ExpressionVisitor
    {
        private CompilerContext context;
        private ShaderCompilation compilation;

        public Visitor(CompilerContext context, ShaderCompilation compilation)
        {
            this.context = context;
            this.compilation = compilation;
        }

        public override Expression VisitNewExpression(NewExpression expression)
        {
            var method = compilation.GetMethod(expression.Constructor);
            return base.VisitShaderCallExpression(new ShaderCallExpression(method, expression.Arguments));
        }

        public override Expression VisitCallExpression(CallExpression expression)
        {
            if (context.Intrinsics.IsIntrinsic(expression.Callee))
            {
                return base.VisitCallExpression(expression);
            }

            var method = compilation.GetMethod(expression.Callee);
            return base.VisitShaderCallExpression(new ShaderCallExpression(method, expression.Instance is null ? expression.Arguments : expression.Arguments.Prepend(expression.Instance).ToList()));
        }
    }
}
