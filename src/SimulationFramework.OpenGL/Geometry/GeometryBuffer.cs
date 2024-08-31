using System;

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
            glCreateBuffers(1, bufferPtr);
            glNamedBufferData(buffer, size, null, GL_DYNAMIC_DRAW);
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
            glNamedBufferSubData(buffer, 0, size, dataPtr);
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
