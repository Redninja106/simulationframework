using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Expressions;
public record ShaderIntrinsicCall(MethodInfo Intrinsic, IReadOnlyList<ShaderExpression> Arguments) : ShaderExpression
{
    public override ShaderExpression Accept(ExpressionVisitor visitor)
    {
        return visitor.VisitShaderIntrinsicCall(this);
    }
}
