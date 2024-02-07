using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Expressions;
public record UnaryExpression(UnaryOperation Operation, Expression Operand) : Expression
{
    public override Expression Accept(ExpressionVisitor visitor)
    {
        return visitor.VisitUnaryExpression(this);
    }

    public override Expression VisitChildren(ExpressionVisitor visitor)
    {
        return new UnaryExpression(Operation, Operand.Accept(visitor));
    }

    public string GetOperator()
    {
        return Operation switch
        {
            UnaryOperation.Negate => "-",
            UnaryOperation.Not => "!",
        };
    }

    public override string ToString()
    {
        string op = GetOperator();
        return $"{op}{Operand}";
    }
}
