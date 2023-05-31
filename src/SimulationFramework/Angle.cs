using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

/// <summary>
/// Provides utilities for working with angles.
/// </summary>
public static class Angle
{
    /// <summary>
    /// The ratio of one radian to one degree. Multiplying a value by this constant converts it from radians to degrees.
    /// </summary>
    public const float RadiansToDegrees = 180f / MathF.PI;

    /// <summary>
    /// The ratio of one degree to one radian. Multiplying a value by this constant converts it from degrees to radians.
    /// </summary>
    public const float DegreesToRadians = MathF.PI / 180f;

    /// <summary>
    /// Converts a value from radians to degrees.
    /// </summary>
    /// <param name="radians">The value to convert to degrees.</param>
    /// <returns>The provided value converted to degrees.</returns>
    public static float ToDegrees(float radians)
    {
        return radians * RadiansToDegrees;
    }

    /// <summary>
    /// Converts a value from degrees to radians.
    /// </summary>
    /// <param name="degrees">The value to convert to radians.</param>
    /// <returns>The provided value converted to radians.</returns>
    public static float ToRadians(float degrees)
    {
        return degrees * DegreesToRadians;
    }

    /// <summary>
    /// Converts the provided angle to a unit vector..
    /// </summary>
    /// <param name="radians"></param>
    /// <returns></returns>
    public static Vector2 ToVector(float radians)
    {
        var (sin, cos) = MathF.SinCos(radians);
        return new Vector2(cos, sin);
    }

    /// <summary>
    /// Finds the angle of the provided vector
    /// </summary>
    /// <param name="vector"></param>
    /// <returns>The normalized angle of the vector.</returns>
    public static float FromVector(Vector2 vector)
    {
        float angle = MathF.Atan2(vector.Y, vector.X);
        return Normalize(angle);
    }

    /// <summary>
    /// Finds the smallest positive angle which is equivalent to the given angle.
    /// </summary>
    /// <param name="radians">The angle to normalize, in radians.</param>
    /// <returns>The normalized angle. This value will within the range <c>[0, 2pi)</c></returns>
    public static float Normalize(float radians)
    {
        radians %= MathF.Tau;

        if (radians < 0)
            radians += MathF.Tau;
        
        return radians;
    }

    /// <summary>
    /// Linearly interpolates from one angle towards another along the unit circle.
    /// </summary>
    /// <param name="a">The angle to interpolate from.</param>
    /// <param name="b">The angle to interpolate to.</param>
    /// <param name="t">The interpolation weight. A value of 0 returns <paramref name="a"/>, and a value of 1 returns <paramref name="b"/>.</param>
    /// <returns>The value between <paramref name="a"/> and <paramref name="b"/> according to <paramref name="t"/>.</returns>
    public static float LerpClamped(float a, float b, float t)
    {
        return Lerp(a, b, MathHelper.Normalize(t));
    }

    /// <summary>
    /// Linearly interpolates from one angle towards another along the unit circle.
    /// </summary>
    /// <param name="a">The angle to interpolate from.</param>
    /// <param name="b">The angle to interpolate to.</param>
    /// <param name="t">The interpolation weight. A value of 0 returns <paramref name="a"/>, and a value of 1 returns <paramref name="b"/>.</param>
    /// <returns>The value between <paramref name="a"/> and <paramref name="b"/> according to <paramref name="t"/>.</returns>
    public static float Lerp(float a, float b, float t)
    {
        a = Normalize(a);
        b = Normalize(b);

        return a - t * SignedDistance(a, b);
    }

    /// <summary>
    /// Steps one angle towards another along the unit circle.
    /// </summary>
    /// <param name="a">The angle to step from, in radians.</param>
    /// <param name="b">The angle to step towards, in radians.</param>
    /// <param name="s">The amount to step by, in radians.</param>
    /// <returns>
    /// <c><paramref name="a"/>-<paramref name="s"/></c> or <c><paramref name="a"/>+<paramref name="s"/></c>, whichever is closer to <paramref name="b"/> on the unit circle.
    /// If the distance between <paramref name="a"/> and <paramref name="b"/> is less than <paramref name="s"/>, than <paramref name="b"/> is returned.
    /// </returns>
    /// <remarks>
    /// </remarks>
    public static float Step(float a, float b, float s)
    {
        float distance = Distance(a, b);

        if (distance <= s)
            return b;

        return Lerp(a, b, s / distance);
    }

    /// <summary>
    /// Finds the distance between two angles along the unit circle.
    /// </summary>
    public static float Distance(float a, float b)
    {
        return MathF.Abs(SignedDistance(a, b));
    }

    internal static float SignedDistance(float a, float b)
    {
        a = Normalize(a);
        b = Normalize(b);

        float d0 = a - b;
        float d1 = d0 + MathF.Tau;
        float d2 = d0 - MathF.Tau;

        return MathF.MinMagnitude(d0, MathF.MinMagnitude(d1, d2));
    }
}