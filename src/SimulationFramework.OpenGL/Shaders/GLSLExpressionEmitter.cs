using SimulationFramework.Drawing.Shaders;
using SimulationFramework.Drawing.Shaders.Compiler;
using SimulationFramework.Drawing.Shaders.Compiler.Expressions;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

namespace SimulationFramework.OpenGL.Shaders;

class GLSLExpressionEmitter(IndentedTextWriter writer, GLSLShaderEmitter emitter) : ShaderExpressionVisitor
{
    public override ShaderExpression VisitBlockExpression(BlockExpression expression)
    {
        writer.WriteLine('{');
        writer.Indent++;
        foreach (var expr in expression.Expressions)
        {
            if (expr == ShaderExpression.Empty)
                continue;

            expr.Accept(this);
            writer.WriteLine(';');
        }

        writer.Indent--;
        writer.Write('}');
        return expression;
    }

    public override ShaderExpression VisitBinaryExpression(BinaryExpression expression)
    {
        if (expression.LeftOperand is BinaryExpression)
        {
            writer.Write('(');
            expression.LeftOperand.Accept(this);
            writer.Write(')');
        }
        else
        {
            expression.LeftOperand.Accept(this);
        }

        writer.Write(' ');
        writer.Write(expression.GetOperator());
        writer.Write(' ');

        if (expression.RightOperand is BinaryExpression)
        {
            writer.Write('(');
            expression.RightOperand.Accept(this);
            writer.Write(')');
        }
        else
        {
            expression.RightOperand.Accept(this);
        }
        return expression;
    }

    public override ShaderExpression VisitBreakExpression(BreakExpression expression)
    {
        writer.Write("break");
        return expression;
    }

    public override ShaderExpression VisitContinueExpression(ContinueExpression expression)
    {
        writer.Write("continue");
        return expression;
    }

    public override ShaderExpression VisitReturnExpression(ReturnExpression expression)
    {
        writer.Write("return");
        if (expression.ReturnValue is not null)
        {
            writer.Write(' ');
            expression.ReturnValue.Accept(this);
        }
        return expression;
    }

    public override ShaderExpression VisitConstantExpression(ConstantExpression expression)
    {
        writer.Write(expression.Value switch
        {
            bool b => b ? "true" : "false",
            int i => i.ToString(),
            float f => f switch
            {
                float.NaN => "_NaN",
                float.PositiveInfinity => "_PositiveInfinity",
                float.NegativeInfinity => "_NegativeInfinity",
                _ => f.ToString("f7"),
            },
            Vector2 v2 => $"vec2({v2.X:f7}, {v2.Y:f7})",
            Vector3 v3 => $"vec3({v3.X:f7}, {v3.Y:f7}, {v3.Z:f7})",
            Vector4 v4 => $"vec4({v4.X:f7}, {v4.Y:f7}, {v4.Z:f7}, {v4.W:f7})",
            ColorF colF => $"vec4({colF.R:f7}, {colF.G:f7}, {colF.B:f7}, {colF.A:f7})",
            _ => throw new($"unsupported constant {expression.Value.GetType().Name}")
        });

        return expression;
    }

    public override ShaderExpression VisitShaderVariableExpression(ShaderVariableExpression expression)
    {
        emitter.EmitName(expression.Variable.Name);
        return expression;
    }

    public override ShaderExpression VisitShaderMethodCall(ShaderMethodCall expression)
    {
        emitter.EmitName(expression.Callee.Name);
        WriteArgList(expression.Arguments);
        return expression;
    }

    private void WriteArgList(IReadOnlyList<ShaderExpression> args)
    {
        writer.Write('(');
        if (args.Count > 0)
        {
            args[0].Accept(this);
            foreach (var arg in args.Skip(1))
            {
                writer.Write(", ");
                arg.Accept(this);
            }
        }
        writer.Write(')');
    }

    private HashSet<Type> intrinsics = [
        typeof(Vector2),
        typeof(Vector3),
        typeof(Vector4),
        typeof(ColorF),
    ];

    public override ShaderExpression VisitMemberAccess(MemberAccess expression)
    {
        ShaderExpression instance = expression;
        while (instance is MemberAccess ma)
        {
            instance = ma.Instance;
        }

        if (instance is ShaderVariableExpression varExpr)
        {
            if (varExpr.Variable == emitter.vertexDataVariable)
            {
                WriteVertexDataAccess(expression);
                return expression;
            }
        }

        string name = expression.Member.Name;
        if (name.StartsWith('<'))
        {
            name = name[1..].Split('>')[0];
        }
        if (intrinsics.Contains(expression.Member.DeclaringType))
        {
            name = name.ToLower();
        }

        expression.Instance!.Accept(this);
        writer.Write('.');
        writer.Write(name);

        return expression;
    }

