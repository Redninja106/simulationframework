using SimulationFramework.Shaders.Compiler.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders.Compiler.Passes;

// converts this.* to CompiledVariableExpressions
internal class VariableAccessReplacements : CompilerPass
{
    CompilationContext context;

    public override void CheckMethod(CompilationContext context, CompiledMethod compiledMethod)
    {
        this.context = context;
        compiledMethod.TransformBody(this);

        base.CheckMethod(context, compiledMethod);
    }

    protected override Expression VisitMember(MemberExpression node)
    {
        if (node.Expression!.Type != context.ShaderType)
        {
            return base.VisitMember(node);
        }

        return new CompiledVariableExpression(context.AllVariables.Single(v => v.BackingField == node.Member));
    }

    protected override Expression VisitBinary(BinaryExpression node)
    {
        if (node.NodeType is not ExpressionType.Assign
            || node.Left is not MemberExpression memberExpression
            || memberExpression!.Member.DeclaringType != context.ShaderType)
        {
            return base.VisitBinary(node);
        }

        var compiledVarExpr = new CompiledVariableExpression(context.AllVariables.Single(v => v.BackingField == memberExpression.Member));
        return new CompiledVariableAssignmentExpression(compiledVarExpr, Visit(node.Right));
    }
}
