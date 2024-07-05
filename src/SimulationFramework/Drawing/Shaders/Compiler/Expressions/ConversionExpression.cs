using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Expressions;
public record ConversionExpression : ShaderExpression
{
    public override ShaderExpression Accept(ExpressionVisitor visitor)
    {
        throw new NotImplementedException();
    }

    public override ShaderExpression VisitChildren(ExpressionVisitor visitor)
    {
        throw new NotImplementedException();
    }
}
