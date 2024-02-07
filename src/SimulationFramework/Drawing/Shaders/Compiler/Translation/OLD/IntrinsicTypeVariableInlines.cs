using SimulationFramework.Drawing.Shaders.Compiler.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Translation.OLD;
internal class IntrinsicTypeVariableInlines : CompilerPass
{
    // readonly Dictionary<ParameterExpression, CompiledVariableExpression> inlines = new();

    public IntrinsicTypeVariableInlines()
    {
    }

    //public override void CheckMethod(CompilationContext context, ShaderMethod compiledMethod)
    //{
    //    inlines.Clear();

    //    foreach (var local in compiledMethod.Body.Variables)
    //    {
    //        if (!CanvasShaderCompiler.IsTypeIntrinsic(local.Type))
    //            continue;

    //        var assignment = FindFirstAssignment(local, compiledMethod.Body);

    //        if (assignment is null)
    //            throw new Exception();

    //        compiledMethod.TransformBody(new LocalReplacementVisitor(local, assignment.RightOperand, assignment));
    //    }

    //    base.CheckMethod(context, compiledMethod);
    //}

    //private BinaryExpression FindFirstAssignment(ParameterExpression variable, System.Linq.Expressions.BlockExpression methodBody)
    //{
    //    foreach (var block in methodBody.Expressions)
    //    {
    //        if (block is not BinaryExpression binaryExpr)
    //            continue;

    //        if (binaryExpr.NodeType is not ExpressionType.Assign)
    //            continue;

    //        if (binaryExpr.LeftOperand != variable)
    //            continue;

    //        return binaryExpr;
    //    }

    //    return null;
    //}

    //class LocalReplacementVisitor : ExpressionVisitor
    //{
    //    readonly ParameterExpression toReplace;
    //    readonly Expression replaceWith;
    //    readonly BinaryExpression assignmentToRemove;

    //    public LocalReplacementVisitor(ParameterExpression toReplace, Expression replaceWith, BinaryExpression assignmentToRemove)
    //    {
    //        this.toReplace = toReplace;
    //        this.replaceWith = replaceWith;
    //        this.assignmentToRemove = assignmentToRemove;
    //    }

    //    protected override Expression VisitBlock(System.Linq.Expressions.BlockExpression node)
    //    {
    //        return node.Update(node.Variables.Where(v => v != toReplace), node.Expressions.Where(e => e != assignmentToRemove).Select(e => Visit(e)));
    //    }

    //    protected override Expression VisitParameter(ParameterExpression node)
    //    {
    //        if (node == toReplace)
    //            return replaceWith;

    //        return base.VisitParameter(node);
    //    }
    //}
}
