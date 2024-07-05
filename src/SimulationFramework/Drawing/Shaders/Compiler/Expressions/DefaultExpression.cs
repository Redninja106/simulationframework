using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Expressions;
public record DefaultExpression(ShaderType Type) : ShaderExpression
{
    public override ShaderExpression Accept(ExpressionVisitor visitor) => visitor.VisitDefaultExpression(this);
}
