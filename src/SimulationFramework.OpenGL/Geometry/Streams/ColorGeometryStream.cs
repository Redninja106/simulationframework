﻿using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SimulationFramework.OpenGL.Geometry.Streams;

unsafe class ColorGeometryStream : GeometryStream
{
    private List<ColorVertex> vertices = [];
    public Color Color { get; set; }
    public override VertexLayout VertexLayout { get; } = VertexLayout.Get(typeof(ColorVertex));

    public ColorGeometryStream()
    {
    }

    public override void WriteVertex(Vector2 position)
    {
        vertices.Add(new()
        {
            position = Vector2.Transform(position, TransformMatrix),
            color = Color,
        });
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
