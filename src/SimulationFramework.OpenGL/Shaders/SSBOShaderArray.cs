using SimulationFramework.Drawing;
using SimulationFramework.Drawing.Shaders.Compiler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml.Linq;

namespace SimulationFramework.OpenGL;

unsafe class SSBOShaderArray : ShaderArray
{
    private uint buffer;
    private int size = 128;

    public SSBOShaderArray(uint program, ShaderVariable uniform) : base(program, uniform)
    {
    }

    public override void Bind(UniformHandler handler)
    {
        glBindBufferBase(GL_SHADER_STORAGE_BUFFER, (uint)handler.bufferSlot++, buffer);
        glBindBuffer(GL_SHADER_STORAGE_BUFFER, 0);
    }

    protected override Field[] CreateFields(uint program, ShaderVariable uniform, out int arrayStride, out int bufferStride)
    {
        List<Field> fields = [];

        ShaderType elementType = (uniform.Type as ShaderArrayType)!.ElementType;
        
        arrayStride = ShaderType.CalculateTypeSize(elementType);
        bufferStride = QueryBufferStride(program, uniform);

        int arrayOffset = 0;
        CreateFieldsHelper((uniform.Type as ShaderArrayType)!.ElementType, uniform.Name.value + "[0]");

        return fields.ToArray();

        void CreateFieldsHelper(ShaderType type, string name)
        {
            if (type is ShaderStructureType structType)
            {
                foreach (var field in structType.structure.fields)
                {
                    CreateFieldsHelper(field.Type, name + "." + field.Name);
                }
            }
            else if (type.GetPrimitiveKind() != null)
            {
                uint index;
                fixed (byte* namePtr = Encoding.UTF8.GetBytes(name))
                {
                    index = glGetProgramResourceIndex(program, GL_BUFFER_VARIABLE, namePtr);
                }

                uint props = GL_OFFSET;
                int bufferOffset = 0;
                glGetProgramResourceiv(program, GL_BUFFER_VARIABLE, index, 1, &props, 1, null, &bufferOffset);

                int size = ShaderType.CalculateTypeSize(type);
                fields.Add(new Field()
                {
                    arrayOffset = arrayOffset,
                    bufferOffset = bufferOffset,
                    size = size,
                });
                arrayOffset += size;
            }
            else
            {
                throw new();
            }
        }
    }

    private int QueryBufferStride(uint program, ShaderVariable uniform)
    {
        int bufferStride = 0;

        // for some reason on primitive arrays GL_TOP_LEVEL_ARRAY_STRIDE returns
        // 0 while on arrays of structs GL_ARRAY_STRIDE returns 0

        uint strideProperty = GL_ARRAY_STRIDE;
        var firstFieldName = uniform.Name.ToString();
        var firstFieldType = uniform.Type;
        while (true)
        {
            if (firstFieldType is ShaderArrayType arrayType)
            {
                firstFieldType = arrayType.ElementType;
                firstFieldName += "[0]";
            }
            else if (firstFieldType is ShaderStructureType structType)
            {
                firstFieldType = structType.structure.fields[0].Type;
                firstFieldName += "." + structType.structure.fields[0].Name.ToString();
                strideProperty = GL_TOP_LEVEL_ARRAY_STRIDE;
            }
            else if (firstFieldType.GetPrimitiveKind() is ShaderPrimitiveKind primitiveKind)
            {
                break;
            }
            else
            {
                throw new();
            }
        }

        uint index;
        fixed (byte* namePtr = Encoding.UTF8.GetBytes(firstFieldName))
        {
            index = glGetProgramResourceIndex(program, GL_BUFFER_VARIABLE, namePtr);

        }

        if (index == uint.MaxValue)
        {
            throw new();
        }
        
        glGetProgramResourceiv(program, GL_BUFFER_VARIABLE, index, 1, &strideProperty, 1, null, &bufferStride);
        return bufferStride;
    }

    public uint GetBuffer()
    {
        return buffer;
    }

    private void SetBufferSize(int sizeInBytes)
    {
        fixed (uint* bufferPtr = &buffer)
        {
            int currentSize = 0;
            if (buffer != 0)
            {
                glBindBuffer(GL_SHADER_STORAGE_BUFFER, buffer);
                glGetBufferParameteriv(GL_SHADER_STORAGE_BUFFER, GL_BUFFER_SIZE, &currentSize);
            }
            if (sizeInBytes != currentSize)
            {
                if (buffer != 0)
                {
                    glDeleteBuffers(1, bufferPtr);
                }

                glGenBuffers(1, bufferPtr);
                glBindBuffer(GL_SHADER_STORAGE_BUFFER, buffer);
                glBufferData(GL_SHADER_STORAGE_BUFFER, sizeInBytes, null, GL_DYNAMIC_DRAW);
            }
            size = sizeInBytes;
        }
    }

    public override void WriteData(void* data, int count)
    {
        SetBufferSize(count * bufferStride);

        if (count != 0)
        {
            glBindBuffer(GL_SHADER_STORAGE_BUFFER, buffer);
            void* mappedBuf = glMapBufferRange(GL_SHADER_STORAGE_BUFFER, 0, size, GL_MAP_WRITE_BIT);
            CopyArrayToBuffer(data, mappedBuf, count);
            glUnmapBuffer(GL_SHADER_STORAGE_BUFFER);
        }
    }

    public override void ReadData(void* outData, int count)
    {
        glBindBuffer(GL_SHADER_STORAGE_BUFFER, buffer);
        void* mappedBuf = glMapBufferRange(GL_SHADER_STORAGE_BUFFER, 0, size, GL_MAP_READ_BIT);
        CopyBufferToArray(outData, mappedBuf, count);
        glUnmapBuffer(GL_SHADER_STORAGE_BUFFER);
    }

}
