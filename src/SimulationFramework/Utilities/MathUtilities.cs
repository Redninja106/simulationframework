using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Utilities;

/// <summary>
/// Contains various useful math-related methods.
/// </summary>
public static class MathUtilities
{
    /// <summary>
    /// Finds the bounding box of a polygon.
    /// </summary>
    /// <param name="polygon">The polygon to find the bounds of.</param>
    public static Rectangle PolygonBounds(Span<Vector2> polygon)
    {
        float minX = float.PositiveInfinity, minY = float.PositiveInfinity, maxX = float.NegativeInfinity, maxY = float.NegativeInfinity;

        for (int i = 0; i < polygon.Length; i++)
        {
            if (polygon[i].X < minX)
            {
                minX = polygon[i].X;
            }
            if (polygon[i].X > maxX)
            {
                maxX = polygon[i].X;
            }
            if (polygon[i].Y < minY)
            {
                minY = polygon[i].Y;
            }
            if (polygon[i].Y > maxY)
            {
                maxY = polygon[i].Y;
            }
        }

        return new Rectangle(minX, minY, maxX - minX, maxY - minY);
    }

    public static float Lerp(float a, float b, float t)
    {
        return a + (b - a) * t;
    }
}
