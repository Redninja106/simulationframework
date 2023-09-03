using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Expressions;
public record MethodParameterExpression(MethodParameter Parameter) : Expression
{
    public override Expression Accept(ExpressionVisitor visitor)
    {
        return visitor.VisitMethodParameterExpression(this);
    }

    public override string ToString()
    {
        return Parameter.Name;
    }
}
