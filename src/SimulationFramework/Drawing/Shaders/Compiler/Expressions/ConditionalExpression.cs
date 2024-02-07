using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Expressions;

public record ConditionalExpression(Expression Condition, Expression? Success, Expression? Failure) : Expression
{
    public override Expression Accept(ExpressionVisitor visitor) => visitor.VisitConditionalExpression(this);
    public override Expression VisitChildren(ExpressionVisitor visitor)
    {
        return new ConditionalExpression(Condition.Accept(visitor), Success?.Accept(visitor), Failure?.Accept(visitor));
    }
}
