using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SimulationFramework.OpenGL.Geometry.Streams;

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
        }
    }

    public override int GetVertexSize()
    {
        return Unsafe.SizeOf<ColorVertex>();
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
        glVertexAttribPointer(0, 2, GL_FLOAT, (byte)GL_FALSE, Unsafe.SizeOf<ColorVertex>(), null);
        glEnableVertexAttribArray(0);
        glVertexAttribPointer(1, 4, GL_UNSIGNED_BYTE, (byte)GL_TRUE, Unsafe.SizeOf<ColorVertex>(), (void*)(sizeof(float) * 2));
        glEnableVertexAttribArray(1);
    }

    public override void Clear()
    {
        vertices.Clear();
    }

    public override int GetVertexCount()
    {
        return vertices.Count;
    }

    public override ReadOnlySpan<byte> GetData()
    {
        return MemoryMarshal.AsBytes(CollectionsMarshal.AsSpan(this.vertices));
    }

    struct ColorVertex
    {
        public Vector2 position;
        public Color color;
    }
}
