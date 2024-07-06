using SimulationFramework.Drawing.Shaders;
using SimulationFramework.Drawing.Shaders.Compiler;
using SimulationFramework.Drawing.Shaders.Compiler.Expressions;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.OpenGL;
internal class GLSLCanvasShaderEmitter
{
    IndentedTextWriter writer;
    GLSLExpressionEmitter expressionEmitter;

    public GLSLCanvasShaderEmitter(TextWriter writer)
    {
        this.writer = new(writer);
        this.expressionEmitter = new(this.writer);
    }

    public void Emit(ShaderCompilation compilation)
    {
        foreach (var structure in compilation.Structures)
        {
            EmitStructureDefinition(structure);
        }

        foreach (var uniform in compilation.Uniforms)
        {
            EmitUniform(uniform);
        }

        foreach (var method in compilation.Methods)
        {
            EmitMethodHeader(method);
            writer.WriteLine(';');
        }

        foreach (var method in compilation.Methods)
        {
            EmitMethodHeader(method);
            writer.Write(' ');
            EmitMethodBody(method);
        }
    }

    private void EmitMethodBody(ShaderMethod method)
    {
        writer.WriteLine("{");
        writer.Indent++;
        foreach (var localGroup in method.Locals.GroupBy(l => l.Type))
        {
            EmitType(localGroup.Key);
            writer.Write(' ');
            EmitName(localGroup.First().Name);
            foreach (var local in localGroup.Skip(1))
            {
                writer.Write(", ");
                EmitName(local.Name);
            }
            writer.WriteLine(';');
        }
        foreach (var expr in ((BlockExpression)method.Body).Expressions)
        {
            expr.Accept(this.expressionEmitter);
            writer.WriteLine(';');
        }
        writer.Indent--;
        writer.WriteLine("}");
    }

    public void EmitMethodHeader(ShaderMethod method)
    {
        EmitType(method.ReturnType);
        writer.Write(' ');
        EmitName(method.Name);
        writer.Write('(');

        if (method.Parameters.Length > 0)
        {
            EmitType(method.Parameters[0].Type);
            writer.Write(' ');
            EmitName(method.Parameters[0].Name);
            foreach (var parameter in method.Parameters.Skip(1))
            {
                writer.Write(", ");
                EmitType(parameter.Type);
                writer.Write(' ');
                EmitName(parameter.Name);
            }
        }

        writer.Write(')');
    }

    public void EmitType(ShaderType type)
    {
        if (type is ReferenceType refType)
        {
            writer.Write("inout ");
            EmitType(refType.ElementType);
            return;
        }

        if (type is ShaderPrimitiveType primitiveType)
        {
            writer.Write(primitiveType.primitive switch
            {
                ShaderPrimitive.Void => "void",
                ShaderPrimitive.Bool => "bool",
                ShaderPrimitive.Float => "float",
                ShaderPrimitive.Float2 => "vec2",
                ShaderPrimitive.Float3 => "vec3",
                ShaderPrimitive.Float4 => "vec4",
                ShaderPrimitive.Int => "int",
                ShaderPrimitive.Int2 => "vec2i",
                ShaderPrimitive.Int3 => "vec3i",
                ShaderPrimitive.Int4 => "vec4i",
                ShaderPrimitive.Matrix3x2 => "mat3x2",
                ShaderPrimitive.Matrix4x4 => "mat4",
                ShaderPrimitive.Texture => "sampler2D",
                _ => throw new NotSupportedException(primitiveType.primitive.ToString())
            });
            return;
        }

        if (type is ShaderStructureType structureType)
        {
            EmitName(structureType.structure.name);
            return;
        }

        throw new(type.ToString());
    }

    public void EmitName(ShaderName name)
    {
        writer.Write(name);
    }

    private void EmitUniform(ShaderVariable uniform)
    {
        writer.Write("uniform ");
        EmitType(uniform.Type);
        writer.Write(' ');
        EmitName(uniform.Name);
        writer.WriteLine(';');
    }

    private void EmitStructureDefinition(ShaderStructure structure)
    {
        writer.Write("struct ");
        writer.Write(structure.name);
        writer.WriteLine(" {");
        writer.Indent++;

        foreach (var field in structure.fields)
        {
            EmitType(field.Type);
            writer.Write(' ');
            EmitName(field.Name);
            writer.WriteLine(';');
        }

        writer.Indent--;
        writer.WriteLine("};");
    }

}

