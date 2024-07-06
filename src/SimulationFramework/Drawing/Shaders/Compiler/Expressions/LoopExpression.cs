using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Expressions;
public record LoopExpression(ShaderExpression Body) : ShaderExpression
{
    public override ShaderExpression Accept(ExpressionVisitor visitor)
    {
        return visitor.VisitLoopExpression(this);
    }

    public override ShaderExpression VisitChildren(ExpressionVisitor visitor)
    {
        return new LoopExpression(Body.Accept(visitor));
    }
}
