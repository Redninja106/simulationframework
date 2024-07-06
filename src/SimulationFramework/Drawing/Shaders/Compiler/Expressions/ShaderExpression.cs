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
    public abstract ShaderType? ExpressionType { get; }

    public virtual ShaderExpression VisitChildren(ExpressionVisitor visitor) => this;
    public abstract ShaderExpression Accept(ExpressionVisitor visitor);
}
