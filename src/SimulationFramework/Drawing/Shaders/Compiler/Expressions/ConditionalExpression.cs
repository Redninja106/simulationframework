using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Expressions;

public record ConditionalExpression(bool IsTernary, ShaderExpression Condition, ShaderExpression? Success, ShaderExpression? Failure) : ShaderExpression
{
    public override ShaderType? ExpressionType => IsTernary ? Success.ExpressionType : null;

    public override ShaderExpression Accept(ShaderExpressionVisitor visitor) => visitor.VisitConditionalExpression(this);
    public override ShaderExpression VisitChildren(ShaderExpressionVisitor visitor)
    {
        return new ConditionalExpression(IsTernary, Condition.Accept(visitor), Success?.Accept(visitor), Failure?.Accept(visitor));
    }

    public static ShaderExpression InvertCondition(ShaderExpression condition)
    {
        if (condition is UnaryExpression unary && unary.Operation is UnaryOperation.Not)
        {
            return unary.Operand;
        }

        if (condition is BinaryExpression bin)
        {
            BinaryOperation? inverseComparison = bin.Operation switch
            {
                BinaryOperation.NotEqual => BinaryOperation.Equal,
                BinaryOperation.Equal => BinaryOperation.NotEqual,
                BinaryOperation.LessThan => BinaryOperation.GreaterThanEqual,
                BinaryOperation.LessThanEqual => BinaryOperation.GreaterThan,
                BinaryOperation.GreaterThan => BinaryOperation.LessThanEqual,
                BinaryOperation.GreaterThanEqual => BinaryOperation.LessThan,
                _ => null
            };

            if (inverseComparison != null)
            {
                return new BinaryExpression(
                    inverseComparison.Value, 
                    bin.LeftOperand, 
                    bin.RightOperand
                    );
            }
        }

        return new UnaryExpression(
            UnaryOperation.Not,
            condition,
            null
            );
    }
}
