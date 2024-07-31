using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SimulationFramework.OpenGL;

unsafe class PositionGeometryStream : GeometryStream
{
    internal List<Vector2> vertices = [];
    private uint vao;

    public PositionGeometryStream()
    {
        fixed (uint* vaoPtr = &vao)
        {
            glGenVertexArrays(1, vaoPtr);
            glBindVertexArray(vao);
        }
    }

    public override int GetVertexSize()
    {
        return sizeof(float) * 2;
    }

    public override void WriteVertex(Vector2 position)
    {
        vertices.Add(Vector2.Transform(position, TransformMatrix));
    }

    public override void Upload(GeometryBuffer buffer)
    {
        Span<Vector2> span = CollectionsMarshal.AsSpan(vertices);
        buffer.WriteData(MemoryMarshal.AsBytes(span));
    }

    public override int GetVertexCount()
    {
        return vertices.Count;
    }

    public override void Clear()
    {
        vertices.Clear();
    }

    public override void BindVertexArray()
    {
        glBindVertexArray(vao);
        glVertexAttribPointer(0, 2, GL_FLOAT, (byte)GL_FALSE, 2 * sizeof(float), null);
        glEnableVertexAttribArray(0);
        glDisableVertexAttribArray(1);
        glDisableVertexAttribArray(2);
    }
}

class GeometryBufferPool
{
    private readonly Queue<GeometryBuffer> freeBuffers = [];
    private readonly List<GeometryBuffer> usedBuffers = [];

    public GeometryBufferPool()
    {
    }

    public GeometryBuffer Take()
    {
        GeometryBuffer buffer;
        if (freeBuffers.Count > 0)
        {
            buffer = freeBuffers.Dequeue();
        }
        else
        {
            buffer = new GeometryBuffer();
        }

        usedBuffers.Add(buffer);
        return buffer;
    }

    public void Return(GeometryBuffer buffer)
    {
        usedBuffers.Remove(buffer);
        freeBuffers.Enqueue(buffer);
    }

    public void Reset()
    {
        while (usedBuffers.Count > 0)
        {
            var buffer = usedBuffers[0];
            freeBuffers.Enqueue(buffer);
            usedBuffers.RemoveAt(0);
        }
    }
}

unsafe class GeometryBuffer
{
    public uint buffer;
    int size = 0;
    public int offset;

    public GeometryBuffer()
    {
        fixed (uint* bufferPtr = &buffer)
        {
            glGenBuffers(1, bufferPtr);
        }
    }

    public void Reset()
    {
        offset = 0;
    }

    public void Bind()
    {
        glBindBuffer(GL_ARRAY_BUFFER, buffer);
    }

    public void EnsureSize(int size)
    {
        if (this.size < size)
        {
            glBindBuffer(GL_ARRAY_BUFFER, buffer);
            glBufferData(GL_ARRAY_BUFFER, size, null, GL_DYNAMIC_DRAW);
            glBindBuffer(GL_ARRAY_BUFFER, 0);
            this.size = size;
        }
    }

    public int WriteData(ReadOnlySpan<byte> bytes)
    {
        EnsureSize(offset + bytes.Length);
        fixed (byte* bytesPtr = bytes)
        {
            glBindBuffer(GL_ARRAY_BUFFER, buffer);
            glBufferSubData(GL_ARRAY_BUFFER, offset, bytes.Length, bytesPtr);
            glBindBuffer(GL_ARRAY_BUFFER, 0);
        }
        offset += bytes.Length;
        return bytes.Length;
    }
}
