using System;

namespace SimulationFramework.OpenGL.Geometry;

unsafe class GeometryBuffer : IDisposable
{
    public readonly uint buffer;
    public readonly int size;
    private byte[] data;
    private int position;

    public GeometryBuffer(int size)
    {
        this.size = size;
        data = new byte[this.size];

        fixed (uint* bufferPtr = &buffer)
        {
            glCreateBuffers(1, bufferPtr);
            glNamedBufferData(buffer, size, null, GL_DYNAMIC_DRAW);
        }
    }

    public void Reset()
    {
        position = 0;
    }

    public void Upload()
    {
        fixed (byte* dataPtr = data)
        {
            glNamedBufferSubData(buffer, 0, size, dataPtr);
        }
    }

    public void Bind()
    {
        glBindBuffer(GL_ARRAY_BUFFER, buffer);
    }

    public void Write(ReadOnlySpan<byte> bytes, int alignment, out int offset, out int count)
    {
        if (!TryWrite(bytes, alignment, out offset, out count))
        {
            throw new Exception("buffer is full");
        }
    }

    public bool TryWrite(ReadOnlySpan<byte> bytes, int alignment, out int offset, out int count)
    {
        int alignedPosition = position;
        if (alignedPosition % alignment != 0)
        {
            alignedPosition += alignment - alignedPosition % alignment;
        }

        if (alignedPosition + bytes.Length > this.size)
        {
            offset = count = 0;
            return false;
        }

        bytes.CopyTo(data.AsSpan(alignedPosition, bytes.Length));
        position = alignedPosition + bytes.Length;

        offset = alignedPosition;
        count = bytes.Length;
        return true;
    }

    public void Dispose()
    {
        fixed (uint* bufferPtr = &this.buffer)
        {
            glDeleteBuffers(1, bufferPtr);
        }
    }
}
