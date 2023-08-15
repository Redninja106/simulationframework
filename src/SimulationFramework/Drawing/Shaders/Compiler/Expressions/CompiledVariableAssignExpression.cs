using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Expressions;
public class CompiledVariableAssignmentExpression : Expression
{
    public CompiledVariableAssignmentExpression(CompiledVariableExpression left, Expression right)
    {
        Left = left;
        Right = right;
    }

    public override ExpressionType NodeType => ExpressionType.Extension;
    public override Type Type => Left.Type;

    public CompiledVariableExpression Left { get; private set; }
    public Expression Right { get; private set; }

    protected override Expression VisitChildren(ExpressionVisitor visitor)
    {
        Left = (CompiledVariableExpression)visitor.Visit(Left);
        Right = visitor.Visit(Right);
        return this;
    }
}
