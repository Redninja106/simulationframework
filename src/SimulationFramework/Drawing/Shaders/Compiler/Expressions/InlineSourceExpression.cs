using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Expressions;
public record InlineSourceExpression(string Language, string Source) : Expression
{
    public override Expression Accept(ExpressionVisitor visitor) => visitor.VisitInlineSourceExpression(this);
    public override Expression VisitChildren(ExpressionVisitor visitor) => this;
}

public record InlineSourceExpression<T> : InlineSourceExpression
{
    public override Type? ExpressionType => typeof(T);

    public InlineSourceExpression(string language, string source) : base(language, source)
    {
    }
}
