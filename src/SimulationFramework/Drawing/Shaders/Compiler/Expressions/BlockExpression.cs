
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Expressions;
public record BlockExpression : ShaderExpression
{
    public IReadOnlyList<ShaderExpression> Expressions { get; }

    public BlockExpression(IReadOnlyList<ShaderExpression> Expressions)
    {
        this.Expressions = Expressions.Where(e => e is not null && e != Empty).ToList();
    }

    public override ShaderType? ExpressionType => null;

    public override ShaderExpression Accept(ShaderExpressionVisitor visitor)
    {
        return visitor.VisitBlockExpression(this);
    }

    public override ShaderExpression VisitChildren(ShaderExpressionVisitor visitor)
    {
        return new BlockExpression(Expressions.Select(e => e.Accept(visitor)).ToArray());
    }

    public override string ToString()
    {
        return @$"{{
    {Expressions.Aggregate("", (s, e) => $"{s}\n    {e}")}
}}";
    }
}