    private void WriteVertexDataAccess(ShaderExpression expr)
    {
        if (expr is MemberAccess ma)
        {
            WriteVertexDataAccess(ma.Instance);
            // if the member access is on a primitive type, then it's something like 'vec3.x' and needs to use a '.' not a '_'.
            if (ma.Instance.ExpressionType.GetPrimitiveKind() == null)
            {
                writer.Write('_');
                writer.Write(ma.Member.Name);
            }
            else
            {
                writer.Write('.');
                writer.Write(ma.Member.Name.ToLower());
            }
        }
        else if (expr is ShaderVariableExpression)
        {
            expr.Accept(this);
        }
        else
        {
            throw new UnreachableException();
        }

    }

    public override ShaderExpression VisitShaderIntrinsicCall(ShaderIntrinsicCall expression)
    {
        var op = GetOperatorFromName(expression.Intrinsic.Name);
        if (op != null)
        {
            var left = expression.Arguments[0];
            var right = expression.Arguments[1];
            writer.Write('(');
            left.Accept(this);
            writer.Write(op);
            right.Accept(this);
            writer.Write(')');
            return expression;
        }

        if (expression.Intrinsic.Name == nameof(ShaderIntrinsics.GetElement))
        {
            expression.Arguments[0].Accept(this);
            writer.Write('[');
            expression.Arguments[1].Accept(this);
            writer.Write(']');
            return expression;
        }

        if (expression.Intrinsic.Name == nameof(ShaderIntrinsics.SetElement))
        {
            writer.Write('(');
            expression.Arguments[0].Accept(this);
            writer.Write('[');
            expression.Arguments[1].Accept(this);
            writer.Write("] = ");
            expression.Arguments[2].Accept(this);
            writer.Write(')');
            return expression;
        }

        if (expression.Intrinsic.Name == nameof(ShaderIntrinsics.TextureSample))
        {
            writer.Write("texture(");
            expression.Arguments[0].Accept(this);
            writer.Write(", (");
            expression.Arguments[1].Accept(this);
            writer.Write(") / textureSize(");
            expression.Arguments[0].Accept(this);
            writer.Write(", 0).xy)");
            return expression;
        }

        if (expression.Intrinsic.Name == nameof(ShaderIntrinsics.TextureSampleUV))
        {
            writer.Write("texture(");
            expression.Arguments[0].Accept(this);
            writer.Write(", ");
            expression.Arguments[1].Accept(this);
            writer.Write(")");
            return expression;
        }

        if (expression.Intrinsic.Name == nameof(ShaderIntrinsics.TextureWidth))
        {
            writer.Write("textureSize(");
            expression.Arguments.Single().Accept(this);
            writer.Write(").x");
            return expression;
        }

        if (expression.Intrinsic.Name == nameof(ShaderIntrinsics.TextureHeight))
        {
            writer.Write("textureSize(");
            expression.Arguments.Single().Accept(this);
            writer.Write(").y");
            return expression;
        }

        var graphics = Application.GetComponent<GLGraphics>();

        if (expression.Intrinsic.Name == nameof(ShaderIntrinsics.Equals))
        {
            expression.Arguments[0].Accept(this);
            writer.Write(" == ");
            expression.Arguments[1].Accept(this);
            return expression;
        }

        if (expression.Intrinsic.Name == nameof(ShaderIntrinsics.BufferLength))
        {
            if (!graphics.HasGLES31)
            {
                var tex = expression.Arguments[0] as ShaderVariableExpression;
                var elementType = (tex.Variable.Type as ShaderArrayType)!.ElementType;
                writer.Write($"_{tex.Variable.Name}_length");
                return expression;
            }

            expression.Arguments.Single().Accept(this);
            writer.Write(".length()");
            return expression;
        }

        if (expression.Intrinsic.Name == nameof(ShaderIntrinsics.BufferLoad))
        {
            if (!graphics.HasGLES31)
            {
                // array texture read:
                var tex = expression.Arguments[0] as ShaderVariableExpression;
                var elementType = (tex.Variable.Type as ShaderArrayType)!.ElementType;
                var index = expression.Arguments[1];

                writer.Write("_bufferload_");
                writer.Write(tex.Variable.Name.value);
                writer.Write('(');
                index.Accept(this);
                writer.Write(')');

                return expression;
            }

            expression.Arguments[0].Accept(this);
            writer.Write('[');
            expression.Arguments[1].Accept(this);
            writer.Write(']');
            return expression;
        }

        if (expression.Intrinsic.Name == nameof(ShaderIntrinsics.AsBool))
        {
            writer.Write("bool(");
            expression.Arguments[0].Accept(this);
            writer.Write(")");
            return expression;
        }

        if (expression.Intrinsic.Name == nameof(ShaderIntrinsics.AsInt))
        {
            writer.Write("int(");
            expression.Arguments[0].Accept(this);
            writer.Write(")");
            return expression;
        }

        if (expression.Intrinsic.Name == nameof(ShaderIntrinsics.Transform))
        {
            writer.Write('(');
            expression.Arguments[1].Accept(this);
            writer.Write(" * ");
            expression.Arguments[0].Accept(this);
            writer.Write(')');
            return expression;
        }

        if (expression.Intrinsic.Name == nameof(ShaderIntrinsics.Discard))
        {
            writer.Write("discard");
            return expression;
        }

        writer.Write(expression.Intrinsic.Name switch
        {
            nameof(ShaderIntrinsics.DDX) => "dFdx",
            nameof(ShaderIntrinsics.DDY) => "dFdy",
            nameof(ShaderIntrinsics.Vec2) => "vec2",
            nameof(ShaderIntrinsics.Vec3) => "vec3",
            nameof(ShaderIntrinsics.Vec4) => "vec4",
            // nameof(ShaderIntrinsics.MakeColorF) => "vec4",
            nameof(ShaderIntrinsics.BufferLength) => "length",
            nameof(ShaderIntrinsics.Ceiling) => "ceil",
            nameof(ShaderIntrinsics.Atan2) => "atan",
            nameof(ShaderIntrinsics.Truncate) => "trunc",
            nameof(ShaderIntrinsics.Lerp) => "mix",
            _ => expression.Intrinsic.Name.ToLower()
        });
        WriteArgList(expression.Arguments);
        return expression;
    }

