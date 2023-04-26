using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Tests;

[TestClass]
public class MatrixBuilderTests
{
    [TestMethod]
    public void Multiply_Uninvertable_ThrowsArgumentException()
    {
        var builder = new MatrixBuilder();

        Assert.ThrowsException<ArgumentException>(() =>
        {
            builder.Multiply(default);
        });
    }

    [TestMethod]
    public void Multiply_NullInverted_Inverts()
    {
        var builder = new MatrixBuilder();
        var expectedMatrix = Matrix3x2.CreateTranslation(10, -10) * Matrix3x2.CreateRotation(MathF.PI / 3f);
        Matrix3x2.Invert(expectedMatrix, out var expectedInverseMatrix);

        builder.Multiply(expectedMatrix);

        Assert.AreEqual(expectedInverseMatrix, builder.InverseMatrix);
    }

    [TestMethod]
    public void Multiply_Matrix_OrderOfOperations()
    {
        var builder = new MatrixBuilder();

        // When multiplied, matrix transforms work right to left.
        var expectedMatrix = Matrix3x2.CreateRotation(MathF.PI / 3f) * Matrix3x2.CreateTranslation(10, -10);

        // With MatrixBuilder, it should work in the order which the methods called.
        builder
            .Multiply(Matrix3x2.CreateTranslation(10, -10))
            .Multiply(Matrix3x2.CreateRotation(MathF.PI / 3f));

        Assert.AreEqual(expectedMatrix, builder.Matrix);
    }

    [TestMethod]
    public void Multiply_InverseMatrix_OrderOfOperations()
    {
        // Similar to test above, but the inverse should be in reverse order

        var builder = new MatrixBuilder();

        var expectedMatrix = Matrix3x2.CreateTranslation(10, -10) * Matrix3x2.CreateRotation(MathF.PI / 3f);

        // Pass identity matrix for the non-inverse matrix since we don't care about it for this test
        builder
            .Multiply(Matrix3x2.Identity, Matrix3x2.CreateTranslation(10, -10))
            .Multiply(Matrix3x2.Identity, Matrix3x2.CreateRotation(MathF.PI / 3f));

        Assert.AreEqual(expectedMatrix, builder.InverseMatrix);
    }

    [TestMethod]
    public void Multiply_RoundTrip()
    {
        // Tests if the InverseMatrix property is actually the inverse of the Matrix property after Multiply is called.

        var builder = new MatrixBuilder()
            .Multiply(Matrix3x2.CreateTranslation(10, -10))
            .Multiply(Matrix3x2.CreateRotation(MathF.PI / 3f));

        Assert.AreEqual(Matrix3x2.Identity, builder.Matrix * builder.InverseMatrix);
    }

    [TestMethod]
    public void Reset_SetsIdentity()
    {
        var builder = new MatrixBuilder()
            .Multiply(Matrix3x2.Identity, Matrix3x2.CreateTranslation(10, -10))
            .Multiply(Matrix3x2.Identity, Matrix3x2.CreateRotation(MathF.PI / 3f));


        builder.Reset();

        Assert.AreEqual(Matrix3x2.Identity, builder.Matrix);
        Assert.AreEqual(Matrix3x2.Identity, builder.InverseMatrix);

    }
}
