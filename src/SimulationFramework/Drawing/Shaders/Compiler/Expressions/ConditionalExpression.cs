using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Expressions;

public record ConditionalExpression(bool IsTernary, ShaderExpression Condition, ShaderExpression? Success, ShaderExpression? Failure) : ShaderExpression
{
    public override ShaderType? ExpressionType => null;

    public override ShaderExpression Accept(ShaderExpressionVisitor visitor) => visitor.VisitConditionalExpression(this);
    public override ShaderExpression VisitChildren(ShaderExpressionVisitor visitor)
    {
        return new ConditionalExpression(IsTernary, Condition.Accept(visitor), Success?.Accept(visitor), Failure?.Accept(visitor));
    }
}
