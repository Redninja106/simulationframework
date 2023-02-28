using SimulationFramework;
using System.Numerics;

namespace Grass;
struct GrassBlade
{
    public Vector3 Position;
    public float Rotation;
    public float Height;

    public GrassBlade(Vector3 position, float rotation, float height)
    {
        this.Position = position;
        this.Rotation = rotation;
        this.Height = height;
    }

    public void WriteVertices(Span<GrassVertex> vertices, int offset)
    {
        const float baseSize = .01f;

        var rng = Random.Shared;

        Vector3 baseOffset = new Vector3(MathF.Cos(Rotation), 0, MathF.Sin(Rotation)) * (baseSize / 2f);

        vertices[offset + 0] = new(this.Position + baseOffset, rng.NextSingle(), new(Position.X, Position.Z), 0, this.Height);
        vertices[offset + 1] = new(this.Position - baseOffset, rng.NextSingle(), new(Position.X, Position.Z), 0, this.Height);
        vertices[offset + 2] = new(this.Position + new Vector3(0, Height, 0), rng.NextSingle(), new(Position.X, Position.Z), 1, this.Height);
    }
}
