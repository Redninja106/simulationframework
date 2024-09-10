using System;
using System.Runtime.CompilerServices;

namespace SimulationFramework.OpenGL.Geometry;

unsafe class GeometryBuffer : IDisposable
{
    public readonly uint buffer;
    public readonly int size;

    public GeometryBuffer(int size)
    {
        this.size = size;

        fixed (uint* bufferPtr = &buffer)
        {
            glGenBuffers(1, bufferPtr);
            glBindBuffer(GL_ARRAY_BUFFER, buffer);
            glBufferData(GL_ARRAY_BUFFER, size, null, GL_DYNAMIC_DRAW);
        }
    }

    public void Reset()
    {
    }

    public void Upload<T>(ReadOnlySpan<T> data)
        where T : unmanaged
    {
        fixed (T* dataPtr = data)
        {
            glBindBuffer(GL_ARRAY_BUFFER, buffer);
            glBufferSubData(GL_ARRAY_BUFFER, 0, data.Length * Unsafe.SizeOf<T>(), dataPtr);
        }
    }

    public void Bind()
    {
        glBindBuffer(GL_ARRAY_BUFFER, buffer);
    }

    public void BindAsIndexBuffer()
    {
        glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, buffer);
    }

    public void Dispose()
    {
        fixed (uint* bufferPtr = &this.buffer)
        {
            glDeleteBuffers(1, bufferPtr);
        }
    }

}
