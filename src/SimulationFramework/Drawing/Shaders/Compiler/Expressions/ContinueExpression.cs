namespace SimulationFramework.Drawing.Shaders.Compiler.Expressions;


public record ContinueExpression : ShaderExpression
{
    public override ShaderType? ExpressionType => null;

    public override ShaderExpression Accept(ShaderExpressionVisitor visitor) => visitor.VisitContinueExpression(this);
    public override ShaderExpression VisitChildren(ShaderExpressionVisitor visitor) => this;

    public override string ToString()
    {
        return "continue";
    }
}
