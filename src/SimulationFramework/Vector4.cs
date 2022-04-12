using System;
using System.Runtime.CompilerServices;

namespace SimulationFramework;

/// <summary>
/// A 4-component floating point vector.
/// </summary>
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

    /// <summary>
    /// Creates a new <see cref="Vector4"/> with all components set to the same value.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector4(float value)
    {
        X = Y = Z = W = value;
    }

    /// <summary>
    /// Creates a new <see cref="Vector4"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector4(float x, float y, float z, float w)
    {
        this.X = x;
        this.Y = y;
        this.Z = z;
        this.W = w;
    }

    /// <summary>
    /// Creates a new <see cref="Vector4"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector4(Vector3 xyz, float w)
    {
        this.X = xyz.X;
        this.Y = xyz.Y;
        this.Z = xyz.Z;
        this.W = w;
    }

    /// <summary>
    /// Creates a new <see cref="Vector4"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector4(Vector2 xy, float z, float w)
    {
        this.X = xy.X;
        this.Y = xy.Y;
        this.Z = z;
        this.W = w;
    }

    /// <summary>
    /// Creates a new  <see cref="Vector4"/>.
    /// </summary>
    /// <param name="values">A collection of values to be the vector's inital components. Must have a length of 4.</param>
    /// <exception cref="ArgumentException"></exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector4(Span<float> values)
    {
        if (values.Length != ELEMENT_COUNT)
            throw new ArgumentException($"{nameof(values)} must have {ELEMENT_COUNT} elements.", nameof(values));

        for (int i = 0; i < ELEMENT_COUNT; i++)
        {
            this.values[i] = values[i];
        }
    }

    /// <summary>
    /// Returns a unit <see cref="Vector4"/> pointing in the same direction as this one.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector4 Normalized()
    {
        var invLength = 1f / Length();

        if (invLength == float.PositiveInfinity)
            return Zero;

        return (X * invLength, Y * invLength, Z * invLength, W * invLength);
    }

    /// <summary>
    /// Returns the length of this <see cref="Vector4"/> squared.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float LengthSquared()
    {
        return Dot(this, this);
    }

    /// <summary>
    /// Returns the length of the <see cref="Vector4"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Length()
    {
        return MathF.Sqrt(LengthSquared());
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z, W);
    }

    /// <summary>
    /// Returns a <see cref="Vector3"/> containing this vector's X, Y, and Z components.
    /// </summary>
    /// <returns></returns>
    public Vector3 DropW()
    {
        return new Vector3(X, Y, Z);
    }
    
    /// <summary>
    /// Returns the dot product of two <see cref="Vector4"/> instances.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Dot(Vector4 a, Vector4 b)
    {
        return (a.X * b.X) + (a.Y * b.Y) + (a.Z * b.Z) + (a.W * b.W);
    }

    /// <summary>
    /// Returns a string containing the nicely formatted values of this <see cref="Vector4"/>.
    /// </summary>
    public override string ToString()
    {
        return $"{{{X}, {Y}, {Z}, {W}}}";
    }

    /// <summary>
    /// Returns <see langword="true"/>if this <see cref="Vector4"/> is equal to another.
    /// </summary>
    public bool Equals(Vector4 other)
    {
        return this.X == other.X && this.Y == other.Y && this.Z == other.Z && this.W == other.W;
    }

    /// <inheritdoc/>
    public override bool Equals(object obj)
    {
        return obj is Vector4 vec && Equals(vec);
    }

    /// <summary>
    /// Returns a reference to the element at the specified index into a <see cref="Vector4"/>. 
    /// </summary>
    public ref float this[int index] => ref this.values[index];

    /// <summary>
    /// Casts from a <see cref="Vector4"/> to a tuple of 4 floats.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator (float, float, float, float)(Vector4 vector) => Unsafe.As<Vector4, (float, float, float, float)>(ref vector);
    
    /// <summary>
    /// Casts from a tuple of 4 floats to a <see cref="Vector4"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector4((float, float, float, float) vector) => Unsafe.As<(float, float, float, float), Vector4>(ref vector);

    /// <summary>
    /// Casts from a <see cref="Vector4"/> to a <see cref="System.Numerics.Vector4"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator System.Numerics.Vector4(Vector4 vector) => Unsafe.As<Vector4, System.Numerics.Vector4>(ref vector);

    /// <summary>
    /// Casts from a <see cref="System.Numerics.Vector4"/> to a <see cref="Vector4"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector4(System.Numerics.Vector4 vector) => Unsafe.As<System.Numerics.Vector4, Vector4>(ref vector);

    /// <summary>
    /// Checks if two vectors are equal.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Vector4 left, Vector4 right) => left.Equals(right);
    
    /// <summary>
    /// Checks if two vectors are not equal.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Vector4 left, Vector4 right) => !(left == right);
    
    /// <summary>
    /// Performs per-component addition between two <see cref="Vector4"/> instances.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator +(Vector4 left, Vector4 right) => new(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);
    
    /// <summary>
    /// Performs per-component subtraction between two <see cref="Vector4"/> instances.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator -(Vector4 left, Vector4 right) => new(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W + right.W);
    
    /// <summary>
    /// Negates each component of a <see cref="Vector4"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator -(Vector4 value) => new(-value.X, -value.Y, -value.Z, -value.W);

    /// <summary>
    /// Multiplies each component in a <see cref="Vector4"/> by a scalar value.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator *(Vector4 left, float right) => new(left.X * right, left.Y * right, left.Z * right, left.W * right);

    /// <summary>
    /// Multiplies each component in a <see cref="Vector4"/> by a scalar value.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator *(float left, Vector4 right) => right * left;

    /// <summary>
    /// Performs per-component multiplication between two <see cref="Vector4"/> instances.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator *(Vector4 left, Vector4 right) => new(left.X * right.X, left.Y * right.Y, left.Z * right.Z, left.W * right.W);
    
    /// <summary>
    /// Divides each component of a <see cref="Vector4"/> by a scalar value.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator /(Vector4 left, float right) => left * (1f / right);

    /// <summary>
    /// Divides each component of a <see cref="Vector4"/> by a scalar value.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator /(float left, Vector4 right) => right / left;

    /// <summary>
    /// Performs per-component division between two <see cref="Vector4"/> instances.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator /(Vector4 left, Vector4 right) => right * (1f / left);
}