    private string? GetOperatorFromName(string operatorName)
    {
        return operatorName switch
        {
            nameof(ShaderIntrinsics.Multiply) => "*",
            nameof(ShaderIntrinsics.Add) => "+",
            nameof(ShaderIntrinsics.Subtract) => "-",
            nameof(ShaderIntrinsics.Divide) => "/",
            _ => null,
        };
    }

    public override ShaderExpression VisitConditionalExpression(ConditionalExpression expression)
    {
        if (expression.IsTernary)
        {
            writer.Write("((");
            expression.Condition.Accept(this);
            writer.Write(")");
            writer.Write(" ? ");
            expression.Success!.Accept(this);
            writer.Write(" : ");
            expression.Failure!.Accept(this);
            writer.Write(")");
            return expression;
        }

        writer.Write("if (");
        expression.Condition.Accept(this);
        writer.Write(") ");

        if (expression.Success != null)
        {
            if (expression.Success is BlockExpression)
            {
                expression.Success.Accept(this);
            }
            else
            {
                writer.WriteLine("{");
                writer.Indent++;
                expression.Success.Accept(this);
                writer.WriteLine(";");
                writer.Indent--;
                writer.Write("}");
            }
        }
        else
        {
            writer.WriteLine("{");
            writer.Write("}");
        }

        if (expression.Failure != null)
        {
            writer.Write(" else ");
            if (expression.Failure is BlockExpression)
            {
                expression.Failure.Accept(this);
            }
            else
            {
                writer.WriteLine("{");
                writer.Indent++;
                expression.Failure.Accept(this);
                writer.WriteLine(";");
                writer.Indent--;
                writer.WriteLine("}");
            }
        }
        return expression;
    }

    public override ShaderExpression VisitLoopExpression(LoopExpression expression)
    {
        if (expression.Body is BlockExpression block)
        {
            if (block.Expressions[0] is ConditionalExpression cond && cond.Success is BreakExpression && cond.Failure is null)
            {
                writer.Write("while (");

                if (cond.Condition is UnaryExpression unary && unary.Operation is UnaryOperation.Not)
                {
                    unary.Operand.Accept(this);
                }
                else
                {
                    var invertedCond = new UnaryExpression(UnaryOperation.Not, cond.Condition, null);
                    invertedCond.Accept(this);
                }

                writer.Write(") ");

                var remainingBody = new BlockExpression(block.Expressions.Skip(1).ToList());
                remainingBody.Accept(this);

                return expression;
            }
        }

        writer.Write("while (true) ");
        expression.Body.Accept(this);

        return expression;
    }

    public override ShaderExpression VisitUnaryExpression(UnaryExpression expression)
    {
        switch (expression.Operation)
        {
            case UnaryOperation.Cast:
                emitter.EmitType(expression.CastType!);
                break;
            default:
                writer.Write(expression.GetOperator());
                break;
        }

        writer.Write('(');
        expression.Operand.Accept(this);
        writer.Write(')');
        return expression;
    }

    public override ShaderExpression VisitDefaultExpression(DefaultExpression expression)
    {
        if (expression.Type == ShaderType.Bool)
        {
            writer.Write("false");
        }
        else if (expression.Type == ShaderType.Int)
        {
            writer.Write("0");
        }
        else if (expression.Type == ShaderType.Float)
        {
            writer.Write("0.0");
        }
        else if (expression.Type == ShaderType.Float2)
        {
            writer.Write("vec2(0, 0)");
        }
        else if (expression.Type == ShaderType.Float3)
        {
            writer.Write("vec3(0, 0, 0)");
        }
        else if (expression.Type == ShaderType.Float4)
        {
            writer.Write("vec4(0, 0, 0, 0)");
        }
        else if (expression.Type is ShaderStructureType type)
        {
            var structure = type.structure;
            writer.Write(structure.name);
            writer.Write('(');
            for (int i = 0; i < structure.fields.Length; i++)
            {
                var field = structure.fields[i];
                if (i != 0)
                {
                    writer.Write(", ");
                }
                VisitDefaultExpression(new DefaultExpression(field.Type));
            }
            writer.Write(')');
        }
        else
        {
            throw new NotSupportedException();
        }

        return base.VisitDefaultExpression(expression);
    }
}