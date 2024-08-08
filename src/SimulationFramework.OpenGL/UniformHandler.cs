using SimulationFramework.Drawing.Shaders;
using SimulationFramework.Drawing.Shaders.Compiler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SimulationFramework.OpenGL;

class UniformHandler
{
    private readonly Dictionary<ShaderVariable, int> uniformLocations = [];
    private readonly Dictionary<ShaderVariable, SSBO> buffers = [];
    private readonly uint program;
    
    private int textureSlot, bufferSlot;

    public UniformHandler(uint program)
    {
        this.program = program;
    }

    public void SetUniforms(ShaderCompilation compilation, Shader shader)
    {
        Reset();
        foreach (var variable in compilation.Variables)
        {
            if (variable.Kind == ShaderVariableKind.Uniform)
            {
                SetUniform(variable, shader);
            }
        }
    }

    public void Reset()
    {
        textureSlot = 0;
        bufferSlot = 0;
    }

    public void SetUniform(ShaderVariable uniform, object parent)
    {
        SetUniform(uniform, parent, null);
    }

    unsafe void SetUniform(ShaderVariable uniform, object parent, string? baseName)
    {
        var name = baseName is null ? uniform.Name.value : baseName + uniform.Name.value;
        var location = GetUniformLocation(uniform, name);

        var field = (FieldInfo?)uniform.BackingMember!;
        var value = field.GetValue(parent)!;

        switch (uniform.Type)
        {
            case ShaderType when uniform.Type.GetPrimitiveKind() is PrimitiveKind primitiveKind:
                SetMemberUniform(primitiveKind, value!, location);
                break;
            case ShaderStructureType structType:
                foreach (var member in structType.structure.fields)
                {
                    SetUniform(member, value, name + ".");
                }
                break;
            case ShaderArrayType arrayType:
                SetArrayUniform(uniform, arrayType, value);
                break;
        }
    }

    private unsafe void SetArrayUniform(ShaderVariable uniform, ShaderArrayType arrayType, object value)
    {
        if (!buffers.TryGetValue(uniform, out SSBO? ssbo))
        {
            ssbo = new(program, uniform);
            buffers[uniform] = ssbo;
        }

        Array array = (Array)value;
        GCHandle arrayHandle = GCHandle.Alloc(array, GCHandleType.Pinned);
        try
        {
            void* ptr = Unsafe.AsPointer(ref MemoryMarshal.GetArrayDataReference(array));
            ssbo.WriteData(ptr, array.Length);

            glBindBufferBase(GL_SHADER_STORAGE_BUFFER, (uint)bufferSlot, ssbo.GetBuffer());
            glBindBuffer(GL_SHADER_STORAGE_BUFFER, 0);
        }
        finally
        {
            arrayHandle.Free();
        }

        bufferSlot++;
    }

    private void SetTextureUniform(GLTexture? texture, int location)
    {
        glActiveTexture(GL_TEXTURE0 + (uint)textureSlot);
        glBindTexture(GL_TEXTURE_2D, texture?.GetID() ?? 0);
        glUniform1i(location, textureSlot);
        textureSlot++;
    }

    private unsafe void SetMemberUniform(PrimitiveKind primitive, object value, int location)
    {
        if (primitive is PrimitiveKind.Texture)
        {
            SetTextureUniform((GLTexture?)value, location);
            return;
        }

        GCHandle valueHandle = GCHandle.Alloc(value, GCHandleType.Pinned);
        void* ptr = (void*)valueHandle.AddrOfPinnedObject();

        switch (primitive)
        {
            case PrimitiveKind.Bool:
                throw new NotImplementedException();
            case PrimitiveKind.Int:
                glUniform1iv(location, 1, (int*)ptr);
                break;
            case PrimitiveKind.Int2:
                glUniform2iv(location, 1, (int*)ptr);
                break;
            case PrimitiveKind.Int3:
                glUniform3iv(location, 1, (int*)ptr);
                break;
            case PrimitiveKind.Int4:
                glUniform4iv(location, 1, (int*)ptr);
                break;
            case PrimitiveKind.Float:
                glUniform1fv(location, 1, (float*)ptr);
                break;
            case PrimitiveKind.Float2:
                glUniform2fv(location, 1, (float*)ptr);
                break;
            case PrimitiveKind.Float3:
                glUniform3fv(location, 1, (float*)ptr);
                break;
            case PrimitiveKind.Float4:
                glUniform4fv(location, 1, (float*)ptr);
                break;
            case PrimitiveKind.Matrix4x4:
                glUniformMatrix4fv(location, 1, (byte)GL_FALSE, (float*)ptr);
                break;
            case PrimitiveKind.Matrix3x2:
                glUniformMatrix3x2fv(location, 1, (byte)GL_FALSE, (float*)ptr);
                break;
            default:
                throw new NotImplementedException();
        }

        valueHandle.Free();
    }

