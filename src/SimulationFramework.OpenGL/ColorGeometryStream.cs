using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SimulationFramework.OpenGL;

unsafe class ColorGeometryStream : GeometryStream
{
    private List<ColorVertex> vertices = [];
    public Color Color { get; set; }
    private uint vao;

    public ColorGeometryStream()
    {
        fixed (uint* vaoPtr = &vao)
        {
            glGenVertexArrays(1, vaoPtr);
            glBindVertexArray(vao);

            glVertexAttribPointer(0, 2, GL_FLOAT, (byte)GL_FALSE, Unsafe.SizeOf<ColorVertex>(), null);
            glEnableVertexAttribArray(0);
            glVertexAttribPointer(1, 4, GL_UNSIGNED_BYTE, (byte)GL_TRUE, Unsafe.SizeOf<ColorVertex>(), (void*)(sizeof(float) * 2));
            glEnableVertexAttribArray(1);

            glBindVertexArray(0);
        }
    }

    public override void WriteVertex(Vector2 position)
    {
        vertices.Add(new()
        {
            position = Vector2.Transform(position, TransformMatrix),
            color = Color,
        });
    }

    public override void BindVertexArray()
    {
        glBindVertexArray(vao);
    }

    public override void Clear()
    {
        vertices.Clear();
    }

    public override int GetVertexCount()
    {
        return vertices.Count;
    }

    public override void Upload(GeometryBuffer buffer)
    {
        Span<ColorVertex> data = CollectionsMarshal.AsSpan(vertices);
        buffer.UpdateData(MemoryMarshal.AsBytes(data));
    }

    struct ColorVertex
    {
        public Vector2 position;
        public Color color;
    }
}
