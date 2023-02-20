using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders.Compiler.Expressions;
public abstract class ExtendedExpressionVisitor : ExpressionVisitor
{
    protected override Expression VisitExtension(Expression node)
    {
        if (node is CompiledMethodCallExpression methodCall)
        {
            return VisitCompiledMethodCallExpression(methodCall);
        }

        if (node is CompiledVariableAssignmentExpression variableAssignmentExpression)
        {
            return VisitCompiledVariableAssignmentExpression(variableAssignmentExpression);
        }

        if (node is CompiledVariableExpression variableExpression)
        {
            return VisitCompiledVariableExpression(variableExpression);
        }

        if (node is ConstructorCallExpression constructorCallExpression)
        {
            return VisitConstructorCallExpression(constructorCallExpression);
        }

        if (node is DereferenceExpression dereferenceExpression)
        {
            return VisitDereferenceExpression(dereferenceExpression);
        }

        if (node is InlineSourceExpression inlineSourceExpression)
        {
            return VisitInlineSourceExpression(inlineSourceExpression);
        }

        if (node is IntrinsicCallExpression intrinsicCallExpression)
        {
            return VisitIntrinsicCallExpression(intrinsicCallExpression);
        }

        if (node is ReferenceAssignmentExpression referenceAssignmentExpression)
        {
            return VisitReferenceAssignmentExpression(referenceAssignmentExpression);
        }

        return base.VisitExtension(node);
    }

    protected virtual Expression VisitIntrinsicCallExpression(IntrinsicCallExpression intrinsicCallExpression)
    {
        return intrinsicCallExpression;
    }

    protected virtual Expression VisitReferenceAssignmentExpression(ReferenceAssignmentExpression referenceAssignmentExpression)
    {
        return referenceAssignmentExpression;
    }

    protected virtual Expression VisitInlineSourceExpression(InlineSourceExpression inlineSourceExpression)
    {
        return inlineSourceExpression;
    }

    protected virtual Expression VisitDereferenceExpression(DereferenceExpression dereferenceExpression)
    {
        return dereferenceExpression;
    }

    protected virtual Expression VisitConstructorCallExpression(ConstructorCallExpression constructorCallExpression)
    {
        return constructorCallExpression;
    }

    protected virtual Expression VisitCompiledVariableExpression(CompiledVariableExpression variableExpression)
    {
        return variableExpression;
    }

    protected virtual Expression VisitCompiledVariableAssignmentExpression(CompiledVariableAssignmentExpression variableAssignmentExpression)
    {
        return variableAssignmentExpression;
    }

    protected virtual Expression VisitCompiledMethodCallExpression(CompiledMethodCallExpression methodCall)
    {
        return methodCall;
    }
}
