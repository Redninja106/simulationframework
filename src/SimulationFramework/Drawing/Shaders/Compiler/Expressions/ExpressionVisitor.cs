using System;

namespace SimulationFramework.Drawing.Shaders.Compiler.Expressions;

public class ExpressionVisitor
{
    public virtual Expression VisitBinaryExpression(BinaryExpression expression) => expression.VisitChildren(this);
    public virtual Expression VisitBlockExpression(BlockExpression expression) => expression.VisitChildren(this);
    public virtual Expression VisitBreakExpression(BreakExpression expression) => expression.VisitChildren(this);
    public virtual Expression VisitCallExpression(CallExpression expression) => expression.VisitChildren(this);
    public virtual Expression VisitConditionalExpression(ConditionalExpression expression) => expression.VisitChildren(this);
    public virtual Expression VisitConstantExpression(ConstantExpression expression) => expression.VisitChildren(this);
    public virtual Expression VisitConversionExpression(ConversionExpression expression) => expression.VisitChildren(this);
    public virtual Expression VisitInlineSourceExpression(InlineSourceExpression expression) => expression.VisitChildren(this);
    public virtual Expression VisitLocalVariableExpression(LocalVariableExpression expression) => expression.VisitChildren(this);
    public virtual Expression VisitLoopExpression(LoopExpression expression) => expression.VisitChildren(this);
    public virtual Expression VisitMemberAccessExpression(MemberAccessExpression expression) => expression.VisitChildren(this);
    public virtual Expression VisitMethodParameterExpression(MethodParameterExpression expression) => expression.VisitChildren(this);
    public virtual Expression VisitNewExpression(NewExpression expression) => expression.VisitChildren(this);
    public virtual Expression VisitReturnExpression(ReturnExpression expression) => expression.VisitChildren(this);
    public virtual Expression VisitUnaryExpression(UnaryExpression expression) => expression.VisitChildren(this);
    public virtual Expression VisitShaderCallExpression(ShaderCallExpression expression) => expression.VisitChildren(this);
    public virtual Expression VisitUniformExpression(UniformExpression expression) => expression.VisitChildren(this);
}