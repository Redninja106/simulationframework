using SimulationFramework.Drawing.Shaders.Compiler.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler;
internal class ConditionalCleanup : ShaderExpressionVisitor
{
    public static BlockExpression CleaupConditionals(BlockExpression expressions)
    {
        return (BlockExpression)expressions.Accept(new ConditionalCleanup());
    }

    public override ShaderExpression VisitConditionalExpression(ConditionalExpression expression)
    {
        if (IsEmpty(expression.Success) && !IsEmpty(expression.Failure))
        {
            expression = new ConditionalExpression(
                expression.IsTernary,
                ConditionalExpression.InvertCondition(expression.Condition), 
                expression.Failure,
                null
                );
        }

        return base.VisitConditionalExpression(expression);
    }

    private bool IsEmpty(ShaderExpression? expression)
    {
        if (expression is null)
            return true;

        if (expression is BlockExpression block && block.Expressions.Count is 0)
            return true;

        return false;
    }
}
