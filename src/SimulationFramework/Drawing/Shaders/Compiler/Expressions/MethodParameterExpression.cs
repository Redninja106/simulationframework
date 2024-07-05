using SimulationFramework.Drawing.Shaders.Compiler.ILDisassembler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Expressions;
public record MethodParameterExpression : ShaderExpression
{
    MethodParameter Parameter { get; }

    internal MethodParameterExpression(MethodDisassembly method, ParameterInfo info)
    {
        Parameter = new(method, info);
    }

    public override ShaderExpression Accept(ExpressionVisitor visitor)
    {
        return visitor.VisitMethodParameterExpression(this);
    }

    public override string ToString()
    {
        return Parameter.Name;
    }
}
