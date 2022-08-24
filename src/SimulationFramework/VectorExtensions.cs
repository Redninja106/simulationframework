using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

/// <summary>
/// Provides small utility extensions onto the System.Numerics Vector types.
/// </summary>
public static class VectorExtensions
{
    /// <summary>
    /// Returns this vector with a length of 1.
    /// </summary>
    public static Vector2 Normalized(this Vector2 vector)
    {
        float length = vector.Length();

        if (length is 0.0f)
            return Vector2.Zero;

        float invLength = 1.0f / length;

        return new(vector.X * invLength, vector.Y * invLength);
    }
    
/// <summary>
    /// Returns this vector with a length of 1.
    /// </summary>
    public static Vector3 Normalized(this Vector3 vector)
    {
        float length = vector.Length();

        if (length is 0.0f)
            return Vector3.Zero;

        float invLength = 1.0f / length;

        return new(vector.X * invLength, vector.Y * invLength, vector.Z * invLength);
    }
    
    /// <summary>
    /// Returns this vector with a length of 1.
    /// </summary>
    public static Vector4 Normalized(this Vector4 vector)
    {
        float length = vector.Length();

        if (length is 0.0f)
            return Vector4.Zero;

        float invLength = 1.0f / length;

        return new(vector.X * invLength, vector.Y * invLength, vector.Z * invLength, vector.W * invLength);
    }

    public static void Deconstruct(this Vector2 vector, out float x, out float y)
    {
        x = vector.X;
        y = vector.Y;
    }

    public static void Deconstruct(this Vector3 vector, out float x, out float y, out float z)
    {
        x = vector.X;
        y = vector.Y;
        z = vector.Z;
    }

    public static void Deconstruct(this Vector4 vector, out float x, out float y, out float z, out float w)
    {
        x = vector.X;
        y = vector.Y;
        z = vector.Z;
        w = vector.W;
    }
}