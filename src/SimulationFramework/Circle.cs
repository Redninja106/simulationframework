using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

/// <summary>
/// 
/// </summary>
public struct Circle
{
    public Vector2 Position;
    public float Radius;

    public Circle(float x, float y, float radius, Alignment alignment = Alignment.Center) : this(new(x, y), radius)
    {
    }

    public Circle(Vector2 position, float radius, Alignment alignment = Alignment.Center)
    {
        Position = position;
        Radius = radius;
    }


    public float Area()
    {
        return MathF.PI * Radius * Radius;
    }

    /// <summary>
    /// Gets the point on this circle at the provided angle.
    /// </summary>
    public Vector2 GetPoint(float angle)
    {
        return Position + new Vector2(MathF.Cos(angle) * Radius, MathF.Sin(angle) * Radius);
    }
}
