using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Expressions;
public record ShaderVariableExpression(ShaderVariable Variable) : ShaderExpression
{
    public override ShaderType? ExpressionType => Variable.Type;

    public override ShaderExpression Accept(ShaderExpressionVisitor visitor)
    {
        return visitor.VisitShaderVariableExpression(this);
    }

    public override string ToString()
    {
        return Variable.Name.ToString();
    }
}
