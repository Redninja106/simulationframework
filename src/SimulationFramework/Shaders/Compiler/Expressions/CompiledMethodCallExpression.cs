using SimulationFramework.Shaders.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders.Compiler.Expressions;

public class CompiledMethodCallExpression : Expression
{
    public CompiledMethod Method { get; private set; }
    public List<Expression> Arguments { get; private set; }

    public override Type Type => Method.ReturnType;
    public override ExpressionType NodeType => ExpressionType.Extension;

    public CompiledMethodCallExpression(CompiledMethod method, IEnumerable<Expression> arguments)
    {
        Method = method;
        Arguments = new(arguments);
    }

    protected override Expression VisitChildren(ExpressionVisitor visitor)
    {
        for (int i = 0; i < Arguments.Count; i++)
        {
            Arguments[i] = visitor.Visit(Arguments[i]);
        }

        return this;
    }
}
