using System.Numerics;

namespace SimulationFramework;

/// <summary>
/// Contains various useful math-related methods.
/// </summary>
public static class MathHelper
{
    /// <summary>
    /// Linearly interpolates between two values.
    /// </summary>
    /// <param name="a">The first value.</param>
    /// <param name="b">The second value.</param>
    /// <param name="t">The interpolation amount, from 0.0 to 1.0.</param>
    /// <returns>The interpolated value.</returns>
    [Obsolete("use float.Lerp instead")]
    public static float Lerp(float a, float b, float t)
    {
        return a + (b - a) * t;
    }

    /// <summary>
    /// Linearly interpolates between two values, clamping the result between the two values.
    /// </summary>
    /// <param name="a">The first value.</param>
    /// <param name="b">The second value.</param>
    /// <param name="t">The interpolation amount, from 0.0 to 1.0.</param>
    /// <returns>The interpolated value.</returns>
    [Obsolete("use float.Lerp instead")]
    public static float LerpClamped(float a, float b, float t)
    {
        return Lerp(a, b, Normalize(t));
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
    /// Steps one vector towards another by a specified distance.
    /// </summary>
    /// <param name="point">The point to step towards <paramref name="target"/>.</param>
    /// <param name="target">The target point.</param>
    /// <param name="distance">The distance to step <paramref name="point"/> towards <paramref name="target"/> by. Should not be negative. </param>
    /// <returns><paramref name="target"/> if the distance between <paramref name="point"/> and <paramref name="target"/> is less than or equal to <paramref name="distance"/>; otherwise, a vector <paramref name="distance"/> units closer to <paramref name="target"/> compared to <paramref name="point"/>.</returns>
    public static Vector2 Step(Vector2 point, Vector2 target, float distance)
    {
        var diff = target - point;
        if (diff.LengthSquared() <= distance * distance)
        {
            return target;
        }

        return point + distance * diff.Normalized();
    }

    /// <summary>
    /// Steps one value towards another by a specified distance.
    /// </summary>
    /// <param name="value">The value to step towards <paramref name="target"/>.</param>
    /// <param name="target">The target value.</param>
    /// <param name="distance">The distance to step <paramref name="value"/> towards <paramref name="target"/> by. Should not be negative. </param>
    /// <returns><paramref name="target"/> if the distance between <paramref name="value"/> and <paramref name="target"/> is less than or equal to <paramref name="distance"/>; otherwise, the value <paramref name="distance"/> units closer to <paramref name="target"/> compared to <paramref name="value"/>.</returns>
    public static float Step(float value, float target, float distance)
    {
        var diff = target - value;
        if (MathF.Abs(diff) <= distance)
        {
            return target;
        }

        return value + distance * MathF.Sign(diff);
    }

    /// <summary>
    /// Computes the 2D cross product of two vectors. The 2D cross product is the Z component of the 3D cross product of the vectors.
    /// </summary>
    public static float Cross(Vector2 a, Vector2 b)
    {
        return Vector3.Cross(new(a, 0), new(b, 0)).Z;
    }
}