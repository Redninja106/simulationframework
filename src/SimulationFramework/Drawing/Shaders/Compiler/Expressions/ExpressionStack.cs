using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Expressions;

internal class ExpressionStack
{
    private Stack<(ShaderExpression expr, bool isOnIlStack)> values = new();

    public bool HasElementsOnStack => values.Any(v => v.isOnIlStack);

    public void Push(ShaderExpression expression)
    {
        values.Push((expression, true));
    }

    public void PushStatement(ShaderExpression expression)
    {
        values.Push((expression, false));
    }

    public ShaderExpression Pop()
    {
        List<ShaderExpression> notOnStackValues = new();

        ShaderExpression expr;
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

    public IEnumerable<ShaderExpression> GetExpressions()
    {
        return values.Select(v => v.expr).Reverse();
    }
}