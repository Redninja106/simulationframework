using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Expressions;
public record CallExpression(ShaderExpression? Instance, MethodInfo Callee, IReadOnlyList<ShaderExpression> Arguments) : ShaderExpression
{
    public override Type? ExpressionType => Callee.ReturnType;


    public override ShaderExpression VisitChildren(ExpressionVisitor visitor)
    {
        return new CallExpression(Instance?.Accept(visitor), Callee, Arguments.Select(e => e.Accept(visitor)).ToArray());
    }

    public override ShaderExpression Accept(ExpressionVisitor visitor) => visitor.VisitCallExpression(this);

    public override string ToString()
    {
        return $"{Instance?.ToString() ?? Callee.DeclaringType!.Name}.{Callee.Name}({string.Join(", ", Arguments.Select(a => a.ToString()))})";
    }
}
