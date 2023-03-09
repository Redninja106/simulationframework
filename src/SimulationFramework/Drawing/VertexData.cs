using SimulationFramework.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing;


public interface IVertex
{
}

struct MyVertex : IVertex
{
    public Vector3 Position;
    public Vector2 UV;
    public float custom;

    public void Populate(VertexDataEntry entry)
    {
        entry.AddAttribute(Position, VertexAttributeKind.Position);
        entry.AddAttribute(UV, VertexAttributeKind.TextureCoordinate);
        entry.AddAttribute(custom, VertexAttributeKind.Unknown);
    }
}


public enum VertexAttributeKind 
{
    Position,
    TextureCoordinate,
    Color,
    Normal,
    Tangent,
    Bitangent,
    AnimationWeight,
    Unknown,
}

public class VertexData<T> where T : unmanaged
{
    private VertexData(PrimitiveKind kind)
    {
        throw new NotImplementedException();
    }

    public void ComputeNormals()
    {
        throw new NotImplementedException();
    }

    public void ComputeTangents()
    {
        throw new NotImplementedException();
    }

    public VertexDataEntry AddVertex()
    {
        throw new NotImplementedException();
    }

    VertexDataEntry GetVertex(int index)
    {
        throw new NotImplementedException();
    }
}

public ref struct VertexDataEntry
{
    private VertexData<int> vertexData;
    public int Index;

    public void AddAttribute<T>(T value, VertexAttributeKind attributeKind) where T : unmanaged
    {
    }

}