class GLSLExpressionEmitter(IndentedTextWriter writer) : ExpressionVisitor
{
    public override ShaderExpression VisitBlockExpression(BlockExpression expression)
    {
        writer.WriteLine('{');
        writer.Indent++;
        foreach (var expr in expression.Expressions)
        {
            expr.Accept(this);
            writer.WriteLine(';');
        }

        writer.Indent--;
        writer.WriteLine('}');
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
        writer.Write(expression.Operation switch
        {
            BinaryOperation.Add => "+",
            BinaryOperation.Subtract => "-",
            BinaryOperation.Multiply => "*",
            BinaryOperation.Divide => "/",
            BinaryOperation.Modulus => "%",
            BinaryOperation.NotEqual => "!=",
            BinaryOperation.Equal => "==",
            BinaryOperation.LessThan => "<",
            BinaryOperation.LessThanEqual => "<=",
            BinaryOperation.GreaterThan => ">",
            BinaryOperation.GreaterThanEqual => ">=",
            BinaryOperation.Assignment => "=",
            BinaryOperation.LeftShift => "<<",
            BinaryOperation.RightShift => ">>",
            BinaryOperation.Or => "||",
            BinaryOperation.And => "&&",
            _ => throw new NotImplementedException(expression.Operation.ToString())
        });
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
            int i => i.ToString(),
            float f => f.ToString("G9"),
            Vector2 v2 => $"vec2({v2.X:G9}, {v2.Y:G9})",
            Vector3 v3 => $"vec3({v3.X:G9}, {v3.Y:G9}, {v3.Z:G9})",
            Vector4 v4 => $"vec4({v4.X:G9}, {v4.Y:G9}, {v4.Z:G9}, {v4.W:G9})",
            ColorF colF => $"vec4({colF.R:G9}, {colF.G:G9}, {colF.B:G9}, {colF.A:G9})",
            _ => throw new($"unsupported constant {expression.Value.GetType().Name}")
        });

        return expression;
    }

    public override ShaderExpression VisitShaderVariableExpression(ShaderVariableExpression expression)
    {
        writer.Write(expression.Variable.Name.ToString());
        return expression;
    }

    public override ShaderExpression VisitShaderMethodCall(ShaderMethodCall expression)
    {
        writer.Write(expression.Callee.Name);
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

    public override ShaderExpression VisitShaderIntrinsicCall(ShaderIntrinsicCall expression)
    {
        var op = GetOperatorFromName(expression.Intrinsic.Name);
        if (op != null)
        {
            var left = expression.Arguments[0];
            var right = expression.Arguments[1];
            left.Accept(this);
            writer.Write(op);
            right.Accept(this);
            return expression;
        }

        if (expression.Intrinsic.Name == nameof(ShaderIntrinsics.Sample))
        {
            // writer.Write("texture");
            
        }

        writer.Write(expression.Intrinsic.Name switch
        {
            nameof(ShaderIntrinsics.ColorF) => "vec4",
            nameof(ShaderIntrinsics.Sample) => "texture",
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
            "op_Addition" => "+",
            "op_Subtraction" => "-",
            _ => null,
        };
    }

    public override ShaderExpression VisitConditionalExpression(ConditionalExpression expression)
    {
        writer.Write("if (");
        expression.Condition.Accept(this);
        writer.Write(")");
        
        if (expression.Success != null)
        {
            expression.Success.Accept(this);
        }
        else
        {
            writer.Write("{ }");
        }

        if (expression.Failure != null)
        {
            writer.Write("else ");
            expression.Failure.Accept(this);
        }
        return expression;
    }

    public override ShaderExpression VisitLoopExpression(LoopExpression expression)
    {
        writer.Write("while (true)");
        expression.Body.Accept(this);

        return expression;
    }

    public override ShaderExpression VisitUnaryExpression(UnaryExpression expression)
    {
        writer.Write(expression.Operation switch
        {
            UnaryOperation.Not => "!",
            UnaryOperation.Negate => "-",
        });
        writer.Write('(');
        expression.Operand.Accept(this);
        writer.Write(')');
        return expression;
    }

    public override ShaderExpression VisitDefaultExpression(DefaultExpression expression)
    {
        if (expression.Type == ShaderPrimitiveType.Get(ShaderPrimitive.Bool))
        {
            writer.Write("false");
        }
        else if (expression.Type == ShaderPrimitiveType.Get(ShaderPrimitive.Int))
        {
            writer.Write("0");
        }
        else if(expression.Type == ShaderPrimitiveType.Get(ShaderPrimitive.Float))
        {
            writer.Write("0.0");
        }
        else if (expression.Type == ShaderPrimitiveType.Get(ShaderPrimitive.Float2))
        {
            writer.Write("vec2(0, 0)");
        }
        else if (expression.Type == ShaderPrimitiveType.Get(ShaderPrimitive.Float3))
        {
            writer.Write("vec3(0, 0, 0)");
        }
        else if (expression.Type == ShaderPrimitiveType.Get(ShaderPrimitive.Float4))
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