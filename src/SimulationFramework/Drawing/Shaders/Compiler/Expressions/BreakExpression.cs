using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Expressions;
public record BreakExpression : Expression
{
    public override Expression Accept(ExpressionVisitor visitor) => visitor.VisitBreakExpression(this);
    public override Expression VisitChildren(ExpressionVisitor visitor) => this;

    public override string ToString()
    {
        return "break";
    }
}
