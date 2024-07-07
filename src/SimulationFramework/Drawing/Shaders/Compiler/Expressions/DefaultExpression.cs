using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Expressions;
public record DefaultExpression(ShaderType Type) : ShaderExpression
{
    public override ShaderType? ExpressionType => Type;

    public override ShaderExpression Accept(ShaderExpressionVisitor visitor) => visitor.VisitDefaultExpression(this);
}
