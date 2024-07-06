using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Expressions;
public record UnaryExpression(UnaryOperation Operation, ShaderExpression Operand, ShaderType? CastType) : ShaderExpression
{
    public override ShaderType? ExpressionType => CastType ?? Operand.ExpressionType;

    public override ShaderExpression Accept(ExpressionVisitor visitor)
    {
        return visitor.VisitUnaryExpression(this);
    }

    public override ShaderExpression VisitChildren(ExpressionVisitor visitor)
    {
        return new UnaryExpression(Operation, Operand.Accept(visitor), CastType);
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
