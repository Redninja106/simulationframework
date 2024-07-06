using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Expressions;
public record BreakExpression : ShaderExpression
{
    public override ShaderType? ExpressionType => null;

    public override ShaderExpression Accept(ExpressionVisitor visitor) => visitor.VisitBreakExpression(this);
    public override ShaderExpression VisitChildren(ExpressionVisitor visitor) => this;

    public override string ToString()
    {
        return "break";
    }
}
