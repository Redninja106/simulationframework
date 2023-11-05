using SimulationFramework.Drawing.Shaders;
using SimulationFramework.Drawing.Shaders.Compiler;
using SimulationFramework.Drawing.Shaders.Compiler.Expressions;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.SkiaSharp.Shaders;

internal class SKSLCodeGenerator
{
    private readonly ShaderCompilation compilation;

    private Dictionary<Type, string> typeAliases = new()
    {
        [typeof(Vector4)] = "float4",
        [typeof(Vector3)] = "float3",
        [typeof(Vector2)] = "float2",
        [typeof(ColorF)] = "float4",
        [typeof(float)] = "float",
        [typeof(int)] = "int",
        [typeof(bool)] = "bool",
        [typeof(uint)] = "uint",
        [typeof(void)] = "void",
        [typeof(Matrix3x2)] = "float3x2",
        [typeof(Matrix4x4)] = "float4x4",
    };

    public Dictionary<string, string> intrinsicAliases = new()
    {
        [nameof(ShaderIntrinsics.Multiply)] = "mul",
        [nameof(ShaderIntrinsics.ColorF)] = "float4",
        [nameof(ShaderIntrinsics.Vec2)] = "float2",
        [nameof(ShaderIntrinsics.Vec3)] = "float3",
        [nameof(ShaderIntrinsics.Vec4)] = "float4",
    };

    public Dictionary<FieldInfo, string> fieldAliases = new()
    {
        [typeof(Vector4).GetField("X")] = "x",
        [typeof(Vector4).GetField("Y")] = "y",
        [typeof(Vector4).GetField("Z")] = "z",
        [typeof(Vector4).GetField("W")] = "w",
        [typeof(Vector3).GetField("X")] = "x",
        [typeof(Vector3).GetField("Y")] = "y",
        [typeof(Vector3).GetField("Z")] = "z",
        [typeof(Vector2).GetField("X")] = "x",
        [typeof(Vector2).GetField("Y")] = "y",
    };

    public Dictionary<PropertyInfo, string> propertyAliases = new()
    {
        [typeof(ColorF).GetProperty("R")] = "r",
        [typeof(ColorF).GetProperty("G")] = "g",
        [typeof(ColorF).GetProperty("B")] = "b",
        [typeof(ColorF).GetProperty("A")] = "a",
    };


    public SKSLCodeGenerator(ShaderCompilation compilation)
    {
        this.compilation = compilation;
    }

    public void Emit(TextWriter writer)
    {
        // do some backend-specific transformations
        foreach (var method in compilation.Methods)
        {
            method.VisitBody(new IntrinsicOperatorReplacementVisitor());
        }

        foreach (var uniform in compilation.Uniforms)
        {
            EmitUniform(writer, uniform);
        }

        foreach (var method in compilation.Methods)
        {
            // EmitMethod(writer, method, false);
        }

        foreach (var method in compilation.Methods.Reverse())
        {
            EmitMethod(writer, method, true);
        }

        writer.Write(@$"float4 main(float2 p) {{
    return {compilation.EntryPoint}(p);
}}
");
    }

    private void EmitUniform(TextWriter writer, ShaderUniform uniform)
    {
        writer.Write("uniform ");
        EmitType(writer, uniform.UniformType);
        writer.Write(" ");
        EmitIdentifier(writer, uniform.Name);
        writer.WriteLine(";");
    }

    private void EmitMethod(TextWriter writer, ShaderMethod method, bool emitBody)
    {
        var bodyEmitter = new MethodBodyEmitter(this, writer, method);

        EmitType(writer, method.ReturnType);
        writer.Write(" ");
        EmitIdentifier(writer, method.FullName);
        writer.Write("(");
        foreach (var param in method.Parameters)
        {
            if (param != method.Parameters.First())
                writer.Write(", ");

            switch (param.Modifier)
            {
                case MethodParameterModifier.Out:
                    writer.Write("out ");
                    break;
                case MethodParameterModifier.In:
                    writer.Write("in ");
                    break;
                case MethodParameterModifier.Ref:
                    writer.Write("inout ");
                    break;
            }

            EmitType(writer, param.ParameterType);
            writer.Write(" ");
            EmitIdentifier(writer, param.Name);
        }
        writer.Write(")");

        if (emitBody)
        {
            method.VisitBody(bodyEmitter);
        }
        else
        {
            writer.WriteLine(";");
        }
    }

    internal void EmitType(TextWriter writer, Type type)
    {
        string name = typeAliases.TryGetValue(type, out name) ? name : type.Name;
        writer.Write(name);
    }

    internal void EmitIdentifier(TextWriter writer, string identifier)
    {
        writer.Write(identifier);
    }
}

class MethodBodyEmitter : ExpressionVisitor
{
    public readonly IndentedTextWriter writer;
    private readonly SKSLCodeGenerator generator;
    private readonly ShaderMethod method;

    public MethodBodyEmitter(SKSLCodeGenerator generator, TextWriter writer, ShaderMethod method)
    {
        this.writer = new(writer, "    ");
        this.generator = generator;
        this.method = method;
    }

    public override Expression VisitBinaryExpression(BinaryExpression expression)
    {
        writer.Write("(");
        expression.LeftOperand.Accept(this);
        writer.Write(" ");
        writer.Write(expression.GetOperator());
        writer.Write(" ");
        expression.RightOperand.Accept(this);
        writer.Write(")");
        return expression;
    }

