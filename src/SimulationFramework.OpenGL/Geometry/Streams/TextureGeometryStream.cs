using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.OpenGL.Geometry.Streams;
internal class TextureGeometryStream : GeometryStream
{
    private List<TextureVertex> vertices = [];
    private int texCoordPosition;

    public override VertexLayout VertexLayout { get; } = VertexLayout.Get(typeof(TextureVertex));

    public unsafe TextureGeometryStream()
    {
    }

    public override void Clear()
    {
        vertices.Clear();
    }

    public override int GetVertexCount()
    {
        return vertices.Count;
    }

    public override void WriteVertex(Vector2 position)
    {
        throw new InvalidOperationException();
    }

    public void WriteVertexFlipUV(Vector2 position, Vector2 textureCoordinate)
    {
        vertices.Add(new()
        {
            position = Vector2.Transform(position, TransformMatrix),
            textureCoordinate = new(textureCoordinate.X, 1f - textureCoordinate.Y),
        });
    }

    public void WriteVertex(Vector2 position, Vector2 textureCoordinate)
    {
        vertices.Add(new()
        {
            position = Vector2.Transform(position, TransformMatrix),
            textureCoordinate = new(textureCoordinate.X, textureCoordinate.Y),
        });
    }

    public override ReadOnlySpan<byte> GetData()
    {
        return MemoryMarshal.AsBytes(CollectionsMarshal.AsSpan(this.vertices));
    }

    private struct TextureVertex
    {
        public Vector2 position;
        public Vector2 textureCoordinate;
    }
}
