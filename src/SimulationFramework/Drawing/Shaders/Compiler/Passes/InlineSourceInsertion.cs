using SimulationFramework.Drawing.Shaders.Compiler.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Passes;
internal class InlineSourceInsertion : CompilerPass
{
    public override void CheckMethod(CompilationContext context, CompiledMethod compiledMethod)
    {
        compiledMethod.TransformBody(this);

        base.CheckMethod(context, compiledMethod);
    }

    protected override Expression VisitExtension(Expression node)
    {
        if (node is not IntrinsicCallExpression intrinsicCall)
            return base.VisitExtension(node);

        if (intrinsicCall.Method.Name != nameof(ShaderIntrinsics.Hlsl))
            return base.VisitExtension(node);

        var strExpr = intrinsicCall.Arguments.Single();

        if (strExpr is not ConstantExpression constExpr)
            throw new Exception();

        if (constExpr.Value is not string str)
            throw new Exception();

        return new InlineSourceExpression(str);
    }
}
