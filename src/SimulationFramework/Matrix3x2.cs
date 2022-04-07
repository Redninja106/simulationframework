﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

/// <summary>
/// A 2d transformation matrix.
/// </summary>
public struct Matrix3x2 : IEquatable<Matrix3x2>
{
    /// <summary>
    /// The identity matrix.
    /// </summary>
    public static readonly Matrix3x2 Identity = new(1,0,0,1,0,0);

    /// <summary>
    /// The top-left value of the matrix.
    /// </summary>
    public float M11;

    /// <summary>
    /// The bottom-center value of the matrix.
    /// </summary>
    public float M12;

    /// <summary>
    /// The top-center value of the matrix.
    /// </summary>
    public float M21;
    
    /// <summary>
    /// The bottom-center value of the matrix.
    /// </summary>
    public float M22;

    /// <summary>
    /// The top-right value of the matrix.
    /// </summary>
    public float M31;

    /// <summary>
    /// The bottom-right value of the matrix.
    /// </summary>
    public float M32;

    /// <summary>
    /// The translation component of the martix.
    /// </summary>
    public Vector2 Translation { get => (M31, M32); set => (M31, M32) = value; }

    /// <summary>
    /// Whether the tranlation 
    /// </summary>
    public bool IsIdentity => this == Identity;

    ///
    public Matrix3x2(float m11, float m12, float m21, float m22, float m31, float m32)
    {
        M11 = m11;
        M12 = m12;
        M21 = m21;
        M22 = m22;
        M31 = m31;
        M32 = m32;
    }

    ///
    public bool Equals(Matrix3x2 other)
    {
        return
            M11 == other.M11 && M12 == other.M12 &&
            M21 == other.M21 && M22 == other.M22 &&
            M31 == other.M31 && M32 == other.M32;
    }

    ///
    public override bool Equals([NotNullWhen(true)] object obj)
    {
        if (obj is Matrix3x2 matrix)
            return Equals(matrix);

        return false;
    }

    ///
    public static bool operator ==(Matrix3x2 a, Matrix3x2 b)
    {
        return a.Equals(b);
    }

    /// 
    public static bool operator !=(Matrix3x2 a, Matrix3x2 b)
    {
        return a.Equals(b);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(M11, M12, M21, M22, M31, M32);
    }

    ///
    public static Matrix3x2 operator *(Matrix3x2 a, Matrix3x2 b)
    {
        Matrix3x2 m;

        // First row
        m.M11 = a.M11 * b.M11 + a.M12 * b.M21;
        m.M12 = a.M11 * b.M12 + a.M12 * b.M22;

        // Second row
        m.M21 = a.M21 * b.M11 + a.M22 * b.M21;
        m.M22 = a.M21 * b.M12 + a.M22 * b.M22;

        // Third row
        m.M31 = a.M31 * b.M11 + a.M32 * b.M21 + b.M31;
        m.M32 = a.M31 * b.M12 + a.M32 * b.M22 + b.M32;

        return m;
    }

    ///
    public static Matrix3x2 operator *(Matrix3x2 a, float b)
    {
        Matrix3x2 m;

        // First row
        m.M11 = a.M11 * b;
        m.M12 = a.M12 * b;

        // Second row
        m.M21 = a.M21 * b;
        m.M22 = a.M22 * b;

        // Third row
        m.M31 = a.M31 * b;
        m.M32 = a.M32 * b;

        return m;
    }

    /// <summary>
    /// Casts from this matrix type to the one in System.Numerics.
    /// </summary>
    public static implicit operator Matrix3x2(System.Numerics.Matrix3x2 matrix)
    {
        return Unsafe.As<System.Numerics.Matrix3x2, Matrix3x2>(ref matrix);
    }

    /// <summary>
    /// Casts from the System.Numerics matrix type to this one.
    /// </summary>
    public static implicit operator System.Numerics.Matrix3x2(Matrix3x2 matrix)
    {
        return Unsafe.As<Matrix3x2, System.Numerics.Matrix3x2>(ref matrix);
    }

    /// <summary>
    /// Creates a scaling matrix.
    /// </summary>
    public static Matrix3x2 CreateScale(float scale) => CreateScale(scale, scale);
    
    /// <summary>
    /// Creates a scaling matrix.
    /// </summary>
    public static Matrix3x2 CreateScale(Vector2 scale) => CreateScale(scale.X, scale.Y);

    /// <summary>
    /// Creates a scaling matrix.
    /// </summary>
    public static Matrix3x2 CreateScale(float scaleX, float scaleY)
    {
        return new Matrix3x2(scaleX, 0, 0, scaleY, 0, 0);
    }

    /// <summary>
    /// Creates a rotation matrix around the origin.
    /// </summary>
    public static Matrix3x2 CreateRotation(float rotation) => CreateRotation(rotation, Vector2.Zero);

    /// <summary>
    /// Creates a rotation matrix around the provided center point.
    /// </summary>
    public static Matrix3x2 CreateRotation(float rotation, float centerX, float centerY) => CreateRotation(rotation, (centerX, centerY));
    
    /// <summary>
    /// Creates a rotation matrix around the provided center point.
    /// </summary>
    public static Matrix3x2 CreateRotation(float rotation, Vector2 center)
    {
        float s = MathF.Sin(rotation), c = MathF.Cos(rotation);
        return new Matrix3x2(c, s, -s, c, center.X, center.Y);
    }

    /// <summary>
    /// Creates a translation matrix.
    /// </summary>
    public static Matrix3x2 CreateTranslation(float x, float y) => CreateTranslation((x, y));

    /// <summary>
    /// Creates a translation matrix.
    /// </summary>
    public static Matrix3x2 CreateTranslation(Vector2 translation)
    {
        return Identity with { Translation = translation };
    }

    public bool Invert(out Matrix3x2 result)
    {
        float determinant = GetDeterminant(); ;

        if (MathF.Abs(determinant) < float.Epsilon)
        {
            result = new Matrix3x2(float.NaN, float.NaN, float.NaN, float.NaN, float.NaN, float.NaN);
            return false;
        }

        float invDeterminant = 1.0f / determinant;

        result.M11 = M22 * invDeterminant;
        result.M12 = -M12 * invDeterminant;

        result.M21 = -M21 * invDeterminant;
        result.M22 = M11 * invDeterminant;

        result.M31 = (M21 * M32 - M31 * M22) * invDeterminant;
        result.M32 = (M31 * M12 - M11 * M32) * invDeterminant;

        return true;
    }

    public float GetDeterminant()
    {
        // :) https://www.mathsisfun.com/algebra/matrix-determinant.html
        return M11 * M22 - M21 * M12;
    }
}