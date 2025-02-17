using SimulationFramework.Drawing.Shaders.Compiler;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace SimulationFramework.OpenGL;

unsafe class ShaderTypeLayout
{
    private static Dictionary<Type, ShaderTypeLayout> layouts = [];

    public static ShaderTypeLayout Get(Type type)
    {
        if (!layouts.TryGetValue(type, out var layout))
        {
            layouts[type] = layout = new(type);
        }

        return layout;
    }

    public readonly ImmutableArray<Field> fields;
    public readonly int arrayStride;
    public readonly int bufferStride;
    private readonly bool isTightlyPacked;

    // https://registry.khronos.org/OpenGL/specs/gl/glspec45.core.pdf#page=138
    private ImmutableArray<Field> CreateFieldsSTD430(Type elementType, out int arrayStride, out int bufferStride)
    {
        List<Field> fields = [];
        int bufferOffset = 0; // in "basic machine units"
        int arrayEnd = 0;

        CreateMemberFields(fields, elementType, 0, string.Empty, out bufferStride);
        arrayStride = arrayEnd;

        return [..fields];

        static int AlignOffset(int offset, int align) => (offset + align - 1) & ~(align - 1);

        void CreateMemberFields(List<Field> fields, Type memberType, int memberOffset, string name, out int bufferStride)
        {
            if (ShaderType.PrimitiveTypeMap.TryGetValue(memberType, out ShaderType? memberShaderType))
            {
                ShaderPrimitiveKind? primitive = memberShaderType.GetPrimitiveKind();

                // TODO: support bools and matrices here
                int channels = primitive switch
                {
                    ShaderPrimitiveKind.UInt or ShaderPrimitiveKind.Int or ShaderPrimitiveKind.Float => 1,
                    ShaderPrimitiveKind.Float2 or ShaderPrimitiveKind.Int2 or ShaderPrimitiveKind.UInt2 => 2,
                    ShaderPrimitiveKind.Float3 or ShaderPrimitiveKind.Int3 or ShaderPrimitiveKind.UInt3 => 3,
                    ShaderPrimitiveKind.Float4 or ShaderPrimitiveKind.Int4 or ShaderPrimitiveKind.UInt4 => 4,
                    ShaderPrimitiveKind.Matrix3x2 => 8,
                    ShaderPrimitiveKind.Matrix4x4 => 16,
                    _ => throw new NotSupportedException("type " + memberType.Name + " is not supported!"),
                };
                
                int align = channels;
                if (align == 3)
                {
                    align = 4;
                }

                bufferOffset = AlignOffset(bufferOffset, align);

                int size = channels;

                fields.Add(new()
                {
                    arraySize = size * 4,
                    arrayOffset = memberOffset,
                    bufferSize = size,
                    bufferOffset = bufferOffset ,
                    primitiveKind = primitive.Value,
                    fullName = name,
                });

                bufferOffset += channels;

                bufferStride = align;

                // TODO: replace with RuntimeHelpers.SizeOf
                arrayEnd = memberOffset + size * 4;
            }
            else 
            {
                bufferStride = 0;
                int bufferAlignment = 0;
                foreach (var field in elementType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    CreateMemberFields(fields, field.FieldType, GetFieldOffset(field), name + "." + field.Name, out int bufAlign);
                    bufferAlignment = Math.Max(bufferAlignment, bufAlign);
                    bufferStride = AlignOffset(bufferStride, bufAlign);
                    bufferStride += bufAlign;
                }
                bufferStride = AlignOffset(bufferStride, bufferAlignment);
            }
        }
        static int GetFieldOffset(FieldInfo field)
        {
            return Marshal.ReadInt32(field.FieldHandle.Value + (4 + IntPtr.Size)) & 0xFFFFFF;
        }
    }

    public ShaderTypeLayout(Type type)
    {
        // Debug.Assert(uniform.Type is ShaderArrayType);
        
        fields = CreateFieldsSTD430(type, out this.arrayStride, out this.bufferStride);
    }

    public void CopyArrayToBuffer(void* array, void* buffer, int count)
    {
        for (int i = 0; i < fields.Length; i++)
        {
            byte* arrayBytes = (byte*)array;
            byte* bufferBytes = (byte*)buffer;
            Field field = fields[i];

            for (int j = 0; j < count; j++)
            {
                // TODO: support types with different sizes
                // Debug.Assert(field.bufferSize == field.arraySize);

                Buffer.MemoryCopy(arrayBytes + field.arrayOffset, bufferBytes + field.bufferOffset * 4, field.bufferSize * 4, field.arraySize);
                arrayBytes += arrayStride;
                bufferBytes += bufferStride * 4;
            }
        }
    }

