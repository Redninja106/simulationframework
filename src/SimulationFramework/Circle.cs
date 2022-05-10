using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

public struct Circle
{
    public Vector2 Position;
    public float Radius;

    public Circle(Vector2 position, float radius)
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
