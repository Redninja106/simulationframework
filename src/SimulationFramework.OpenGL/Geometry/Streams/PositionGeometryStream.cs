using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SimulationFramework.OpenGL.Geometry.Streams;

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

    public override ReadOnlySpan<byte> GetData()
    {
        return MemoryMarshal.AsBytes(CollectionsMarshal.AsSpan(vertices));
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
