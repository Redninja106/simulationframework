using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

public struct Vector2 : IEquatable<Vector2>
{
    public static readonly Vector2 Zero = new(0, 0);
    public static readonly Vector2 Up = new(0,-1);
    public static readonly Vector2 Down = new(0, 1);
    public static readonly Vector2 Left = new(-1, 0);
    public static readonly Vector2 Right = new(1, 0);

    public float X;
    public float Y;

    public Vector2(float x, float y)
    {
        this.X = x;
        this.Y = y;
    }

    public readonly Vector2 Normalized()
    {
        return new Vector2(X, Y);
    }

    public readonly float LengthSquared()
    {
        return X * X + Y * Y;
    }

    public readonly float Length()
    {
        return MathF.Sqrt(LengthSquared());
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

    public static implicit operator (float, float)(Vector2 vector) => (vector.X, vector.Y);
    public static implicit operator Vector2((float, float) values) => new(values.Item1, values.Item2);
    public static implicit operator System.Numerics.Vector2(Vector2 vector) => new(vector.X, vector.Y);
    public static implicit operator Vector2(System.Numerics.Vector2 vector) => new(vector.X, vector.Y);
    public static bool operator ==(Vector2 left, Vector2 right) => left.Equals(right);
    public static bool operator !=(Vector2 left, Vector2 right) => !(left == right);
    public static Vector2 operator +(Vector2 left, Vector2 right) => new(left.X + right.X, left.Y + right.X);
    public static Vector2 operator -(Vector2 left, Vector2 right) => new(left.X - right.X, left.Y - right.Y);
    public static Vector2 operator -(Vector2 value) => new(-value.X, -value.Y);
    public static Vector2 operator *(Vector2 left, float right) =>new(left.X * right, left.Y * right);
}