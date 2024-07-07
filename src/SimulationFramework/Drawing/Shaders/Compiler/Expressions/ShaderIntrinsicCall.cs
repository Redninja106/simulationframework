using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Expressions;
public record ShaderIntrinsicCall(MethodInfo Intrinsic, ShaderType ReturnType, IReadOnlyList<ShaderExpression> Arguments) : ShaderExpression
{
    public override ShaderType? ExpressionType => ReturnType;

    public override ShaderExpression Accept(ShaderExpressionVisitor visitor)
    {
        return visitor.VisitShaderIntrinsicCall(this);
    }

    public override ShaderExpression VisitChildren(ShaderExpressionVisitor visitor)
    {
        return new ShaderIntrinsicCall(
            Intrinsic,
            ReturnType,
            Arguments.Select(arg => arg.Accept(visitor)).ToList()
            );
    }
}
