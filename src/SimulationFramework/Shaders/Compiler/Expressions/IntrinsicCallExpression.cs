using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders.Compiler.Expressions;

public class IntrinsicCallExpression : Expression
{
    public MethodInfo Method { get; }
    public Expression[] Arguments { get; }

    public override ExpressionType NodeType => ExpressionType.Extension;

    public override Type Type => Method.ReturnType;

    public IntrinsicCallExpression(MethodInfo method, IEnumerable<Expression> args)
    {
        Method = method;
        Arguments = args.ToArray();
    }

    protected override Expression VisitChildren(ExpressionVisitor visitor)
    {
        for (int i = 0; i < Arguments.Length; i++)
        {
            Arguments[i] = visitor.Visit(Arguments[i]);
        }

        return this;
    }
}