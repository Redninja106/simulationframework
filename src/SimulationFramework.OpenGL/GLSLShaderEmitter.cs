using SimulationFramework.Drawing.Shaders;
using SimulationFramework.Drawing.Shaders.Compiler;
using SimulationFramework.Drawing.Shaders.Compiler.Expressions;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.OpenGL;
internal class GLSLShaderEmitter
{
    IndentedTextWriter writer;
    GLSLExpressionEmitter expressionEmitter;
    private int nextBufferSlot = 0;
    internal ShaderVariable? vertexDataVariable;
    internal ShaderCompilation compilation;
    internal int vsOutLocation = 0;

    private static readonly ImmutableHashSet<string> reservedWords = [
        // KEYWORDS:
        "attribute",
        "const",
        "uniform",
        "varying",
        "layout",
        "centroid",
        "flat",
        "smooth",
        "noperspective",
        "patch",
        "sample",
        "break",
        "continue",
        "do",
        "for",
        "while",
        "switch",
        "case",
        "default",
        "if",
        "else",
        "subroutine",
        "in",
        "out",
        "inout",
        "float",
        "double",
        "int",
        "void",
        "bool",
        "true",
        "false",
        "invariant",
        "discard",
        "return",
        "mat2",
        "mat3",
        "mat4",
        "dmat2",
        "dmat3",
        "dmat4",
        "mat2x2",
        "mat2x3",
        "mat2x4",
        "dmat2x2",
        "dmat2x3",
        "dmat2x4",
        "mat3x2",
        "mat3x3",
        "mat3x4",
        "dmat3x2",
        "dmat3x3",
        "dmat3x4",
        "mat4x2",
        "mat4x3",
        "mat4x4",
        "dmat4x2",
        "dmat4x3",
        "dmat4x4",
        "vec2",
        "vec3",
        "vec4",
        "ivec2",
        "ivec3",
        "ivec4",
        "bvec2",
        "bvec3",
        "bvec4",
        "dvec2",
        "dvec3",
        "dvec4",
        "uint",
        "uvec2",
        "uvec3",
        "uvec4",
        "lowp",
        "mediump",
        "highp",
        "precision",
        "sampler1D",
        "sampler2D",
        "sampler3D",
        "samplerCube",
        "sampler1DShadow",
        "sampler2DShadow",
        "samplerCubeShadow",
        "sampler1DArray",
        "sampler2DArray",
        "sampler1DArrayShadow",
        "sampler2DArrayShadow",
        "isampler1D",
        "isampler2D",
        "isampler3D",
        "isamplerCube",
        "isampler1DArray",
        "isampler2DArray",
        "usampler1D",
        "usampler2D",
        "usampler3D",
        "usamplerCube",
        "usampler1DArray",
        "usampler2DArray",
        "sampler2DRect",
        "sampler2DRectShadow",
        "isampler2DRect",
        "usampler2DRect",
        "samplerBuffer",
        "isamplerBuffer",
        "usamplerBuffer",
        "sampler2DMS",
        "isampler2DMS",
        "usampler2DMS",
        "sampler2DMSArray",
        "isampler2DMSArray",
        "usampler2DMSArray",
        "samplerCubeArray",
        "samplerCubeArrayShadow",
        "isamplerCubeArray",
        "usamplerCubeArray",
        "struct",
        "common",
        "partition",
        "active",
        "asm",
        "class",
        "union",
        "enum",
        "typedef",
        "template",
        "this",
        "packed",
        "goto",
        "inline",
        "noinline",
        "volatile",
        "public",
        "static",
        "extern",
        "external",
        "interface",
        "long",
        "short",
        "half",
        "fixed",
        "unsigned",
        "superp",
        "input",
        "output",
        "hvec2",
        "hvec3",
        "hvec4",
        "fvec2",
        "fvec3",
        "fvec4",
        "sampler3DRect",
        "filter",
        "image1D",
        "image2D",
        "image3D",
        "imageCube",
        "iimage1D",
        "iimage2D",
        "iimage3D",
        "iimageCube",
        "uimage1D",
        "uimage2D",
        "uimage3D",
        "uimageCube",
        "image1DArray",
        "image2DArray",
        "iimage1DArray",
        "iimage2DArray",
        "uimage1DArray",
        "uimage2DArray",
        "image1DShadow",
        "image2DShadow",
        "image1DArrayShadow",
        "image2DArrayShadow",
        "imageBuffer",
        "iimageBuffer",
        "uimageBuffer",
        "sizeof",
        "cast",
        "namespace",
        "using",
        "row_major",
        // INTRINSICS:
        "abs",
        "acos",
        "acosh",
        "all",
        "any",
        "asin",
        "asinh",
        "atan",
        "atanh",
        "atomicAdd",
        "atomicAnd",
        "atomicCompSwap",
        "atomicCounter",
        "atomicCounterDecrement",
        "atomicCounterIncrement",
        "atomicExchange",
        "atomicMax",
        "atomicMin",
        "atomicOr",
        "atomicXor",
        "barrier",
        "bitCount",
        "bitfieldExtract",
        "bitfieldInsert",
        "bitfieldReverse",
        "ceil",
        "clamp",
        "cos",
        "cosh",
        "cross",
        "degrees",
        "determinant",
        "dFdx",
        "dFdxCoarse",
        "dFdxFine",
        "dFdy",
        "dFdyCoarse",
        "dFdyFine",
        "distance",
        "dot",
        "EmitStreamVertex",
        "EmitVertex",
        "EndPrimitive",
        "EndStreamPrimitive",
        "equal",
        "exp",
        "exp2",
        "faceforward",
        "findLSB",
        "findMSB",
        "floatBitsToInt",
        "floatBitsToUint",
        "floor",
        "fma",
        "fract",
        "frexp",
        "fwidth",
        "fwidthCoarse",
        "fwidthFine",
        "greaterThan",
        "greaterThanEqual",
        "groupMemoryBarrier",
        "imageAtomicAdd",
        "imageAtomicAnd",
        "imageAtomicCompSwap",
        "imageAtomicExchange",
        "imageAtomicMax",
        "imageAtomicMin",
        "imageAtomicOr",
        "imageAtomicXor",
        "imageLoad",
        "imageSamples",
        "imageSize",
        "imageStore",
        "imulExtended",
        "intBitsToFloat",
        "interpolateAtCentroid",
        "interpolateAtOffset",
        "interpolateAtSample",
        "inverse",
        "inversesqrt",
        "isinf",
        "isnan",
        "ldexp",
        "length",
        "lessThan",
        "lessThanEqual",
        "log",
        "log2",
        "matrixCompMult",
        "max",
        "memoryBarrier",
        "memoryBarrierAtomicCounter",
        "memoryBarrierBuffer",
        "memoryBarrierImage",
        "memoryBarrierShared",
        "min",
        "mix",
        "mod",
        "modf",
        "noise",
        "noise1",
        "noise2",
        "noise3",
        "noise4",
        "normalize",
        "not",
        "notEqual",
        "outerProduct",
        "packDouble2x32",
        "packHalf2x16",
        "packSnorm2x16",
        "packSnorm4x8",
        "packUnorm",
        "packUnorm2x16",
        "packUnorm4x8",
        "pow",
        "radians",
        "reflect",
        "refract",
        "removedTypes",
        "round",
        "roundEven",
        "sign",
        "sin",
        "sinh",
        "smoothstep",
        "sqrt",
        "step",
        "tan",
        "tanh",
        "texelFetch",
        "texelFetchOffset",
        "texture",
        "textureGather",
        "textureGatherOffset",
        "textureGatherOffsets",
        "textureGrad",
        "textureGradOffset",
        "textureLod",
        "textureLodOffset",
        "textureOffset",
        "textureProj",
        "textureProjGrad",
        "textureProjGradOffset",
        "textureProjLod",
        "textureProjLodOffset",
        "textureProjOffset",
        "textureQueryLevels",
        "textureQueryLod",
        "textureSamples",
        "textureSize",
        "transpose",
        "trunc",
        "uaddCarry",
        "uintBitsToFloat",
        "umulExtended",
        "unpackDouble2x32",
        "unpackHalf2x16",
        "unpackSnorm2x16",
        "unpackSnorm4x8",
        "unpackUnorm",
        "unpackUnorm2x16",
        "unpackUnorm4x8",
        "usubBorrow",
    ];

