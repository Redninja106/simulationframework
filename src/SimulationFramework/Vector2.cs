using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimulationFramework.Utilities;

namespace SimulationFramework;

// math types will be commented later
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public struct Vector2 : IEquatable<Vector2>
{
    public static readonly Vector2 Zero = (0, 0);
    public static readonly Vector2 One = (1, 1);
    public static readonly Vector2 UnitY = (0, 1);
    public static readonly Vector2 UnitX = (1, 0);

    public float X;
    public float Y;

    public Vector2(float value)
    {
        X = Y = value;
    }

    public Vector2(float x, float y)
    {
        this.X = x;
        this.Y = y;
    }

    public readonly Vector2 Normalized()
    {
        var l = 1f / Length();
        return (X * l, Y * l);
    }

    public readonly float LengthSquared()
    {
        return X * X + Y * Y;
    }

    public readonly float Length()
    {
        return MathF.Sqrt(LengthSquared());
    }

    public readonly void Deconstruct(out float x, out float y)
    {
        x = this.X;
        y = this.Y;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public override string ToString()
    {
        return $"{{{X}, {Y}}}";
    }

    public bool Equals(Vector2 other)
    {
        return this.X == other.X && this.Y == other.Y;
    }

    public override bool Equals(object obj)
    {
        if (obj is Vector2 vec)
            return Equals(vec);

        return false;
    }

    /// <summary>
    /// Creates a unit vector from the provided angle.
    /// </summary>
    public static Vector2 FromAngle(float angle)
    {
        return (MathF.Cos(angle), MathF.Sin(angle));
    }

    /// <summary>
    /// Transforms a point by the provided transformation.
    /// </summary>
    public static Vector2 Transform(Vector2 point, Matrix3x2 transform)
    {
        return (point.X * transform.M11 + point.Y * transform.M21 + transform.M31, point.X * transform.M12 + point.Y * transform.M22 + transform.M32);
    }

    /// <summary>
    /// Reflects a vector across a normal.
    /// </summary>
    public static Vector2 Reflect(Vector2 vector, Vector2 normal)
    {
        return vector - (2 * Dot(vector, normal) * normal);
    }

    /// <summary>
    /// Determines the dot product between two vectors.
    /// </summary>
    public static float Dot(Vector2 a, Vector2 b)
    {
        return a.X * b.X + a.Y * b.Y;
    }
    
    /// <summary>
    /// Determines the angle between two vectors.
    /// </summary>
    public static float AngleBetween(Vector2 a, Vector2 b)
    {
        return MathF.Acos(Dot(a, b) / a.Length() * b.Length());
    }

    /// <summary>
    /// Performs a linear interpolation between two vectors.
    /// </summary>
    public static Vector2 Lerp(Vector2 a, Vector2 b, float t)
    {
        return (MathUtilities.Lerp(a.X, b.X, t), MathUtilities.Lerp(a.Y, b.Y, t));
    }

    public static implicit operator (float, float)(Vector2 vector) => (vector.X, vector.Y);
    public static implicit operator Vector2((float, float) values) => new(values.Item1, values.Item2);
    public static implicit operator System.Numerics.Vector2(Vector2 vector) => new(vector.X, vector.Y);
    public static implicit operator Vector2(System.Numerics.Vector2 vector) => new(vector.X, vector.Y);
    public static bool operator ==(Vector2 left, Vector2 right) => left.Equals(right);
    public static bool operator !=(Vector2 left, Vector2 right) => !(left == right);
    public static Vector2 operator +(Vector2 left, Vector2 right) => new(left.X + right.X, left.Y + right.Y);
    public static Vector2 operator -(Vector2 left, Vector2 right) => new(left.X - right.X, left.Y - right.Y);
    public static Vector2 operator -(Vector2 value) => new(-value.X, -value.Y);
    public static Vector2 operator *(Vector2 left, float right) =>new(left.X * right, left.Y * right);
    public static Vector2 operator *(float left, Vector2 right) => right * left;
}