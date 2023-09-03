using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Expressions;
public record LoopExpression(Expression Body) : Expression
{
    public override Expression Accept(ExpressionVisitor visitor)
    {
        return new LoopExpression(Body.Accept(visitor));
    }

    public override Expression VisitChildren(ExpressionVisitor visitor)
    {
        return base.VisitChildren(visitor);
    }
}
