using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Expressions;
public record MemberAccess(ShaderExpression? Instance, MemberInfo Member, ShaderType MemberType) : ShaderExpression
{
    public override ShaderType? ExpressionType => MemberType;

    public override ShaderExpression Accept(ShaderExpressionVisitor visitor)
    {
        return visitor.VisitMemberAccess(this);
    }

    public override ShaderExpression VisitChildren(ShaderExpressionVisitor visitor)
    {
        return new MemberAccess(Instance.Accept(visitor), Member, MemberType);
    }

    public override string ToString()
    {
        return (Instance?.ToString() ?? Member.DeclaringType!.Name) + "." + Member.Name;
    }
}
