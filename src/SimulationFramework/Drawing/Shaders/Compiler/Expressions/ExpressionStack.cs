using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Expressions;
internal class ExpressionStack : IEnumerable<Expression>
{
    private Stack<(Expression expr, bool isOnIlStack)> values = new();

    public void Push(Expression expression, bool isOnStack = true)
    {
        values.Push((expression, isOnStack));
    }

    public Expression Pop()
    {
        List<Expression> notOnStackValues = new();

        Expression expr;
        bool isOnStack;
        while (true)
        {
            (expr, isOnStack) = values.Pop();

            if (isOnStack)
            {
                break;
            }
            else
            {
                notOnStackValues.Add(expr);
            }
        }

        foreach (var e in notOnStackValues)
        {
            values.Push((e, false));
        }

        return expr;
    }

    public void Clear()
    {
        values.Clear();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<Expression> GetEnumerator()
    {
        return values.Select(e => e.expr).GetEnumerator();
    }
}
