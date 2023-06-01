using System;
using System.Numerics;
using SimulationFramework.Drawing;
using SkiaSharp;

namespace SimulationFramework.SkiaSharp;

public static class SkiaExtensions
{
    public static SKColor AsSKColor(this Color color)
    {
        return new SKColor(color.R, color.G, color.B, color.A);
    }

    public static SKMatrix AsSKMatrix(this Matrix3x2 matrix)
    {
        return new SKMatrix(matrix.M11, matrix.M21, matrix.M31, matrix.M12, matrix.M22,matrix.M32, 0, 0, 1);
    }

    public static Matrix3x2 AsMatrix3x2(this SKMatrix matrix)
    {
        return new Matrix3x2(
        matrix.ScaleX, matrix.SkewY,
        matrix.SkewX, matrix.ScaleY,
        matrix.TransX, matrix.TransY
        );
    }

    public static SKRect AsSKRect(this Rectangle rect)
    {
        return new SKRect(rect.X, rect.Y, rect.X + rect.Width, rect.Y + rect.Height);
    }

    public static SKPoint AsSKPoint(this Vector2 vector)
    {
        return new SKPoint(vector.X, vector.Y);
    }

    public static SKShaderTileMode AsSKShaderTileMode(this TileMode tileMode)
    {
        return tileMode switch
        {
            TileMode.Clamp => SKShaderTileMode.Clamp,
            TileMode.Repeat => SKShaderTileMode.Repeat,
            TileMode.Mirror => SKShaderTileMode.Mirror,
            TileMode.None => SKShaderTileMode.Decal,
            _ => throw new ArgumentException("Unknown TileMode value!", nameof(tileMode)),
        };
    }
}