using SimulationFramework.Shaders.Compiler.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders.Compiler.Rules;
internal class ShaderTypeParameterRules : CompilationRule
{
    CompilationContext context;

    public override void CheckMethod(CompilationContext context, CompiledMethod compiledMethod)
    {
        this.context = context;

        var shaderTypeParameters = compiledMethod.Parameters.Where(p => p.Type == context.ShaderType);
        if (shaderTypeParameters.Any())
        {
            foreach (var param in shaderTypeParameters.ToArray())
            {
                compiledMethod.Parameters.Remove(param);
            }
        }

        compiledMethod.TransformBody(this);

        base.CheckMethod(context, compiledMethod);
    }

    protected override Expression VisitExtension(Expression node)
    {
        if (node is CompiledMethodCallExpression compiledMethodCallExpression)
        {
            foreach (var arg in compiledMethodCallExpression.Arguments)
            {
                if (arg.Type == context.ShaderType)
                {
                    compiledMethodCallExpression.Arguments.Remove(arg);
                }
            }

            return compiledMethodCallExpression;
        }

        return base.VisitExtension(node);
    }
}
