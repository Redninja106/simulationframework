using SimulationFramework.Drawing.Shaders.Compiler.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Translation.OLD;

// converts this.* to CompiledVariableExpressions
internal class VariableAccessReplacements : CompilerPass
{
    CompilationContextOLD context;

    //public override void CheckMethod(CompilationContext context, ShaderMethod compiledMethod)
    //{
    //    this.context = context;
    //    compiledMethod.TransformBody(this);

    //    base.CheckMethod(context, compiledMethod);
    //}

    //protected override Expression VisitMember(MemberExpression node)
    //{
    //    if (node.Expression!.Type != context.ShaderType)
    //    {
    //        return base.VisitMember(node);
    //    }

    //    return new CompiledVariableExpression(context.uniforms.Single(v => v.BackingField == node.Member));
    //}

    //protected override Expression VisitBinary(BinaryExpression node)
    //{
    //    if (node.NodeType is not ExpressionType.Assign
    //        || node.LeftOperand is not MemberExpression memberExpression
    //        || memberExpression!.Member.DeclaringType != context.ShaderType)
    //    {
    //        return base.VisitBinary(node);
    //    }

    //    var compiledVarExpr = new CompiledVariableExpression(context.uniforms.Single(v => v.BackingField == memberExpression.Member));
    //    return new CompiledVariableAssignmentExpression(compiledVarExpr, Visit(node.RightOperand));
    //}
}
