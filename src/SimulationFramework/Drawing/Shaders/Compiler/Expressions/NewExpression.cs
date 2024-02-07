using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Expressions;
public record NewExpression(ConstructorInfo Constructor, IReadOnlyList<Expression> Arguments) : Expression
{
    public override Expression Accept(ExpressionVisitor visitor)
    {
        return visitor.VisitNewExpression(this);
    }

    public override Expression VisitChildren(ExpressionVisitor visitor)
    {
        return new NewExpression(Constructor, Arguments.Select(e => e.Accept(visitor)).ToArray());
    }

    public override string ToString()
    {
        return $"new {Constructor.DeclaringType.Name}({string.Join(", ", Arguments.Select(a => a.ToString()))})";
    }
}
