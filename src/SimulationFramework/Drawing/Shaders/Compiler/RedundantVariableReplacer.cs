using SimulationFramework.Drawing.Shaders.Compiler.Expressions;
using System.Collections.Immutable;
using System.Diagnostics;

namespace SimulationFramework.Drawing.Shaders.Compiler;

class RedundantVariableReplacer : ShaderExpressionVisitor
{
    internal Dictionary<ShaderVariable, ShaderExpression?> values = [];

    public RedundantVariableReplacer(IEnumerable<ShaderVariable> variables)
    {
        foreach (var value in variables)
        {
            values.Add(value, null);
        }
    }

    public static BlockExpression ReplaceRedundantVariables(BlockExpression expression, ref ShaderVariable[] locals)
    {
        RedundantVariableFinder redundantVariableFinder = new();
        expression.Accept(redundantVariableFinder);
        var redundants = redundantVariableFinder.GetRedundantVariables();

        RedundantVariableReplacer replacer = new(redundants);
        expression = (BlockExpression)expression.Accept(replacer);
        locals = locals.Except(redundants).ToArray();

        Debug.Assert(replacer.values.Count is 0);

        return expression;
    }

    public override ShaderExpression VisitBinaryExpression(BinaryExpression expression)
    {
        if (expression.Operation is BinaryOperation.Assignment)
        {
            if (expression.LeftOperand is ShaderVariableExpression varExpr)
            {
                if (values.ContainsKey(varExpr.Variable))
                {
                    values[varExpr.Variable] = expression.RightOperand.Accept(this);
                    return ShaderExpression.Empty;
                }
            }
        }

        return base.VisitBinaryExpression(expression);
    }

    public override ShaderExpression VisitShaderVariableExpression(ShaderVariableExpression expression)
    {
        if (values.Remove(expression.Variable, out ShaderExpression? value))
        {
            // expression was used but not assigned?
            if (value is null)
            {
                return base.VisitShaderVariableExpression(expression);
            }

            return value;
        }

        return base.VisitShaderVariableExpression(expression);
    }

    class RedundantVariableFinder : ShaderExpressionVisitor
    {

        /* 
         * redundant variables are variables that are both assigned and used exactly once.
         * these variables are usually not in the original source, instead being inserted by the compiler.
         */

        // TODO: BUG if the single usage is a member access on the lhs of an assignment, this causes improper codegen

        Dictionary<ShaderVariable, int> assignments = [];
        Dictionary<ShaderVariable, int> usages = [];

        public override ShaderExpression VisitBinaryExpression(BinaryExpression expression)
        {
            if (expression.Operation is BinaryOperation.Assignment)
            {
                if (expression.LeftOperand is ShaderVariableExpression variableExpr)
                {
                    if (assignments.TryGetValue(variableExpr.Variable, out int assigmentCount))
                    {
                        assignments[variableExpr.Variable] = assigmentCount + 1;
                    }
                    else
                    {
                        assignments[variableExpr.Variable] = 1;
                    }

                    expression.RightOperand.Accept(this);
                    return expression;
                }
            }

            return base.VisitBinaryExpression(expression);
        }

        public override ShaderExpression VisitShaderVariableExpression(ShaderVariableExpression expression)
        {
            if (usages.TryGetValue(expression.Variable, out int usageCount))
            {
                usages[expression.Variable] = usageCount + 1;
            }
            else
            {
                usages[expression.Variable] = 1;
            }
            return base.VisitShaderVariableExpression(expression);
        }

        public IEnumerable<ShaderVariable> GetRedundantVariables()
        {
            foreach (var (var, assignmentCount) in assignments)
            {
                if (assignmentCount == 1 && usages.GetValueOrDefault(var, 0) == 1)
                {
                    yield return var;
                }
            }
        }

    }
}