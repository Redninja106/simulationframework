using SimulationFramework.Drawing.Shaders.Compiler.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler;
internal class TernaryBlockRemover : ShaderExpressionVisitor
{
    public static BlockExpression RemoveBlocksFromTernaryExpressions(BlockExpression expressions)
    {
        var remover = new TernaryBlockRemover();
        return (BlockExpression)expressions.Accept(remover);
    }

    public override ShaderExpression VisitBlockExpression(BlockExpression expression)
    {
        if (expression.Expressions.Count != 0 && expression.Expressions[^1] is ReturnExpression ret && ret.ReturnValue is ConditionalExpression cond)
        {
            var newCond = new ConditionalExpression(
                false,
                cond.Condition,
                cond.Success is null ? null : AddReturn(cond.Success),
                cond.Failure is null ? null : AddReturn(cond.Failure)
                );

            var newBlock = new BlockExpression(expression.Expressions.SkipLast(1).Append(newCond).ToList());

            return base.VisitBlockExpression(newBlock);
        }

        return base.VisitBlockExpression(expression);
    }

    private ShaderExpression AddReturn(ShaderExpression expression)
    {
        if (expression is BlockExpression block)
        {
            if (block.Expressions[^1] is not ReturnExpression)
            {
                return new BlockExpression(
                    block.Expressions
                    .SkipLast(1)
                    .Append(new ReturnExpression(block.Expressions[^1]))
                    .ToList()
                    );
            }
            else
            {
                return expression; // we already have a return!
            }
        }
        else if (expression is ReturnExpression)
        {
            return new BlockExpression([expression]);
        }
        else
        {
            return new BlockExpression([new ReturnExpression(expression)]);
        }
    }

}
