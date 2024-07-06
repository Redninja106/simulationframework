using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Expressions;
public record InlineSourceExpression(string Language, string Source) : ShaderExpression
{
    public override ShaderType? ExpressionType => null;

    public override ShaderExpression Accept(ExpressionVisitor visitor) => visitor.VisitInlineSourceExpression(this);
    public override ShaderExpression VisitChildren(ExpressionVisitor visitor) => this;
}
