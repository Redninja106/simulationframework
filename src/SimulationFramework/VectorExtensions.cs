using System.Numerics;

namespace SimulationFramework;

/// <summary>
/// Provides small utility extensions onto the System.Numerics Vector types.
/// </summary>
public static class VectorExtensions
{
    /// <summary>
    /// Returns this vector with a length of 1.
    /// <para>
    /// When provided a zero-length vector, this method returns a zero-length vector,
    /// unlike <see cref="Vector2.Normalize(Vector2)"/>, which returns <c>(NaN, NaN)</c>.
    /// </para>
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
    /// <para>
    /// When provided a zero-length vector, this method returns a zero-length vector,
    /// unlike <see cref="Vector3.Normalize(Vector3)"/>, which returns <c>(NaN, NaN, NaN)</c>.
    /// </para>
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
    /// 
    /// <para>
    /// When provided a zero-length vector, this method returns a zero-length vector,
    /// unlike <see cref="Vector4.Normalize(Vector4)"/>, which returns <c>(NaN, NaN, NaN, NaN)</c>.
    /// </para>
    /// </summary>
    public static Vector4 Normalized(this Vector4 vector)
    {
        float length = vector.Length();

        if (length is 0.0f)
            return Vector4.Zero;

        float invLength = 1.0f / length;

        return new(vector.X * invLength, vector.Y * invLength, vector.Z * invLength, vector.W * invLength);
    }

    /// <summary>
    /// Deconstructs this vector into its components.
    /// </summary>
    /// <param name="vector">The vector to deconstruct.</param>
    /// <param name="x">The value of the vector's x component.</param>
    /// <param name="y">The value of the vector's y component.</param>
    public static void Deconstruct(this Vector2 vector, out float x, out float y)
    {
        x = vector.X;
        y = vector.Y;
    }

    /// <summary>
    /// Deconstructs this vector into its components.
    /// </summary>
    /// <param name="vector">The vector to deconstruct.</param>
    /// <param name="x">The value of the vector's x component.</param>
    /// <param name="y">The value of the vector's y component.</param>
    /// <param name="z">The value of the vector's y component.</param>
    public static void Deconstruct(this Vector3 vector, out float x, out float y, out float z)
    {
        x = vector.X;
        y = vector.Y;
        z = vector.Z;
    }

    /// <summary>
    /// Deconstructs this vector into its components.
    /// </summary>
    /// <param name="vector">The vector to deconstruct.</param>
    /// <param name="x">The value of the vector's x component.</param>
    /// <param name="y">The value of the vector's y component.</param>
    /// <param name="z">The value of the vector's z component.</param>
    /// <param name="w">The value of the vector's w component.</param>
    public static void Deconstruct(this Vector4 vector, out float x, out float y, out float z, out float w)
    {
        x = vector.X;
        y = vector.Y;
        z = vector.Z;
        w = vector.W;
    }
}