    public GLSLShaderEmitter(TextWriter writer)
    {
        this.writer = new(writer);
        writer.WriteLine("const float _PositiveInfinity = uintBitsToFloat(0x7F800000);");
        writer.WriteLine("const float _NegativeInfinity = uintBitsToFloat(0xFF800000);");
        writer.WriteLine("const float _NaN = 0 * _PositiveInfinity;");
        this.expressionEmitter = new(this.writer, this);
    }

    public void Emit(ShaderCompilation compilation)
    {
        this.compilation = compilation;
        vertexDataVariable = compilation.Variables.FirstOrDefault(v => v.Kind == ShaderVariableKind.VertexData);
        
        foreach (var structure in compilation.Structures)
        {
            EmitStructureDefinition(structure);
        }

        foreach (var variable in compilation.Variables)
        {
            EmitVariable(variable);
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
            if (expr == ShaderExpression.Empty)
                continue;

            expr.Accept(expressionEmitter);
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

        if (type.GetPrimitiveKind() is PrimitiveKind primitive)
        {
            writer.Write(primitive switch
            {
                PrimitiveKind.Void => "void",
                PrimitiveKind.Bool => "bool",
                PrimitiveKind.Float => "float",
                PrimitiveKind.Float2 => "vec2",
                PrimitiveKind.Float3 => "vec3",
                PrimitiveKind.Float4 => "vec4",
                PrimitiveKind.Int => "int",
                PrimitiveKind.Int2 => "vec2i",
                PrimitiveKind.Int3 => "vec3i",
                PrimitiveKind.Int4 => "vec4i",
                PrimitiveKind.Matrix3x2 => "mat3x2",
                PrimitiveKind.Matrix4x4 => "mat4",
                PrimitiveKind.Texture => "sampler2D",
                _ => throw new NotSupportedException(primitive.ToString())
            });
            return;
        }

        if (type is ShaderStructureType structureType)
        {
            EmitName(structureType.structure.name);
            return;
        }

        if (type is ShaderArrayType arrayType)
        {
            //EmitType(arrayType.ElementType);
            writer.Write("buffer");
            return;
        }

        throw new(type.ToString());
    }

    public void EmitName(ShaderName name)
    {
        if (IsReservedWord(name.value))
        {
            writer.Write('_');
        }

        writer.Write(name.value);
    }

    private bool IsReservedWord(string word)
    {
        if (word.StartsWith("gl_"))
            return true;

        if (reservedWords.Contains(word))
            return true;

        return false;
    }

    private void EmitVariable(ShaderVariable variable)
    {
        if (variable.Type is ShaderArrayType bufferType)
        {
            Debug.Assert(variable.Kind == ShaderVariableKind.Uniform);

            writer.Write("layout(std430, binding=");
            writer.Write(nextBufferSlot);
            writer.Write(") buffer _buf_");
            writer.Write(nextBufferSlot);
            writer.WriteLine(" {");
            writer.Indent++;

            EmitType(bufferType.ElementType);
            writer.Write(' ');
            EmitName(variable.Name);
            writer.WriteLine("[];");

            writer.Indent--;
            writer.WriteLine("};");

            return;
        }

        if (variable.Kind == ShaderVariableKind.Uniform)
        {
            writer.Write("uniform ");
        }
        else if (variable.Kind == ShaderVariableKind.VertexData)
        {
            int location = 0;
            EmitVertexDeclaration(variable.Type, variable.Name.value, ref location);
            return;
        }
        else if (variable.Kind == ShaderVariableKind.VertexShaderOutput)
        {
            writer.Write("layout(location = ");
            writer.Write(vsOutLocation++);
            if (compilation.Kind == ShaderKind.Canvas)
            {
                writer.Write(") in ");
            }
            else if (compilation.Kind == ShaderKind.Vertex)
            {
                writer.Write(") out ");
            }
            else
            {
                throw new NotSupportedException();
            }
        }
        else
        {
            throw new NotSupportedException(variable.Kind.ToString());
        }

        EmitType(variable.Type);
        writer.Write(' ');
        EmitName(variable.Name);
        writer.WriteLine(';');
    }

    private void EmitVertexDeclaration(ShaderType type, string baseName, ref int location)
    {
        if (type is ShaderStructureType structType)
        {
            foreach (var field in structType.structure.fields)
            {
                EmitVertexDeclaration(field.Type, baseName + "_" + field.Name, ref location);
            }
        }
        else if (type.GetPrimitiveKind() != null)
        {
            writer.Write("layout(location = ");
            writer.Write(location);
            writer.Write(") in ");
            EmitType(type);
            writer.Write(' ');
            writer.Write(baseName);
            writer.WriteLine(';');
            location++;
        }
        else
        {
            throw new NotSupportedException($"type {type} is not supported as vertex data!");
        }
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
            BinaryOperation.XOr => "^",
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
            bool b => b ? "true" : "false",
            int i => i.ToString(),
            float f => f switch 
            { 
                float.NaN => "_NaN",
                float.PositiveInfinity => "_PositiveInfinity",
                float.NegativeInfinity => "_NegativeInfinity",
                _ => f.ToString("G9"),
            },
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

        if (expression.Intrinsic.Name == nameof(ShaderIntrinsics.BufferLength))
        {
            expression.Arguments.Single().Accept(this);
            writer.Write(".length()");
            return expression;
        }

        if (expression.Intrinsic.Name == nameof(ShaderIntrinsics.BufferLoad))
        {
            expression.Arguments[0].Accept(this);
            writer.Write('[');
            expression.Arguments[1].Accept(this);
            writer.Write(']');
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
            nameof(ShaderIntrinsics.MakeVector2) => "vec2",
            nameof(ShaderIntrinsics.MakeVector3) => "vec3",
            nameof(ShaderIntrinsics.MakeVector4) => "vec4",
            nameof(ShaderIntrinsics.MakeColorF) => "vec4",
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
            case UnaryOperation.Not:
                writer.Write('!');
                break;
            case UnaryOperation.Negate:
                writer.Write('-');
                break;
            case UnaryOperation.Cast:
                emitter.EmitType(expression.CastType!);
                break;
            default:
                throw new UnreachableException();
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
        else if(expression.Type == ShaderType.Float)
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