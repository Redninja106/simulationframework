using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Expressions;
public record BreakExpression : ShaderExpression
{
    public override ShaderType? ExpressionType => null;

    public override ShaderExpression Accept(ShaderExpressionVisitor visitor) => visitor.VisitBreakExpression(this);
    public override ShaderExpression VisitChildren(ShaderExpressionVisitor visitor) => this;

    public override string ToString()
    {
        return "break";
    }
}
