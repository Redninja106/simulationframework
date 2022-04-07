using System;
using System.Runtime.CompilerServices;

namespace SimulationFramework;

public unsafe struct Vector4 : IEquatable<Vector4>
{
    private const int ELEMENT_COUNT = 4;

    // this is the vector's data.
    private fixed float values[ELEMENT_COUNT];

    /// <summary>
    /// A vector with all of its components set to zero.
    /// </summary>
    public static readonly Vector4 Zero = (0, 0, 0, 0);

    /// <summary>
    /// A vector with all of its components set to one.
    /// </summary>
    public static readonly Vector4 One = (1, 1, 1, 1);

    /// <summary>
    /// A unit vector pointing in the positive X direction.
    /// </summary>
    public static readonly Vector4 UnitX = (1, 0, 0, 0);
    
    /// <summary>
    /// A unit vector pointing in the positive Y direction.
    /// </summary>
    public static readonly Vector4 UnitY = (0, 1, 0, 0);
    
    /// <summary>
    /// A unit vector pointing in the positive Z direction.
    /// </summary>
    public static readonly Vector4 UnitZ = (0, 0, 1, 0);
    
    /// <summary>
    /// A unit vector pointing in the positive W direction.
    /// </summary>
    public static readonly Vector4 UnitW = (0, 0, 0, 1);

    /// <summary>
    /// The X component of the vector.
    /// </summary>
    public ref float X => ref values[0];

    /// <summary>
    /// The Y component of the vector.
    /// </summary>
    public ref float Y => ref values[1];
    
    /// <summary>
    /// The Z component of the vector.
    /// </summary>
    public ref float Z => ref values[2];

    /// <summary>
    /// The W component of the vector.
    /// </summary>
    public ref float W => ref values[3];

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

    public Vector4(Vector3 xyz, float w)
    {
        this.X = xyz.X;
        this.Y = xyz.Y;
        this.Z = xyz.Z;
        this.W = w;
    }

    public Vector4(Vector2 xy, float z, float w)
    {
        this.X = xy.X;
        this.Y = xy.Y;
        this.Z = z;
        this.W = w;
    }

    public Vector4(Span<float> values)
    {
        if (values.Length != ELEMENT_COUNT)
            throw new ArgumentException("values must have a length of 4!", nameof(values));

        for (int i = 0; i < ELEMENT_COUNT; i++)
        {
            this.values[i] = values[i];
        }
    }

    /// <summary>
    /// Returns this vector with a length of 1 (but pointing in the same direction).
    /// </summary>
    public Vector4 Normalized()
    {
        var l = 1f / Length();
        return (X * l, Y * l, Z * l, W * l);
    }
    
    public float LengthSquared()
    {
        return Dot(this, this);
    }

    public float Length()
    {
        return MathF.Sqrt(LengthSquared());
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z, W);
    }

    public static float Dot(Vector4 a, Vector4 b)
    {
        return (a.X * b.X) + (a.Y * b.Y) + (a.Z * b.Z) + (a.W * b.W);
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
    
    public ref float this[int index] => ref this.values[index];
    
    public static implicit operator (float, float, float, float)(Vector4 vector) => (vector.X, vector.Y, vector.Z, vector.W);
    public static implicit operator Vector4((float, float, float, float) values) => new(values.Item1, values.Item2, values.Item3, values.Item4);
    public static implicit operator System.Numerics.Vector4(Vector4 vector) => new(vector.X, vector.Y, vector.Z, vector.W);
    public static implicit operator Vector4(System.Numerics.Vector4 vector) => new(vector.X, vector.Y, vector.Z, vector.W);
    public static bool operator ==(Vector4 left, Vector4 right) => left.Equals(right);
    public static bool operator !=(Vector4 left, Vector4 right) => !(left == right);
    public static Vector4 operator +(Vector4 left, Vector4 right) => new(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);
    public static Vector4 operator -(Vector4 left, Vector4 right) => new(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W + right.W);
    public static Vector4 operator -(Vector4 value) => new(-value.X, -value.Y, -value.Z, -value.W);
    public static Vector4 operator *(Vector4 left, float right) => new(left.X * right, left.Y * right, left.Z * right, left.W * right);
    public static Vector4 operator *(float left, Vector4 right) => right * left;
    public static Vector4 operator /(Vector4 left, float right) => new(left.X / right, left.Y / right, left.Z / right, left.W / right);
    public static Vector4 operator /(float left, Vector4 right) => right * left;
}