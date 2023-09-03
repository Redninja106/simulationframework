using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Expressions;
public record ShaderCallExpression(ShaderMethod Callee, IReadOnlyList<Expression> Arguments) : Expression
{
    public override Expression Accept(ExpressionVisitor visitor) => visitor.VisitShaderCallExpression(this);

    public override Expression VisitChildren(ExpressionVisitor visitor)
    {
        return new ShaderCallExpression(Callee, Arguments.Select(arg => arg.Accept(visitor)).ToList());
    }

    public override string ToString()
    {
        return $"{Callee.FullName}({string.Join(", ", Arguments.Select(e => e.ToString()))})";
    }
}
