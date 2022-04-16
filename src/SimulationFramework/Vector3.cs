using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

/// <summary>
/// A 3-component floating point vector.
/// </summary>
public unsafe struct Vector3 : IEquatable<Vector3>
{
    private const int ELEMENT_COUNT = 3;

    private fixed float values[ELEMENT_COUNT];

    /// <summary>
    /// A <see cref="Vector3"/> with all its components set to zero.
    /// </summary>
    public static readonly Vector3 Zero = (0, 0, 0);

    /// <summary>
    /// A <see cref="Vector3"/> with all its components set to one.
    /// </summary>
    public static readonly Vector3 One = (1, 1, 1);

    /// <summary>
    /// A unit vector in the X positve direction.
    /// </summary>
    public static readonly Vector3 UnitX = (1, 0, 0);

    /// <summary>
    /// A unit vector in the Y positve direction.
    /// </summary>
    public static readonly Vector3 UnitY = (0, 1, 0);

    /// <summary>
    /// A unit vector in the Z positve direction.
    /// </summary>
    public static readonly Vector3 UnitZ = (0, 0, 1);

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
    /// Creates a new <see cref="Vector3"/> with each component set to the same value.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3(float value)
    {
        X = Y = Z = value;
    }

    /// <summary>
    /// Creates a new <see cref="Vector3"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3(float x, float y, float z)
    {
        this.X = x;
        this.Y = y;
        this.Z = z;
    }
    /// <summary>
    /// Creates a new <see cref="Vector3"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3(Vector2 value, float z)
    {
        this.X = value.X;
        this.Y = value.Y;
        this.Z = z;
    }

    /// <summary>
    /// Creates a new <see cref="Vector3"/>.
    /// </summary>
    /// <param name="values">A collection of values to be the vector's inital components. Must have a length of 3.</param>
    /// <exception cref="ArgumentException"></exception>
    public Vector3(Span<float> values)
    {
        if (values.Length != 3)
            throw new ArgumentException($"{nameof(values)} must have {ELEMENT_COUNT} elements.", nameof(values));

        for (int i = 0; i < ELEMENT_COUNT; i++)
        {
            this.values[i] = values[i];
        }
    }

    /// <summary>
    /// Returns a unit <see cref="Vector3"/> pointing in the same direction as this one.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3 Normalized()
    {
        var invLength = 1f / Length();

        if (invLength == float.PositiveInfinity)
            return Zero;

        return (X * invLength, Y * invLength, Z * invLength);
    }

    /// <summary>
    /// Returns the length of this <see cref="Vector4"/> squared.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float LengthSquared()
    {
        return Dot(this, this);
    }

    /// <summary>
    /// Returns the dot product of two <see cref="Vector3"/> instances.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Dot(Vector3 a, Vector3 b)
    {
        return (a.X * b.X) + (a.Y * b.Y) + (a.Z + b.Z);
    }

    /// <summary>
    /// Returns the length of the <see cref="Vector3"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Length()
    {
        return MathF.Sqrt(LengthSquared());
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z);
    }

    /// <summary>
    /// Returns a string containing the nicely formatted values of this <see cref="Vector3"/>.
    /// </summary>
    public override string ToString()
    {
        return $"{{{X}, {Y}, {Z}}}";
    }

    /// <summary>
    /// Returns <see langword="true"/>if this <see cref="Vector3"/> is equal to another.
    /// </summary>
    public bool Equals(Vector3 other)
    {
        return this.X == other.X && this.Y == other.Y && this.Z == other.Z;
    }

    /// <inheritdoc/>
    public override bool Equals(object obj)
    {
        return obj is Vector3 vec && this.Equals(vec);
    }

    /// <summary>
    /// Returns a reference to the element at the specified index into a <see cref="Vector3"/>. 
    /// </summary>
    public ref float this[int index] => ref values[index];

    /// <summary>
    /// Casts from a <see cref="Vector3"/> to a tuple of 3 floats.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator (float, float, float)(Vector3 vector) => Unsafe.As<Vector3, (float, float, float)>(ref vector);
    /// <summary>
    /// Casts from a tuple of 3 floats to a <see cref="Vector3"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector3((float, float, float) vector) => Unsafe.As<(float, float, float), Vector3>(ref vector);

    /// <summary>
    /// Casts from a <see cref="Vector3"/> to a <see cref="System.Numerics.Vector3"/>.
    /// </summary>
    /// <param name="vector"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator System.Numerics.Vector3(Vector3 vector) => Unsafe.As<Vector3, System.Numerics.Vector3>(ref vector);

    /// <summary>
    /// Casts from a <see cref="System.Numerics.Vector3"/> to a <see cref="Vector3"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector3(System.Numerics.Vector3 vector) => Unsafe.As<System.Numerics.Vector3, Vector3>(ref vector);

    /// <summary>
    /// Checks if two <see cref="Vector3"/> objects are equal.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Vector3 left, Vector3 right) => left.Equals(right);

    /// <summary>
    /// Checks if two <see cref="Vector3"/> objects are not equal.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Vector3 left, Vector3 right) => !(left == right);

    /// <summary>
    /// Performs per-component addition between two <see cref="Vector3"/> instances.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator +(Vector3 left, Vector3 right) => new(left.X + right.X, left.Y + right.Y, left.Z + right.Z);

    /// <summary>
    /// Performs per-component addition between two <see cref="Vector3"/> instances.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator -(Vector3 left, Vector3 right) => new(left.X - right.X, left.Y - right.Y, left.Z - right.Z);

    /// <summary>
    /// Negates each component of a <see cref="Vector3"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator -(Vector3 value) => new(-value.X, -value.Y, -value.Z);

    /// <summary>
    /// Multiplies each component in a <see cref="Vector3"/> by a scalar value.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator *(Vector3 left, float right) => new(left.X * right, left.Y * right, left.Z * right);

    /// <summary>
    /// Multiplies each component in a <see cref="Vector3"/> by a scalar value.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator *(float left, Vector3 right) => right * left;

    /// <summary>
    /// Performs per-component multiplication between two <see cref="Vector3"/> instances.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator *(Vector3 left, Vector3 right) => new(left.X * right.X, left.Y * right.Y, left.Z * right.Z);

    /// <summary>
    /// Divides each component of a <see cref="Vector4"/> by a scalar value.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator /(float left, Vector3 right) => (1f / left) * right;

    /// <summary>
    /// Divides each component of a <see cref="Vector4"/> by a scalar value.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator /(Vector3 left, float right) => left * (1f / right);

    /// <summary>
    /// Performs per-component division between two <see cref="Vector3"/> instances.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator /(Vector3 left, Vector3 right) => (1f / left) * right;
}