    unsafe int GetUniformLocation(ShaderVariable uniform, string? nameOverride = null)
    {
        if (uniformLocations.TryGetValue(uniform, out int result))
        {
            return result;
        }

        fixed (byte* namePtr = Encoding.UTF8.GetBytes(nameOverride ?? uniform.Name.value))
        {
            result = glGetUniformLocation(program, namePtr);
            uniformLocations[uniform] = result;
        }

        return result;
    }

    internal SSBO GetSSBO(ShaderVariable uniform)
    {
        return buffers[uniform];
    }
}

unsafe class SSBO
{
    readonly Field[] fields;
    readonly int arrayStride;
    readonly int bufferStride;

    private uint buffer;
    private int size = 128;


    public SSBO(uint program, ShaderVariable uniform)
    {
        List<Field> fields = [];
        CreateFields(program, uniform, fields);
        this.fields = fields.ToArray();

        uint prop = GL_TOP_LEVEL_ARRAY_STRIDE;
        int stride;
        glGetProgramResourceiv(program, GL_BUFFER_VARIABLE, 0, 1, &prop, 1, null, &stride);
        bufferStride = stride;

        arrayStride = ShaderType.CalculateTypeSize((uniform.Type as ShaderArrayType)!.ElementType);
    }

    private void CreateFields(uint program, ShaderVariable uniform, List<Field> fields)
    {
        Debug.Assert(uniform.Type is ShaderArrayType);
        int arrayOffset = 0;
        CreateFieldsHelper((uniform.Type as ShaderArrayType)!.ElementType, uniform.Name.value + "[0]");

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

    private void CopyArrayToBuffer(void* array, void* buffer, int count)
    {
        for (int i = 0; i < fields.Length; i++)
        {
            byte* arrayBytes = (byte*)array;
            byte* bufferBytes = (byte*)buffer;
            Field field = fields[i];

            for (int j = 0; j < count; j++)
            {
                Buffer.MemoryCopy(arrayBytes + field.arrayOffset, bufferBytes + field.bufferOffset, field.size, field.size);
                arrayBytes += arrayStride;
                bufferBytes += bufferStride;
            }
        }
    }

    private void CopyBufferToArray(void* array, void* buffer, int count)
    {
        for (int i = 0; i < fields.Length; i++)
        {
            byte* arrayBytes = (byte*)array;
            byte* bufferBytes = (byte*)buffer;
            Field field = fields[i];

            for (int j = 0; j < count; j++)
            {
                Buffer.MemoryCopy(bufferBytes + field.bufferOffset, arrayBytes + field.arrayOffset, field.size, field.size);
                arrayBytes += arrayStride;
                bufferBytes += bufferStride;
            }
        }
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
                glGetNamedBufferParameteriv(buffer, GL_BUFFER_SIZE, &currentSize);
            }
            if (sizeInBytes != currentSize)
            {
                if (buffer != 0)
                {
                    glDeleteBuffers(1, bufferPtr);
                }

                glCreateBuffers(1, bufferPtr);
                glNamedBufferStorage(buffer, sizeInBytes, null, GL_DYNAMIC_STORAGE_BIT | GL_MAP_READ_BIT | GL_MAP_WRITE_BIT);
            }
            size = sizeInBytes;
        }
    }

    public void WriteData(void* data, int count)
    {
        SetBufferSize(count * bufferStride);

        void* mappedBuf = glMapNamedBuffer(buffer, GL_WRITE_ONLY);
        CopyArrayToBuffer(data, mappedBuf, count);
        glUnmapNamedBuffer(buffer);
    }

    public void Read(void* outData, int count)
    {
        void* mappedBuf = glMapNamedBuffer(buffer, GL_READ_ONLY);
        CopyBufferToArray(outData, mappedBuf, count);
        glUnmapNamedBuffer(buffer);
    }

    private struct Field
    {
        public int size;
        public int bufferOffset;
        public int arrayOffset;
    }
}