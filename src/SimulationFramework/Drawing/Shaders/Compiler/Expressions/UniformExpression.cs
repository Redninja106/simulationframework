using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Expressions;
public record UniformExpression(ShaderUniform Uniform) : Expression
{
    public override Expression Accept(ExpressionVisitor visitor)
    {
        return visitor.VisitUniformExpression(this);
    }
}
