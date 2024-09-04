using SimulationFramework.Drawing.Shaders.Compiler;
using SimulationFramework.Drawing.Shaders.Compiler.Expressions;
using System;
using System.CodeDom.Compiler;
using System.Collections.Immutable;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.OpenGL.Shaders;
internal class GLSLShaderEmitter
{
    IndentedTextWriter writer;
    GLSLExpressionEmitter expressionEmitter;
    private int nextBufferSlot = 0;
    internal ShaderVariable? vertexDataVariable;
    internal ShaderCompilation compilation;
    internal int vsOutLocation = 0;
    int vertexLayoutLocation;

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
        if (type is ShaderReferenceType refType)
        {
            writer.Write("inout ");
            EmitType(refType.ElementType);
            return;
        }

        if (type.GetPrimitiveKind() is ShaderPrimitiveKind primitive)
        {
            writer.Write(primitive switch
            {
                ShaderPrimitiveKind.Void => "void",
                ShaderPrimitiveKind.Bool => "bool",
                ShaderPrimitiveKind.Float => "float",
                ShaderPrimitiveKind.Float2 => "vec2",
                ShaderPrimitiveKind.Float3 => "vec3",
                ShaderPrimitiveKind.Float4 => "vec4",
                ShaderPrimitiveKind.Int => "int",
                ShaderPrimitiveKind.Int2 => "vec2i",
                ShaderPrimitiveKind.Int3 => "vec3i",
                ShaderPrimitiveKind.Int4 => "vec4i",
                ShaderPrimitiveKind.Matrix3x2 => "mat3x2",
                ShaderPrimitiveKind.Matrix4x4 => "mat4",
                ShaderPrimitiveKind.Texture => "sampler2D",
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
            EmitVertexDeclaration(variable.Type, variable.Name.value, ref vertexLayoutLocation);
            return;
        }
        else if (variable.Kind == ShaderVariableKind.InstanceData)
        {
            EmitVertexDeclaration(variable.Type, variable.Name.value, ref vertexLayoutLocation);
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

            if (type.GetPrimitiveKind() == ShaderPrimitiveKind.Matrix4x4)
            {
                location += 4;
            }
            else if (type.GetPrimitiveKind() == ShaderPrimitiveKind.Matrix3x2)
            {
                location += 3;
            }
            else
            {
                location++;
            }
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
