using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

public struct Vector4 : IEquatable<Vector4>
{
    public static readonly Vector4 Zero = (0, 0, 0, 0);
    public static readonly Vector4 One = (1, 1, 1, 1);
    public static readonly Vector4 UnitX = (1, 0, 0, 0);
    public static readonly Vector4 UnitY = (0, 1, 0, 0);
    public static readonly Vector4 UnitZ = (0, 0, 1, 0);
    public static readonly Vector4 UnitW = (0, 0, 0, 1);


    public float X;
    public float Y;
    public float Z;
    public float W;

    public Vector4(float value)
    {
        X = Y = Z = W = value;
    }

    public Vector4(float x, float y, float z, float w)
    {
        this.X = x;
        this.Y = y;
        this.Z = z;
        this.W = w;
    }

    public readonly Vector4 Normalized()
    {
        var l = 1f / Length();
        return (X * l, Y * l, Z * l, W * l);
    }

    public readonly float LengthSquared()
    {
        return X * X + Y * Y + Z * Z + W * W;
    }

    public readonly float Length()
    {
        return MathF.Sqrt(LengthSquared());
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z, W);
    }

    public override string ToString()
    {
        return $"{{{X}, {Y}, {Z}, {W}}}";
    }

    public bool Equals(Vector4 other)
    {
        return this.X == other.X && this.Y == other.Y && this.Z == other.Z && this.W == other.W;
    }

    public override bool Equals(object obj)
    {
        if (obj is Vector4 vec)
            return Equals(vec);
        return false;
    }

    public static implicit operator (float, float, float, float)(Vector4 vector) => (vector.X, vector.Y, vector.Z, vector.W);
    public static implicit operator Vector4((float, float, float, float) values) => new(values.Item1, values.Item2, values.Item3, values.Item4);
    public static implicit operator System.Numerics.Vector4(Vector4 vector) => new(vector.X, vector.Y, vector.Z, vector.W);
    public static implicit operator Vector4(System.Numerics.Vector4 vector) => new(vector.X, vector.Y, vector.Z, vector.W);
    public static bool operator ==(Vector4 left, Vector4 right) => left.Equals(right);
    public static bool operator !=(Vector4 left, Vector4 right) => !(left == right);
    public static Vector4 operator +(Vector4 left, Vector4 right) => new(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);
    public static Vector4 operator -(Vector4 left, Vector4 right) => new(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W + right.W);
    public static Vector4 operator -(Vector4 value) => new(-value.X, -value.Y, -value.Z, value.W);
    public static Vector4 operator *(Vector4 left, float right) => new(left.X * right, left.Y * right, left.Z * right, left.W * right);
    public static Vector4 operator *(float left, Vector4 right) => right * left;
}