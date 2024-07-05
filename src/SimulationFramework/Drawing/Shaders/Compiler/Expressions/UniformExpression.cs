using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Expressions;
public record UniformExpression(ShaderVariable Uniform) : ShaderExpression
{
    public override ShaderExpression Accept(ExpressionVisitor visitor)
    {
        return visitor.VisitUniformExpression(this);
    }
}
