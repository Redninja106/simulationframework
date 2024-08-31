using SimulationFramework.Drawing.Shaders.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;

namespace SimulationFramework.OpenGL.Geometry.Streams;

class CustomVertexGeometryStream : GeometryStream
{
    internal List<byte> vertexData = [];
    private Type vertexType;

    public override VertexLayout VertexLayout { get; }

    public unsafe CustomVertexGeometryStream(Type vertexType)
    {
        var graphics = Application.GetComponent<GLGraphics>();
        this.vertexType = vertexType;
        VertexLayout = VertexLayout.Get(vertexType);
    }


    public override void WriteVertex(Vector2 position)
    {
        throw new NotSupportedException();
    }

    public override ReadOnlySpan<byte> GetData()
    {
        return CollectionsMarshal.AsSpan(this.vertexData);
    }

    public void WriteVertex<TVertex>(TVertex vertex)
        where TVertex : unmanaged
    {
        var span = new Span<TVertex>(ref vertex);
        var bytes = MemoryMarshal.AsBytes(span);
        vertexData.AddRange(bytes);
    }

    public override int GetVertexCount()
    {
        return vertexData.Count / VertexLayout.VertexSize;
    }

    public override void Clear()
    {
        vertexData.Clear();
    }
}
