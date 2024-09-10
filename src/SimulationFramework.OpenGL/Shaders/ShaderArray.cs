using SimulationFramework.Drawing.Shaders.Compiler;
using System;
using System.Diagnostics;

namespace SimulationFramework.OpenGL;

unsafe abstract class ShaderArray
{
    protected readonly Field[] fields;
    protected readonly int arrayStride;
    protected readonly int bufferStride;

    public ShaderArray(uint program, ShaderVariable uniform)
    {
        Debug.Assert(uniform.Type is ShaderArrayType);

        fields = CreateFields(program, uniform, out arrayStride, out bufferStride);
    }

    public abstract void WriteData(void* data, int count);
    public abstract void ReadData(void* outData, int count);
    public abstract void Bind(UniformHandler handler);

    protected abstract Field[] CreateFields(uint program, ShaderVariable uniform, out int arrayStride, out int bufferStride);

    protected void CopyArrayToBuffer(void* array, void* buffer, int count)
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

    protected void CopyBufferToArray(void* array, void* buffer, int count)
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

    public struct Field
    {
        public int size;
        public int bufferOffset;
        public int arrayOffset;
    }
}
