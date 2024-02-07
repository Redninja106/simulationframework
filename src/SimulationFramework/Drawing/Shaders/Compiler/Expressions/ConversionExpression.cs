using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Expressions;
public record ConversionExpression : Expression
{
    public override Expression Accept(ExpressionVisitor visitor)
    {
        throw new NotImplementedException();
    }

    public override Expression VisitChildren(ExpressionVisitor visitor)
    {
        throw new NotImplementedException();
    }
}
