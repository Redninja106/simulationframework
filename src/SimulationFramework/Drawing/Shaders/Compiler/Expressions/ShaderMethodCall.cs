using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Expressions;
public record ShaderMethodCall(ShaderMethod Callee, IReadOnlyList<ShaderExpression> Arguments) : ShaderExpression
{
    public override ShaderType? ExpressionType => Callee.ReturnType;

    public override ShaderExpression Accept(ExpressionVisitor visitor) => visitor.VisitShaderMethodCall(this);

    public override ShaderExpression VisitChildren(ExpressionVisitor visitor)
    {
        return new ShaderMethodCall(Callee, Arguments.Select(arg => arg.Accept(visitor)).ToList());
    }

    public override string ToString()
    {
        return $"{Callee.Name}({string.Join(", ", Arguments.Select(e => e.ToString()))})";
    }
}
