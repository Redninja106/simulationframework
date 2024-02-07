using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Expressions;
public record MemberAccessExpression(Expression? Instance, MemberInfo Member) : Expression
{
    public override Type? ExpressionType => (Member as FieldInfo)?.FieldType ?? (Member as PropertyInfo)?.PropertyType ?? null;

    public override Expression Accept(ExpressionVisitor visitor)
    {
        return visitor.VisitMemberAccessExpression(this);
    }

    public override Expression VisitChildren(ExpressionVisitor visitor)
    {
        return new MemberAccessExpression(Instance.Accept(visitor), Member);
    }

    public override string ToString()
    {
        return (Instance?.ToString() ?? Member.DeclaringType!.Name) + "." + Member.Name;
    }
}
