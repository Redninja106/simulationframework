using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders.Compiler.Expressions;
public class CompiledVariableExpression : Expression
{
    public override ExpressionType NodeType => ExpressionType.Extension;
    public override Type Type => Variable.VariableType;
    public ShaderVariable Variable { get; private set; }
    public CompiledVariableExpression(ShaderVariable variable)
    {
        Variable = variable;
    }

    protected override Expression VisitChildren(ExpressionVisitor visitor)
    {
        return this;
    }
}