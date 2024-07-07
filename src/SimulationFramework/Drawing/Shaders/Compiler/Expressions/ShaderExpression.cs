using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Expressions;

/// <summary>
/// Base type for expressions/statements in a method body during compilation.
/// </summary>
public abstract record ShaderExpression
{
    public static ShaderExpression Empty { get; } = new EmptyExpression();
    
    public abstract ShaderType? ExpressionType { get; }

    public virtual ShaderExpression VisitChildren(ShaderExpressionVisitor visitor) => this;
    public abstract ShaderExpression Accept(ShaderExpressionVisitor visitor);

    private record EmptyExpression : ShaderExpression
    {
        public override ShaderType? ExpressionType => null;

        public override ShaderExpression Accept(ShaderExpressionVisitor visitor)
        {
            return this;
        }
    }
}
