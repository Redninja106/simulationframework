namespace SimulationFramework.Drawing.Shaders.Compiler.Expressions;

public record ReturnExpression(ShaderExpression? ReturnValue) : ShaderExpression
{
    public override ShaderType? ExpressionType => null;

    public override ShaderExpression VisitChildren(ShaderExpressionVisitor visitor)
    {
        return new ReturnExpression(ReturnValue?.Accept(visitor));
    }

    public override ShaderExpression Accept(ShaderExpressionVisitor visitor)
    {
        return visitor.VisitReturnExpression(this);
    }

    public override string ToString()
    {
        return $"return {ReturnValue}";
    }
}