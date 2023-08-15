using SimulationFramework.Drawing.Shaders.Compiler;
using SimulationFramework.Drawing.Shaders.Compiler.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.SkiaSharp.Shaders;
internal class SKSLCodeGenerator : CodeGenerator
{
    public SKSLCodeGenerator(ShaderCompilation compilation) : base(compilation)
    {
    }


    protected override Expression VisitIntrinsicCallExpression(IntrinsicCallExpression node)
    {
        if (node.Method.Name == "ColorF")
        {
            Writer.Write("float4");
        }
        else
        {
            VisitTypeName(node.Method.DeclaringType);
            Writer.Write('_');
            VisitIdentifier(node.Method.Name);
        }

        Writer.Write("(");

        Visit(node.Arguments.First());
        foreach (var arg in node.Arguments.Skip(1))
        {
            Writer.Write(", ");
            Visit(arg);
        }

        Writer.Write(")");

        return node;
    }

    protected override Expression VisitCompiledMethodCallExpression(CompiledMethodCallExpression node)
    {
        return base.VisitCompiledMethodCallExpression(node);
    }

    protected override void VisitVariable(ShaderVariable variable)
    {
        Writer.Write("uniform ");
        VisitType(variable.VariableType);
        Writer.Write(" ");
        VisitIdentifier(variable.Name);
        Writer.WriteLine(";");

        // base.VisitVariable(variable);
    }

    protected override void VisitType(Type type)
    {
        if (type == typeof(float))
        {
            Writer.Write("float");
            return;
        }

        if (type == typeof(ColorF))
        {
            Writer.Write("float4");
            return;
        }

        if (type == typeof(Vector2))
        {
            Writer.Write("float2");
            return;
        }

        base.VisitType(type);
    }
}
