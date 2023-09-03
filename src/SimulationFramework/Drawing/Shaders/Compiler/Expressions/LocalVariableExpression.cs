using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Expressions;
public record LocalVariableExpression(LocalVariableInfo LocalVariableInfo) : Expression
{
    public override Expression Accept(ExpressionVisitor visitor) => visitor.VisitLocalVariableExpression(this);

    public override string ToString()
    {
        return "var" + LocalVariableInfo.LocalIndex;
    }
}
