using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;
public struct Box
{
    public float X;
    public float Y;
    public float Z;
    public float Width;
    public float Height;
    public float Depth;

    public Box(Vector3 position, Vector3 size) : this()
    {
        Position = position;
        Size = size;
    }

    public Box(float x, float y, float z, float width, float height, float depth)
    {
        this.X = x;
        this.Y = y;
        this.Z = z;
        this.Width = width;
        this.Height = height;
        this.Depth = depth;
    }

    public Vector3 Position
    {
        get => new(X, Y, Z);
        set => (X, Y, Z) = value;
    }

    public Vector3 Size
    {
        get => new(Width, Height, Depth);
        set => (Width, Height, Depth) = value;
    }

    public Vector3 Center
    {
        get => Position + .5f * Size;
        set => Position = value - .5f * Size;
    }

    public static Box FromMinMax(Vector3 min, Vector3 max)
    {
        min.X = MathF.Min(min.X, max.X);
        min.Y = MathF.Min(min.Y, max.Y);
        min.Z = MathF.Min(min.Z, max.Z);

        max.X = MathF.Max(min.X, max.X);
        max.Y = MathF.Max(min.Y, max.Y);
        max.Z = MathF.Max(min.Z, max.Z);

        return new(min, max - min);
    }
}
