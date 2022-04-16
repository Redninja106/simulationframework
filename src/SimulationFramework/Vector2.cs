using System;
using SimulationFramework.Utilities;

namespace SimulationFramework;

/// <summary>
/// A 2-component floating point vector.
/// </summary>
public unsafe struct Vector2 : IEquatable<Vector2>
{
    private const int ELEMENT_COUNT = 2;

    private fixed float values[ELEMENT_COUNT];

    /// <summary>
    /// A <see cref="Vector2"/> with all its components set to zero.
    /// </summary>
    public static readonly Vector2 Zero = (0, 0);

    /// <summary>
    /// A <see cref="Vector2"/> with all of its components set to one.
    /// </summary>
    public static readonly Vector2 One = (1, 1);

    /// <summary>
    /// A unit <see cref="Vector2"/> pointing in the positive X direction.
    /// </summary>
    public static readonly Vector2 UnitX = (1, 0);

    /// <summary>
    /// A unit <see cref="Vector2"/> pointing in the positive Y direction.
    /// </summary>
    public static readonly Vector2 UnitY = (0, 1);

    /// <summary>
    /// The X component of the vector.
    /// </summary>
    public ref float X => ref values[0];
    /// <summary>
    /// The X component of the vector.
    /// </summary>
    public ref float Y => ref values[1];

    /// <summary>
    /// Creates a new <see cref="Vector2"/> with each component set to the same value.
    /// </summary>
    public Vector2(float value)
    {
        X = Y = value;
    }

    /// <summary>
    /// Creates a new <see cref="Vector2"/>.
    /// </summary>
    public Vector2(float x, float y)
    {
        this.X = x;
        this.Y = y;
    }

    /// <summary>
    /// Creates a new <see cref="Vector2"/>.
    /// </summary>
    public Vector2(Span<float> values)
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
    public Vector2 Normalized()
    {
        var invLength = 1f / Length();

        if (invLength == float.PositiveInfinity)
            return Zero;

        return (X * invLength, Y * invLength);
    }

    /// <summary>
    /// Returns the length of this <see cref="Vector2"/> squared.
    /// </summary>
    public float LengthSquared()
    {
        return Dot(this, this);
    }

    /// <summary>
    /// Returns the length of this <see cref="Vector2"/>.
    /// </summary>
    public float Length()
    {
        return MathF.Sqrt(LengthSquared());
    }

    /// <summary>
    /// Deconstructs this <see cref="Vector2"/> into its induvidual components.
    /// </summary>
    public void Deconstruct(out float x, out float y)
    {
        x = X;
        y = Y;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(values[0], values[1]);
    }

    /// <summary>
    /// Returns a string containing the nicely formatted values of this <see cref="Vector2"/>.
    /// </summary>
    public override string ToString()
    {
        return $"{{{Y}, {X}}}";
    }

    /// <summary>
    /// Checks if two vectors are equal.
    /// </summary>
    public bool Equals(Vector2 other)
    {
        return X == other.X && Y == other.Y;
    }

    /// <summary>
    /// Checks if two vectors are equal.
    /// </summary>
    public override bool Equals(object obj)
    {
        return obj is Vector2 vec && Equals(vec);
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

    /// <summary>
    /// Returns a reference to the element at the specified index into a <see cref="Vector2"/>. 
    /// </summary>
    public ref float this[int index] => ref values[index];

    /// <summary>
    /// Casts from a tuple of 2 floats to a <see cref="Vector2"/>.
    /// </summary>
    public static implicit operator (float, float)(Vector2 vector) => (vector.X, vector.Y);

    /// <summary>
    /// Casts from a <see cref="Vector2"/> to a tuple of 2 floats.
    /// </summary>
    public static implicit operator Vector2((float, float) values) => new(values.Item1, values.Item2);

    /// <summary>
    /// Casts from a <see cref="Vector2"/> to a <see cref="System.Numerics.Vector2"/>.
    /// </summary>
    public static implicit operator System.Numerics.Vector2(Vector2 vector) => new(vector.X, vector.Y);

    /// <summary>
    /// Casts from a <see cref="System.Numerics.Vector2"/> to a <see cref="System.Numerics.Vector2"/>.
    /// </summary>
    public static implicit operator Vector2(System.Numerics.Vector2 vector) => new(vector.X, vector.Y);

    /// <summary>
    /// Checks if two vectors are equal.
    /// </summary>
    public static bool operator ==(Vector2 left, Vector2 right) => left.Equals(right);

    /// <summary>
    /// Checks if two vectors are equal.
    /// </summary>
    public static bool operator !=(Vector2 left, Vector2 right) => !(left == right);

    /// <summary>
    /// Performs per-component addition between two <see cref="Vector2"/> instances.
    /// </summary>
    public static Vector2 operator +(Vector2 left, Vector2 right) => new(left.X + right.X, left.Y + right.Y);
    /// <summary>
    /// Performs per-component subtraction between two <see cref="Vector2"/> instances.
    /// </summary>
    public static Vector2 operator -(Vector2 left, Vector2 right) => new(left.X - right.X, left.Y - right.Y);

    /// <summary>
    /// Negates each component in a <see cref="Vector2"/>.
    /// </summary>
    public static Vector2 operator -(Vector2 value) => new(-value.X, -value.Y);

    /// <summary>
    /// Multiplies each component in a <see cref="Vector4"/> by a scalar value.
    /// </summary>
    public static Vector2 operator *(Vector2 left, float right) => new(left.X * right, left.Y * right);

    /// <summary>
    /// Multiplies each component in a <see cref="Vector4"/> by a scalar value.
    /// </summary>
    public static Vector2 operator *(float left, Vector2 right) => right * left;

    /// <summary>
    /// Performs per-component multiplication between two <see cref="Vector4"/> instances.
    /// </summary>
    public static Vector2 operator *(Vector2 left, Vector2 right) => new(left.X, right.Y);

    /// <summary>
    /// Divides each component in a <see cref="Vector4"/> by a scalar value.
    /// </summary>
    public static Vector2 operator /(Vector2 left, float right) => left * (1f / right);

    /// <summary>
    /// Divides each component in a <see cref="Vector4"/> by a scalar value.
    /// </summary>
    public static Vector2 operator /(float left, Vector2 right) => right * left;

    /// <summary>
    /// Performs per-component division between two <see cref="Vector4"/> instances.
    /// </summary>
    public static Vector2 operator /(Vector2 left, Vector2 right) => (1f / left.X) * right;
}