    public override Expression VisitBlockExpression(BlockExpression expression)
    {
        writer.WriteLine();
        writer.WriteLine("{");
        writer.Indent++;

        if (expression == method.Body)
        {
            foreach (var group in method.Locals.GroupBy(l => l.VariableType))
            {
                generator.EmitType(writer, group.Key);
                writer.Write(' ');

                foreach (var local in group)
                {
                    if (local != group.First())
                        writer.Write(", ");

                    generator.EmitIdentifier(writer, local.Name);
                }

                writer.WriteLine(";");
            }
        }

        foreach (var child in expression.Expressions)
        {
            child.Accept(this);
            writer.WriteLine(";");
        }
        writer.Indent--;
        writer.WriteLine();
        writer.WriteLine("}");
        return expression;
    }

    public override Expression VisitBreakExpression(BreakExpression expression)
    {
        writer.Write("break");
        return base.VisitBreakExpression(expression);
    }

    public override Expression VisitCallExpression(CallExpression expression)
    {
        writer.Write(generator.intrinsicAliases.TryGetValue(expression.Callee.Name, out var name) ? name : expression.Callee.Name.ToLower());
        writer.Write("(");

        for (int i = 0; i < expression.Arguments.Count; i++)
        {
            if (i > 0)
                writer.Write(", ");
            expression.Arguments[i].Accept(this);
        }

        writer.Write(")");

        return expression;
    }

    public override Expression VisitShaderCallExpression(ShaderCallExpression expression)
    {
        writer.Write(expression.Callee.FullName);
        writer.Write("(");

        for (int i = 0; i < expression.Arguments.Count; i++)
        {
            if (i > 0)
                writer.Write(", ");
            expression.Arguments[i].Accept(this);
        }

        writer.Write(")");

        return expression;
    }

    public override Expression VisitNewExpression(NewExpression expression)
    {
        // shader ASTs should not have new expressions
        Debug.Assert(false);
        return base.VisitNewExpression(expression);
    }

    public override Expression VisitConstantExpression(ConstantExpression expression)
    {
        writer.Write(EmitConstant(expression.Value));
        return base.VisitConstantExpression(expression);
    }

    private string EmitConstant(object constantValue)
    {
        if (constantValue is ColorF c)
        {
            return $"vec4({c.R}, {c.G}, {c.B}, {c.A})";
        }
        else
        {
            return constantValue.ToString();
        }
    }

    public override Expression VisitLocalVariableExpression(LocalVariableExpression expression)
    {
        writer.Write(expression.ToString());
        return base.VisitLocalVariableExpression(expression);
    }

    public override Expression VisitMethodParameterExpression(MethodParameterExpression expression)
    {
        writer.Write(expression.Parameter.Name);
        return base.VisitMethodParameterExpression(expression);
    }

    public override Expression VisitMemberAccessExpression(MemberAccessExpression expression)
    {
        expression.Instance.Accept(this);

        if (expression.Member.DeclaringType == typeof(Matrix4x4))
        {
            var memName = expression.Member.Name;
            writer.Write($"[{(char)(memName[1]-1)}][{(char)(memName[2]-1)}]");
            return expression;
        }

        writer.Write(".");
        string name = expression.Member.Name;
        if (generator.fieldAliases.TryGetValue(expression.Member as FieldInfo ?? throw new(), out string alias))
        {
            name = alias;
        }
        if (name.StartsWith("<"))
        {
            name = name[(name.IndexOf('<') + 1)..name.IndexOf('>')];
        }
        var prop = expression.Member.DeclaringType.GetProperty(name);
        if (prop is not null && generator.propertyAliases.TryGetValue(prop, out string alias2))
        {
            name = alias2;
        }
        writer.Write(name);
        return expression;
    }

    public override Expression VisitReturnExpression(ReturnExpression expression)
    {
        writer.Write("return" + (expression.ReturnValue is not null ? " " : ""));
        expression.ReturnValue?.Accept(this);
        return expression;
    }

    public override Expression VisitUnaryExpression(UnaryExpression expression)
    {
        writer.Write(expression.GetOperator());
        return base.VisitUnaryExpression(expression);
    }

    public override Expression VisitUniformExpression(UniformExpression expression)
    {
        writer.Write(expression.Uniform.Name);
        return base.VisitUniformExpression(expression);
    }

    public override Expression VisitConditionalExpression(ConditionalExpression expression)
    {
        writer.Write("if (");
        expression.Condition.Accept(this);
        writer.Write(")");
        expression.Success.Accept(this);
        if (expression.Failure is not null)
        {
            writer.Write("else");
            expression.Failure.Accept(this);
        }


        return expression;
    }

    public override Expression VisitDefaultExpression(DefaultExpression expression)
    {
        if (expression.Type == typeof(bool))
        {
            writer.Write("false");
        }
        else if (expression.Type == typeof(float))
        {
            writer.Write("0");
        }
        else if (expression.Type == typeof(Vector2))
        {
            writer.Write("float2(0)");
        }
        else if (expression.Type == typeof(Vector3))
        {
            writer.Write("float3(0)");
        }
        else if (expression.Type == typeof(Vector4))
        {
            writer.Write("float4(0)");
        }
        else
        {
            writer.Write("{}");
        }

        return expression;
    }
}

class IntrinsicOperatorReplacementVisitor : ExpressionVisitor
{
    public override Expression VisitCallExpression(CallExpression expression)
    {
        if (expression.Callee.Name is nameof(ShaderIntrinsics.Multiply) && expression.Callee.DeclaringType == typeof(ShaderIntrinsics))
        {
            return base.VisitBinaryExpression(new(BinaryOperation.Multiply, expression.Arguments[0], expression.Arguments[1]));
        }

        return base.VisitCallExpression(expression);
    }
}