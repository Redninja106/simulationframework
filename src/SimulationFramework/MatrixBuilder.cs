using System;
using System.Numerics;

namespace SimulationFramework;

/// <summary>
/// Provides a mechanism to compose matrix transformations through chaining.
/// </summary>
public class MatrixBuilder
{
    /// <summary>
    /// This builder's result transformation matrix.
    /// </summary>
    public Matrix3x2 Matrix { get; private set; }

    /// <summary>
    /// The inverse of this builder's result transformation matrix.
    /// </summary>
    public Matrix3x2 InverseMatrix { get; private set; }

    /// <summary>
    /// Creates a new instance of the <see cref="MatrixBuilder"/> class.
    /// </summary>
    public MatrixBuilder()
    {
        Reset();
    }

    /// <summary>
    /// Appends a rotation matrix to the current matrix.
    /// </summary>
    /// <param name="rotation">The amount to rotate by.</param>
    /// <returns>This same instance so that methods calls can be chained.</returns>
    public MatrixBuilder Rotate(float rotation) => Rotate(rotation, Vector2.Zero);

    /// <summary>
    /// Appends a rotation matrix around a center point to the current matrix.
    /// </summary>
    /// <param name="rotation">The amount to rotate by.</param>
    /// <param name="centerPoint">The center point of the rotation.</param>
    /// <returns>This same instance so that methods calls can be chained.</returns>
    public MatrixBuilder Rotate(float rotation, Vector2 centerPoint)
    {
        return Multiply(
            Matrix3x2.CreateRotation(rotation, centerPoint), 
            Matrix3x2.CreateRotation(-rotation, centerPoint)
            );
    }

    /// <summary>
    /// Appends a translation matrix to the current matrix.
    /// </summary>
    /// <param name="x">The amount to translate by on the x-axis.</param>
    /// <param name="y">The amount to translate by on the y-axis.</param>
    /// <returns>This same instance so that methods calls can be chained.</returns>
    public MatrixBuilder Translate(float x, float y) => Translate(new(x, y));

    /// <summary>
    /// Appends a translation matrix to the current matrix.
    /// </summary>
    /// <param name="translation">The amount to translate by.</param>
    /// <returns>This same instance so that methods calls can be chained.</returns>
    public MatrixBuilder Translate(Vector2 translation)
    {
        return Multiply(
            Matrix3x2.CreateTranslation(translation), 
            Matrix3x2.CreateTranslation(-translation)
            );
    }

    /// <summary>
    /// Appends a scaling matrix to the current matrix.
    /// </summary>
    /// <param name="scale">The amount to scale by.</param>
    /// <returns>This same instance so that methods calls can be chained.</returns>
    public MatrixBuilder Scale(float scale) => Scale(scale, scale);

    /// <summary>
    /// Appends a scaling matrix to the current matrix.
    /// </summary>
    /// <param name="x">The amount to scale by on the x-axis.</param>
    /// <param name="y">The amount to scale by on the y-axis.</param>
    /// <returns>This same instance so that methods calls can be chained.</returns>
    public MatrixBuilder Scale(float x, float y) => Scale(new Vector2(x, y));

    /// <summary>
    /// Appends a scaling matrix to the current matrix.
    /// </summary>
    /// <param name="scale">The amount to scale by.</param>
    /// <returns>This same instance so that methods calls can be chained.</returns>
    public MatrixBuilder Scale(Vector2 scale) => Scale(scale, Vector2.Zero);

    /// <summary>
    /// Appends a scaling matrix to the current matrix.
    /// </summary>
    /// <param name="scale">The amount to scale by.</param>
    /// <param name="centerPoint">The center point of the scaling.</param>
    /// <returns>This same instance so that methods calls can be chained.</returns>
    public MatrixBuilder Scale(float scale, Vector2 centerPoint) => Scale(scale, centerPoint);

    /// <summary>
    /// Appends a scaling matrix to the current matrix.
    /// </summary>
    /// <param name="x">The amount to scale by on the x-axis.</param>
    /// <param name="y">The amount to scale by on the y-axis.</param>
    /// <param name="centerPoint">The center point of the scaling.</param>
    /// <returns>This same instance so that methods calls can be chained.</returns>
    public MatrixBuilder Scale(float x, float y, Vector2 centerPoint) => Scale(new Vector2(x, y), centerPoint);

    /// <summary>
    /// Appends a scaling matrix to the current matrix.
    /// </summary>
    /// <param name="scale">The amount to scale by.</param>
    /// <param name="centerPoint">The center point of the scaling.</param>
    /// <returns>This same instance so that methods calls can be chained.</returns>
    public MatrixBuilder Scale(Vector2 scale, Vector2 centerPoint)
    {
        return Multiply(
            Matrix3x2.CreateScale(scale, centerPoint), 
            Matrix3x2.CreateScale(Vector2.One / scale, centerPoint)
            );
    }

    /// <summary>
    /// Appends a skew matrix to the current matrix.
    /// </summary>
    /// <param name="radiansX">The angle to skew the X axis by.</param>
    /// <param name="radiansY">The angle to skew the Y axis by.</param>
    /// <returns>This same instance so that methods calls can be chained.</returns>
    public MatrixBuilder Skew(float radiansX, float radiansY) => Skew(radiansX, radiansY, Vector2.Zero);

    /// <summary>
    /// Appends a skew matrix to the current matrix.
    /// </summary>
    /// <param name="radiansX">The angle to skew the X axis by.</param>
    /// <param name="radiansY">The angle to skew the Y axis by.</param>
    /// <param name="centerPoint">The center point of the skew.</param>
    /// <returns>This same instance so that methods calls can be chained.</returns>
    public MatrixBuilder Skew(float radiansX, float radiansY, Vector2 centerPoint)
    {
        return Multiply(
            Matrix3x2.CreateSkew(radiansX, radiansY, centerPoint),
            Matrix3x2.CreateSkew(-radiansX, -radiansY, centerPoint)
            );
    }

    /// <summary>
    /// Appends a matrix to the current matrix.
    /// </summary>
    /// <param name="matrix">The matrix to append.</param>
    /// <param name="inverse">The inverse of the matrix to append. If this value is null, the computed inverse of <paramref name="matrix"/> is used.</param>
    /// <returns>This same instance so that methods calls can be chained.</returns>
    public MatrixBuilder Multiply(Matrix3x2 matrix, Matrix3x2? inverse = null)
    {
        if (inverse is null)
        {
            if (Matrix3x2.Invert(matrix, out var invertedValue))
            {
                inverse = invertedValue;
            }
            else 
            {
                throw new ArgumentException("'inverse' is null and 'transform' is not invertable!");
            } 
        }

        this.Matrix = matrix * this.Matrix;
        this.InverseMatrix *= inverse.Value;

        return this;
    }

    /// <summary>
    /// Resets the current matrix and its inverse to <see cref="Matrix3x2.Identity"/>.
    /// </summary>
    /// <returns>This same instance so that methods calls can be chained.</returns>
    public MatrixBuilder Reset()
    {
        Matrix = Matrix3x2.Identity;
        InverseMatrix = Matrix3x2.Identity;
        return this;
    }
}
