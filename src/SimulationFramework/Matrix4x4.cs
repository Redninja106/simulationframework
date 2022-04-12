using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace SimulationFramework;

/// <summary>
/// A 4x4 row-major transformation matrix.
/// </summary>
public unsafe struct Matrix4x4
{
    private const int ELEMENT_COUNT = 16;

    /// <summary>
    /// The identity matrix.
    /// </summary>
    public static readonly Matrix4x4 Identity = new(
        1, 0, 0, 0,
        0, 1, 0, 0,
        0, 0, 1, 0,
        0, 0, 0, 1
        );

    private unsafe fixed float values[ELEMENT_COUNT];

    /// <summary>
    /// Creates a new <see cref="Matrix4x4"/> from a span of values.
    /// </summary>
    /// <param name="values">The values for the matrix. Must have 16 elements.</param>
    /// <exception cref="ArgumentException"></exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Matrix4x4(Span<float> values)
    {
        if (values.Length != ELEMENT_COUNT)
            throw new ArgumentException("Values must have a length of 16", nameof(values));

        for (int i = 0; i < ELEMENT_COUNT; i++)
            this.values[i] = values[i];
    }

    /// <summary>
    /// Creates a new <see cref="Matrix4x4"/> from the specified values.
    /// </summary>
    /// <param name="m11">The first element of the first row of the matrix.</param>
    /// <param name="m12">The second element of the first row of the matrix.</param>
    /// <param name="m13">The third element of the first row of the matrix.</param>
    /// <param name="m14">The fourth element of the first row of the matrix.</param>
    /// <param name="m21">The first element of the second row of the matrix.</param>
    /// <param name="m22">The second element of the second row of the matrix.</param>
    /// <param name="m23">The third element of the second row of the matrix.</param>
    /// <param name="m24">The fourth element of the second row of the matrix.</param>
    /// <param name="m31">The first element of the third row of the matrix.</param>
    /// <param name="m32">The second element of the third row of the matrix.</param>
    /// <param name="m33">The third element of the third row of the matrix.</param>
    /// <param name="m34">The fourth element of the third row of the matrix.</param>
    /// <param name="m41">The first element of the fourth row of the matrix.</param>
    /// <param name="m42">The second element of the fourth row of the matrix.</param>
    /// <param name="m43">The third element of the fourth row of the matrix.</param>
    /// <param name="m44">The fourth element of the fourth row of the matrix.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Matrix4x4(float m11, float m12, float m13, float m14, 
        float m21, float m22, float m23, float m24, 
        float m31, float m32, float m33, float m34,
        float m41, float m42, float m43, float m44)
    {
        this.values[0] = m11;
        this.values[1] = m12;
        this.values[2] = m13;
        this.values[3] = m14;
        
        this.values[4] = m21;
        this.values[5] = m22;
        this.values[6] = m23;
        this.values[7] = m24;
        
        this.values[8] = m31;
        this.values[9] = m32;
        this.values[10] = m33;
        this.values[11] = m34;
        
        this.values[12] = m41;
        this.values[13] = m42;
        this.values[14] = m43;
        this.values[15] = m44;
    }

    /// <summary>
    /// Returns a <see cref="Vector4"/> containing the values in the specified row of the matrix.
    /// </summary>
    public Vector4 GetRow(int row)
    {
        return new Vector4(
            values[row * 4 + 0],
            values[row * 4 + 1],
            values[row * 4 + 2],
            values[row * 4 + 3]);
    }

    /// <summary>
    /// Returns a <see cref="Vector4"/> containing the values in the specified column of the matrix.
    /// </summary>
    public Vector4 GetColumn(int column)
    {
        return new Vector4(
            values[column + 0 * 4],
            values[column + 1 * 4],
            values[column + 2 * 4],
            values[column + 3 * 4]);
    }

    public float GetDeterminant()
    {
        throw new NotImplementedException();
    }
    
    public static Matrix4x4 Invert(Matrix4x4 matrix)
    {
        if (TryInvert(matrix, out var result))
            return result;
        else 
            throw new Exception("Matrix is not invertible");
    }

    public static bool TryInvert(Matrix4x4 matrix, out Matrix4x4 result)
    {
        // System.Numerics.Matrix4x4 contains a version of this method I could only dream of implementing. For the time being, use that.
        return System.Numerics.Matrix4x4.Invert(matrix, out Unsafe.As<Matrix4x4, System.Numerics.Matrix4x4>(ref result));
    }

    /// <summary>
    /// Creates a translation matrix.
    /// </summary>
    /// <param name="translation"></param>
    /// <returns></returns>
    public static Matrix4x4 CreateTranslation(Vector3 translation)
    {
        Matrix4x4 result = Identity;

        result[0, 3] = translation.X;
        result[1, 3] = translation.Y;
        result[2, 3] = translation.Z;

        return result;
    }
        /// <summary>
    /// Gets the element at the provided row and column.
    /// </summary>
    public ref float this[int row, int column]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            Debug.Assert(row >= 0 && row < 4);
            Debug.Assert(column >= 0 && column< 4);
            return ref values[column * 4 + row];
        }
    }

    /// <summary>
    /// Converts a <see cref="System.Numerics.Matrix4x4"/> to a <see cref="SimulationFramework.Matrix4x4"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Matrix4x4(System.Numerics.Matrix4x4 matrix)
    {
        return Unsafe.As<System.Numerics.Matrix4x4, Matrix4x4>(ref matrix);
    }
    
    /// <summary>
    /// Converts a <see cref="SimulationFramework.Matrix4x4"/> to a <see cref="System.Numerics.Matrix4x4"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator System.Numerics.Matrix4x4(Matrix4x4 matrix)
    {
        return Unsafe.As<Matrix4x4, System.Numerics.Matrix4x4>(ref matrix);
    }

    /// <summary>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Matrix4x4 left, Matrix4x4 right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Matrix4x4 left, Matrix4x4 right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Multiplies each value in the matrix by a scalar.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4x4 operator *(Matrix4x4 left, float right)
    {
        for (int i = 0; i < ELEMENT_COUNT; i++)
            left.values[i] *= right;

        return left;
    }

    /// <summary>
    /// Multiplies two matricies and returns the result.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4x4 operator *(Matrix4x4 left, Matrix4x4 right)
    {
        Matrix4x4 result;

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                result[j, i] = Vector4.Dot(left.GetRow(j), right.GetColumn(i));
            }
        }

        return result;
    }

    // transforms a vector by a matrix
    public static Vector4 operator *(Matrix4x4 left, Vector4 right)
    {
        Vector4 result;

        for (int i = 0; i < 4; i++)
        {
            result[i] = Vector4.Dot(left.GetRow(i), right);
        }

        return result;
        
    }
}