using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

public static class Angle
{
    public const float RadiansToDegrees = 180f / MathF.PI;
    public const float DegreesToRadians = MathF.PI / 180f;

    public static float ToDegrees(float radians)
    {
        return radians * RadiansToDegrees;
    }

    public static float ToRadians(float degrees)
    {
        return degrees * DegreesToRadians;
    }

    public static Vector2 ToVector(float radians)
    {
        var (sin, cos) = MathF.SinCos(radians);
        return new Vector2(cos, sin);
    }

    public static float FromVector(Vector2 vector)
    {
        float angle = MathF.Atan2(vector.Y, vector.X);
        return Normalize(angle);
    }

    /// <summary>
    /// Normalizes an angle to the range [0, 2pi).
    /// </summary>
    public static float Normalize(float radians)
    {
        radians %= MathF.Tau;

        if (radians < 0)
            radians += MathF.Tau;
        
        return radians;
    }

    public static float LerpClamped(float a, float b, float t)
    {
        return Lerp(a, b, MathHelper.Normalize(t));
    }

    public static float Lerp(float a, float b, float t)
    {
        a = Normalize(a);
        b = Normalize(b);

        return a - t * SignedDistance(a, b);
    }

    public static float Step(float a, float b, float s)
    {
        float distance = SignedDistance(a, b);

        if (distance <= s)
            return b;

        return a + s * MathF.Sign(distance);
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