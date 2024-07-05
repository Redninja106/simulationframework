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

            glVertexAttribPointer(0, 2, GL_FLOAT, (byte)GL_FALSE, 2 * sizeof(float), null);
            glEnableVertexAttribArray(0);

            glBindVertexArray(0);
        }
    }

    public override void WriteVertex(Vector2 position)
    {
        vertices.Add(position);
    }

    public override void Upload(GeometryBuffer buffer)
    {
        Span<Vector2> span = CollectionsMarshal.AsSpan(vertices);
        buffer.UpdateData(MemoryMarshal.AsBytes(span));
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
    }
}

unsafe class GeometryBuffer
{
    public uint buffer;
    int size = 0;

    public GeometryBuffer()
    {
        fixed (uint* bufferPtr = &buffer)
        {
            glGenBuffers(1, bufferPtr);
        }
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

    public int UpdateData(ReadOnlySpan<byte> bytes)
    {
        EnsureSize(bytes.Length);
        fixed (byte* bytesPtr = bytes)
        {
            glBindBuffer(GL_ARRAY_BUFFER, buffer);
            glBufferSubData(GL_ARRAY_BUFFER, 0, bytes.Length, bytesPtr);
            glBindBuffer(GL_ARRAY_BUFFER, 0);
        }
        return bytes.Length;
    }
}
