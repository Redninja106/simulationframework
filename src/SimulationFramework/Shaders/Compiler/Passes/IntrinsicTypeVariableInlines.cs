using SimulationFramework.Shaders.Compiler.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders.Compiler.Passes;
internal class IntrinsicTypeVariableInlines : CompilerPass
{
    readonly Dictionary<ParameterExpression, CompiledVariableExpression?> inlines = new();

    public IntrinsicTypeVariableInlines()
    {
    }

    public override void CheckMethod(CompilationContext context, CompiledMethod compiledMethod)
    {
        inlines.Clear();

        foreach (var local in compiledMethod.Body.Variables)
        {
            if (!ShaderCompiler.IsTypeIntrinsic(local.Type))
                continue;

            var assignment = FindFirstAssignment(local, compiledMethod.Body);

            if (assignment is null)
                throw new Exception();

            compiledMethod.TransformBody(new LocalReplacementVisitor(local, assignment.Right, assignment));
        }

        base.CheckMethod(context, compiledMethod);
    }

    private BinaryExpression? FindFirstAssignment(ParameterExpression variable, BlockExpression methodBody)
    {
        foreach (var block in methodBody.Expressions)
        {
            if (block is not BinaryExpression binaryExpr)
                continue;

            if (binaryExpr.NodeType is not ExpressionType.Assign)
                continue;

            if (binaryExpr.Left != variable)
                continue;

            return binaryExpr;
        }

        return null;
    }

    class LocalReplacementVisitor : ExpressionVisitor
    {
        readonly ParameterExpression toReplace;
        readonly Expression replaceWith;
        readonly BinaryExpression assignmentToRemove;

        public LocalReplacementVisitor(ParameterExpression toReplace, Expression replaceWith, BinaryExpression assignmentToRemove)
        {
            this.toReplace = toReplace;
            this.replaceWith = replaceWith;
            this.assignmentToRemove = assignmentToRemove;
        }

        protected override Expression VisitBlock(BlockExpression node)
        {
            return node.Update(node.Variables.Where(v => v != toReplace), node.Expressions.Where(e => e != assignmentToRemove).Select(e => this.Visit(e)));
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (node == toReplace)
                return replaceWith;

            return base.VisitParameter(node);
        }
    }
}
