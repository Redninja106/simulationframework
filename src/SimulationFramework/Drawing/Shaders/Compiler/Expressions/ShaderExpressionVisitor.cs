using System;

namespace SimulationFramework.Drawing.Shaders.Compiler.Expressions;

public class ShaderExpressionVisitor
{
    public virtual ShaderExpression VisitBinaryExpression(BinaryExpression expression) => expression.VisitChildren(this);
    public virtual ShaderExpression VisitBlockExpression(BlockExpression expression) => expression.VisitChildren(this);
    public virtual ShaderExpression VisitBreakExpression(BreakExpression expression) => expression.VisitChildren(this);
    public virtual ShaderExpression VisitContinueExpression(ContinueExpression expression) => expression.VisitChildren(this);
    public virtual ShaderExpression VisitConditionalExpression(ConditionalExpression expression) => expression.VisitChildren(this);
    public virtual ShaderExpression VisitConstantExpression(ConstantExpression expression) => expression.VisitChildren(this);
    public virtual ShaderExpression VisitInlineSourceExpression(InlineSourceExpression expression) => expression.VisitChildren(this);
    public virtual ShaderExpression VisitLoopExpression(LoopExpression expression) => expression.VisitChildren(this);
    public virtual ShaderExpression VisitMemberAccess(MemberAccess expression) => expression.VisitChildren(this);
    public virtual ShaderExpression VisitReturnExpression(ReturnExpression expression) => expression.VisitChildren(this);
    public virtual ShaderExpression VisitUnaryExpression(UnaryExpression expression) => expression.VisitChildren(this);
    public virtual ShaderExpression VisitShaderMethodCall(ShaderMethodCall expression) => expression.VisitChildren(this);
    public virtual ShaderExpression VisitDefaultExpression(DefaultExpression expression) => expression.VisitChildren(this);
    public virtual ShaderExpression VisitShaderVariableExpression(ShaderVariableExpression expression) => expression.VisitChildren(this);
    public virtual ShaderExpression VisitShaderIntrinsicCall(ShaderIntrinsicCall expression) => expression.VisitChildren(this);
}