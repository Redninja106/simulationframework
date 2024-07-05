using SimulationFramework.Drawing.Shaders.Compiler.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Translation;
internal abstract class IntrinsicMethodHandler
{
    public abstract ShaderExpression Handle(CallExpression expression);
}

class OperatorMethodHandler(BinaryOperation op) : IntrinsicMethodHandler
{
    public override ShaderExpression Handle(CallExpression expression)
    {
        return new BinaryExpression(op, expression.Arguments[0], expression.Arguments[1]);
    }
}
