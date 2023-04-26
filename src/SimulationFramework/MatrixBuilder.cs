using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;
public class MatrixBuilder
{
    public Matrix3x2 Matrix { get; private set; }
    public Matrix3x2 InverseMatrix { get; private set; }

    public MatrixBuilder()
    {
        Reset();
    }

    public MatrixBuilder Rotate(float rotation) => Rotate(rotation, Vector2.Zero);
    public MatrixBuilder Rotate(float rotation, Vector2 centerPoint)
    {
        return Multiply(
            Matrix3x2.CreateRotation(rotation, centerPoint), 
            Matrix3x2.CreateRotation(-rotation, centerPoint)
            );
    }

    public MatrixBuilder Translate(float x, float y) => Translate(new(x, y));
    public MatrixBuilder Translate(Vector2 translation)
    {
        return Multiply(
            Matrix3x2.CreateTranslation(translation), 
            Matrix3x2.CreateTranslation(-translation)
            );
    }

    public MatrixBuilder Scale(float scale) => Scale(scale, scale);
    public MatrixBuilder Scale(float x, float y) => Scale(new Vector2(x, y));
    public MatrixBuilder Scale(Vector2 scale) => Scale(scale, Vector2.Zero);
    public MatrixBuilder Scale(float scale, Vector2 centerPoint) => Scale(scale, centerPoint);
    public MatrixBuilder Scale(float x, float y, Vector2 centerPoint) => Scale(new Vector2(x, y), centerPoint);
    public MatrixBuilder Scale(Vector2 scale, Vector2 centerPoint)
    {
        return Multiply(
            Matrix3x2.CreateScale(scale, centerPoint), 
            Matrix3x2.CreateScale(Vector2.One / scale, centerPoint)
            );
    }

    public MatrixBuilder Skew(float radiansX, float radiansY) => Skew(radiansX, radiansY, Vector2.Zero);
    public MatrixBuilder Skew(float radiansX, float radiansY, Vector2 centerPoint)
    {
        return Multiply(
            Matrix3x2.CreateSkew(radiansX, radiansY),
            Matrix3x2.CreateSkew(-radiansX, -radiansY)
            );
    }

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

    public MatrixBuilder Reset()
    {
        Matrix = Matrix3x2.Identity;
        InverseMatrix = Matrix3x2.Identity;
        return this;
    }
}
