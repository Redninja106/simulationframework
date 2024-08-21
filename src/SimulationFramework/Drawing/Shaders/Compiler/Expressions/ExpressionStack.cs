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
    public Stack<ShaderExpression> expressions = [];
    public Stack<ShaderExpression> statements = [];

    public void Push(ShaderExpression expression)
    {
        Debug.Assert(!(expression is BinaryExpression bin && bin.Operation == BinaryOperation.Assignment));
        expressions.Push(expression);
    }

    public void PushStatement(ShaderExpression expression)
    {
        statements.Push(expression);
    }

    public ShaderExpression Pop()
    {
        return expressions.Pop();
    }

    public void Clear()
    {
        expressions.Clear();
        statements.Clear();
    }

    public IEnumerable<ShaderExpression> GetExpressions()
    {
        Debug.Assert(expressions.Count <= 1);
        IEnumerable<ShaderExpression> result = statements.Reverse();
        if (expressions.Count == 1)
        {
            result = result.Append(expressions.Peek());
        }
        return result;
    }
}
