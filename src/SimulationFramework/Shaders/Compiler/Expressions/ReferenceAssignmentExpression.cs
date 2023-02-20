using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders.Compiler.Expressions;

// supports assigning ref type values (ie ref float/float&
public class ReferenceAssignmentExpression : Expression
{
    public Expression Left { get; internal set; }
    public Expression Right { get; internal set; }

    public override ExpressionType NodeType => ExpressionType.Extension;

    public ReferenceAssignmentExpression(Expression left, Expression right)
    {
        Left = left;
        Right = right;
    }

    protected override Expression Accept(ExpressionVisitor visitor)
    {
        return base.Accept(visitor);
    }

    protected override Expression VisitChildren(ExpressionVisitor visitor)
    {
        Left = visitor.Visit(Left);
        Right = visitor.Visit(Right);

        return this;
    }
}
