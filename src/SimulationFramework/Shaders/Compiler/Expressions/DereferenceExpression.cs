using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders.Compiler.Expressions;
public class DereferenceExpression : Expression
{
    public Expression Operand { get; private set; }

    public DereferenceExpression(Expression operand)
    {
        Operand = operand;
    }

    public override ExpressionType NodeType => ExpressionType.Extension;
    public override Type Type => Operand.Type.GetElementType() ?? Operand.Type;

    protected override Expression Accept(ExpressionVisitor visitor)
    {
        return base.Accept(visitor);
    }

    protected override Expression VisitChildren(ExpressionVisitor visitor)
    {
        Operand = visitor.Visit(Operand);
        return this;
    }
}
