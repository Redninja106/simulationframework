using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Expressions;
public record DefaultExpression(Type Type) : Expression
{
    public override Type? ExpressionType => Type;

    public override Expression Accept(ExpressionVisitor visitor) => visitor.VisitDefaultExpression(this);
}
