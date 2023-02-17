using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders.Compiler.Expressions;
public class InlineSourceExpression : Expression
{
    public InlineSourceExpression(string source)
    {
        Source = source;
    }

    public override Type Type => typeof(void);
    public override ExpressionType NodeType => ExpressionType.Extension;

    public string Source { get; private set; }
}
public class InlineSourceExpression<T> : InlineSourceExpression
{
    public InlineSourceExpression(string source) : base(source)
    {
    }

    public override Type Type => typeof(T);
    public override ExpressionType NodeType => ExpressionType.Extension;
}