    public void CopyBufferToArray(void* array, void* buffer, int count)
    {
        for (int i = 0; i < fields.Length; i++)
        {
            byte* arrayBytes = (byte*)array;
            byte* bufferBytes = (byte*)buffer;
            Field field = fields[i];

            for (int j = 0; j < count; j++)
            {
                // TODO: proper handling when bufferSize != arraySize
                Buffer.MemoryCopy(bufferBytes + field.bufferOffset * 4, arrayBytes + field.arrayOffset, field.bufferSize * 4, field.arraySize);
                arrayBytes += arrayStride;
                bufferBytes += bufferStride * 4;
            }
        }
    }

    //// gles 3.1+ way of handling shader arrays
    //private Field[] CreateFieldsSSBO(uint program, ShaderVariable uniform, out int arrayStride, out int bufferStride)
    //{
    //    List<Field> fields = [];

    //    ShaderType elementType = (uniform.Type as ShaderArrayType)!.ElementType;

    //    arrayStride = ShaderType.CalculateTypeSize(elementType);
    //    bufferStride = QueryBufferStride(program, uniform);

    //    int arrayOffset = 0;
    //    CreateFieldsHelper((uniform.Type as ShaderArrayType)!.ElementType, uniform.Name.value + "[0]");

    //    return fields.ToArray();

    //    void CreateFieldsHelper(ShaderType type, string name)
    //    {
    //        if (type is ShaderStructureType structType)
    //        {
    //            foreach (var field in structType.structure.fields)
    //            {
    //                CreateFieldsHelper(field.Type, name + "." + field.Name);
    //            }
    //        }
    //        else if (type.GetPrimitiveKind() != null)
    //        {
    //            uint index;
    //            fixed (byte* namePtr = Encoding.UTF8.GetBytes(name))
    //            {
    //                index = glGetProgramResourceIndex(program, GL_BUFFER_VARIABLE, namePtr);
    //            }

    //            uint props = GL_OFFSET;
    //            int bufferOffset = 0;
    //            glGetProgramResourceiv(program, GL_BUFFER_VARIABLE, index, 1, &props, 1, null, &bufferOffset);

    //            int size = ShaderType.CalculateTypeSize(type);
    //            fields.Add(new Field()
    //            {
    //                arrayOffset = arrayOffset,
    //                bufferOffset = bufferOffset,
    //                size = size,
    //            });
    //            arrayOffset += size;
    //        }
    //        else
    //        {
    //            throw new();
    //        }
    //    }

    //    static int QueryBufferStride(uint program, ShaderVariable uniform)
    //    {
    //        int bufferStride = 0;

    //        // for some reason on primitive arrays GL_TOP_LEVEL_ARRAY_STRIDE returns
    //        // 0 while on arrays of structs GL_ARRAY_STRIDE returns 0

    //        uint strideProperty = GL_ARRAY_STRIDE;
    //        var firstFieldName = uniform.Name.ToString();
    //        var firstFieldType = uniform.Type;
    //        while (true)
    //        {
    //            if (firstFieldType is ShaderArrayType arrayType)
    //            {
    //                firstFieldType = arrayType.ElementType;
    //                firstFieldName += "[0]";
    //            }
    //            else if (firstFieldType is ShaderStructureType structType)
    //            {
    //                firstFieldType = structType.structure.fields[0].Type;
    //                firstFieldName += "." + structType.structure.fields[0].Name.ToString();
    //                strideProperty = GL_TOP_LEVEL_ARRAY_STRIDE;
    //            }
    //            else if (firstFieldType.GetPrimitiveKind() is ShaderPrimitiveKind primitiveKind)
    //            {
    //                break;
    //            }
    //            else
    //            {
    //                throw new();
    //            }
    //        }

    //        uint index;
    //        fixed (byte* namePtr = Encoding.UTF8.GetBytes(firstFieldName))
    //        {
    //            index = glGetProgramResourceIndex(program, GL_BUFFER_VARIABLE, namePtr);
    //        }

    //        if (index == uint.MaxValue)
    //        {
    //            throw new();
    //        }

    //        glGetProgramResourceiv(program, GL_BUFFER_VARIABLE, index, 1, &strideProperty, 1, null, &bufferStride);
    //        return bufferStride;
    //    }
    //}

    public struct Field
    {
        public int arraySize; // in bytes
        public int arrayOffset; // in bytes
        public int bufferSize; // in machine units (4 bytes)
        public int bufferOffset; // in machine units (4 bytes)
        public ShaderPrimitiveKind primitiveKind;
        public string fullName; // prefixed with a '.' (ie ".some.nested.value")
    }
}
