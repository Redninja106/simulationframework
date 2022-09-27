using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders.Compiler.Expressions;
public class ConstructorCallExpression : Expression, IArgumentProvider
{
    public override ExpressionType NodeType => ExpressionType.Extension;
    public override Type Type => Constructor.DeclaringType;
    public IEnumerable<Expression> Arguments => args;

    public ConstructorInfo Constructor { get; private set; }

    private Expression[] args;

    public int ArgumentCount => args.Length;

    public ConstructorCallExpression(ConstructorInfo constructor, IEnumerable<Expression> arguments) : base()
    {
        Constructor = constructor;
        args = arguments.ToArray();
    }

    public Expression GetArgument(int index)
    {
        return args[index];
    }

    protected override Expression VisitChildren(ExpressionVisitor visitor)
    {
        for (int i = 0; i < args.Length; i++)
        {
            args[i] = visitor.Visit(args[i]);
        }

        return this;
    }
}
