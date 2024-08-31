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

    public override VertexLayout VertexLayout { get; } = VertexLayout.Get(typeof(Vector2));

    public PositionGeometryStream()
    {
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
}
