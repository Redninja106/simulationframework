using SimulationFramework.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing;

public interface IVertex 
{
    public Vector3 Position { get; }
    public Vector2 TextureCoordinates { get; }
}

[Flags]
public enum VertexAttributeFlags 
{
    None = 0,
    Position = 1 >> 1,
    TextureCoordinate = 1 >> 2,

}

public class VertexData<TVertex> where TVertex : unmanaged, IVertex
{
    private readonly List<Vector3> positions;
    private readonly List<Vector3> textureCoordinates;

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

public record VertexDataEntry
{
    public int VertexIndex;
    public Vector3? Position;
    public Vector2? TextureCoordinate;
    public ColorF? Color;
    public Vector3? Normal;
    public Vector3? Tangent;
    public Vector3? Bitangent;
}

enum VertexAttributeKind
{
    Position,
    TextureCoordinates,
    Color,
    Normal,
    Tangent,
    Bitangent,
    AnimWeights
}
