namespace SimulationFramework.Drawing.Shaders.Compiler.Expressions;

public record ReturnExpression(Expression? ReturnValue) : Expression
{
    public override Expression VisitChildren(ExpressionVisitor visitor)
    {
        return new ReturnExpression(ReturnValue?.Accept(visitor));
    }

    public override Expression Accept(ExpressionVisitor visitor)
    {
        return visitor.VisitReturnExpression(this);
    }

    public override string ToString()
    {
        return $"return {ReturnValue}";
    }
}