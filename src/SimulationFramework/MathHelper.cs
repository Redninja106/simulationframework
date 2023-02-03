using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

/// <summary>
/// Contains various useful math-related methods.
/// </summary>
public static class MathHelper
{
    /// <summary>
    /// Finds the bounding box of a polygon.
    /// </summary>
    /// <param name="polygon">The polygon to find the bounds of.</param>
    public static unsafe Rectangle PolygonBounds(Span<Vector2> polygon)
    {
        fixed (Vector2* polygonPtr = &polygon[0])
        {
            return PolygonBounds(CollectionsHelper.AsEnumerableUnsafe(polygonPtr, polygon.Length));
        }
    }

    /// <summary>
    /// Finds the bounding box of a polygon.
    /// </summary>
    /// <param name="polygon">The polygon to find the bounds of.</param>
    public static Rectangle PolygonBounds(IEnumerable<Vector2> polygon)
    {
        float minX = float.PositiveInfinity, minY = float.PositiveInfinity, maxX = float.NegativeInfinity, maxY = float.NegativeInfinity;

        foreach (var point in polygon)
        {
            maxX = MathF.Max(maxX, point.X);
            maxY = MathF.Max(maxY, point.Y);
            minX = MathF.Min(minX, point.X);
            minY = MathF.Min(minY, point.Y);
        }

        return new Rectangle(minX, minY, maxX - minX, maxY - minY);
    }

    /// <summary>
    /// Linearly interpolates between two values.
    /// </summary>
    /// <param name="a">The first value.</param>
    /// <param name="b">The second value.</param>
    /// <param name="t">The interpolation amount, from 0.0 to 1.0.</param>
    /// <returns>The interpolated value.</returns>
    public static float Lerp(float a, float b, float t)
    {
        return a + (b - a) * t;
    }

    /// <summary>
    /// Clamps a float to range [0.0, 1.0]
    /// </summary>
    /// <param name="value">The value to normalize.</param>
    /// <returns>The normalized value.</returns>
    public static float Normalize(float value)
    {
        return Math.Clamp(value, 0.0f, 1.0f);
    }

    /// <summary>
    /// Converts from radians to degress.
    /// </summary>
    /// <param name="radians">The angle to convert.</param>
    /// <returns>The converted angle, in degrees.</returns>
    public static float RadiansToDegrees(float radians)
    {
        return radians * (360f / MathF.Tau);
    }

    /// <summary>
    /// Converts from degrees to radians.
    /// </summary>
    /// <param name="degrees">The angle to convert.</param>
    /// <returns>The converted angle, in radians.</returns>
    public static float DegreesToRadians(float degrees)
    {
        return degrees * (MathF.Tau / 360f);
    }

    /// <summary>
    /// Returns the given angle to its smallest positive equivalent.
    /// </summary>
    /// <param name="degrees">The angle to normalize.</param>
    /// <returns>The normalized angle, between 0 (inclusive) and 360 (exclusive).</returns>
    public static float NormalizeDegrees(float degrees)
    {
        degrees %= 360;

        if (degrees < 0)
            degrees++;

        return degrees;
    }

    /// <summary>
    /// Normalizes the given angle to its smallest positive equivalent.
    /// </summary>
    /// <param name="radians">The angle to normalize.</param>
    /// <returns>The normalized angle, between 0 (inclusive) and 2pi (exclusive).</returns>
    public static float NormalizeRadians(float radians)
    {
        radians %= MathF.Tau;

        if (radians < 0)
            radians += MathF.Tau;

        return radians;
    }
}
