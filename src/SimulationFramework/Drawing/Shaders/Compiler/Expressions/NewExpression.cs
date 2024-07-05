using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Expressions;
public record NewExpression(ConstructorInfo Constructor, IReadOnlyList<ShaderExpression> Arguments) : ShaderExpression
{
    public override ShaderExpression Accept(ExpressionVisitor visitor)
    {
        return visitor.VisitNewExpression(this);
    }

    public override ShaderExpression VisitChildren(ExpressionVisitor visitor)
    {
        return new NewExpression(Constructor, Arguments.Select(e => e.Accept(visitor)).ToArray());
    }

    public override string ToString()
    {
        return $"new {Constructor.DeclaringType.Name}({string.Join(", ", Arguments.Select(a => a.ToString()))})";
    }
}
