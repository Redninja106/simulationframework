
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Expressions;
public record BlockExpression(IReadOnlyList<Expression> Expressions) : Expression
{
    public override Type? ExpressionType => Expressions.Last().ExpressionType;

    public override Expression Accept(ExpressionVisitor visitor)
    {
        return visitor.VisitBlockExpression(this);
    }

    public override Expression VisitChildren(ExpressionVisitor visitor)
    {
        return new BlockExpression(Expressions.Select(e => e.Accept(visitor)).ToArray());
    }

    public override string ToString()
    {
        return @$"{{
    {Expressions.Aggregate("", (s, e) => $"{s}\n    {e}")}
}}";
    }
}
