using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

public struct Vector3 : IEquatable<Vector3>
{
    public static readonly Vector3 Zero = (0, 0, 0);
    public static readonly Vector3 One = (1, 1, 1);
    public static readonly Vector3 UnitY = (0, 1, 0);
    public static readonly Vector3 UnitX = (1, 0, 0);
    public static readonly Vector3 UnitZ = (0, 0, 1);

    public float X;
    public float Y;
    public float Z;

    public Vector3(float value)
    {
        X = Y = Z = value;
    }

    public Vector3(float x, float y, float z)
    {
        this.X = x;
        this.Y = y;
        this.Z = z;
    }

    public readonly Vector3 Normalized()
    {
        var l = 1f / Length();
        return (X * l, Y * l, Z * l);
    }

    public readonly float LengthSquared()
    {
        return X * X + Y * Y + Z * Z;
    }

    public readonly float Length()
    {
        return MathF.Sqrt(LengthSquared());
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z);
    }

    public override string ToString()
    {
        return $"{{{X}, {Y}, {Z}}}";
    }

    public bool Equals(Vector3 other)
    {
        return this.X == other.X && this.Y == other.Y && this.Z == other.Z;
    }

    public override bool Equals(object obj)
    {
        if (obj is Vector3 vec)
            return Equals(vec);
        return false;
    }

    public static implicit operator (float, float, float)(Vector3 vector) => (vector.X, vector.Y, vector.Z);
    public static implicit operator Vector3((float, float, float) values) => new(values.Item1, values.Item2, values.Item3);
    public static implicit operator System.Numerics.Vector3(Vector3 vector) => new(vector.X, vector.Y, vector.Z);
    public static implicit operator Vector3(System.Numerics.Vector3 vector) => new(vector.X, vector.Y, vector.Z);
    public static bool operator ==(Vector3 left, Vector3 right) => left.Equals(right);
    public static bool operator !=(Vector3 left, Vector3 right) => !(left == right);
    public static Vector3 operator +(Vector3 left, Vector3 right) => new(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
    public static Vector3 operator -(Vector3 left, Vector3 right) => new(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
    public static Vector3 operator -(Vector3 value) => new(-value.X, -value.Y, -value.Z);
    public static Vector3 operator *(Vector3 left, float right) => new(left.X * right, left.Y * right, left.Z * right);
    public static Vector3 operator *(float left, Vector3 right) => right * left;
}