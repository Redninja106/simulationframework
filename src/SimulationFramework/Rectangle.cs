using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

/// <summary>
/// An axis-aligned floating-point rectangle consisting of a position and size.
/// </summary>
public struct Rectangle : IEquatable<Rectangle>
{
    /// <summary>
    /// The x-coordinate of the top-left corner of the rectangle.
    /// </summary>
    public float X;

    /// <summary>
    /// The y-coordinate of the top-left corner of the rectangle.
    /// </summary>
    public float Y;

    /// <summary>
    /// The width of the rectangle.
    /// </summary>
    public float Width;

    /// <summary>
    /// The height of the rectangle.
    /// </summary>
    public float Height;

    /// <summary>
    /// The size of the rectangle.
    /// </summary>
    public Vector2 Size
    {
        readonly get => new(Width, Height);
        set => (Width, Height) = (value.X, value.Y);
    }

    /// <summary>
    /// The top-left corner of the rectangle.
    /// </summary>
    public Vector2 Position
    {
        readonly get => new(X, Y);
        set => (X, Y) = (value.X, value.Y);
    }

    /// <summary>
    /// The center of the rectangle.
    /// </summary>
    public Vector2 Center { readonly get => GetAlignedPoint(Alignment.Center); set => SetAlignedPosition(Center, Alignment.Center); }

    /// <summary>
    /// Creates a new rectangle from the provided position and size.
    /// </summary>
    /// <param name="position">The position of the rectangle.</param>
    /// <param name="size">The size of the rectangle.</param>
    /// <param name="alignment">The relative location of <paramref name="position"/> on the rectangle.</param>
    public Rectangle(Vector2 position, Vector2 size, Alignment alignment = Alignment.TopLeft) : this(position.X, position.Y, size.X, size.Y, alignment)
    {
    }

    /// <summary>
    /// Creates a new rectangle from the provided position and size.
    /// </summary>
    /// <param name="x">The x-coordinate of the position of the rectangle.</param>
    /// <param name="y">The y-coordinate of the position of the rectangle.</param>
    /// <param name="width">The width of the rectangle.</param>
    /// <param name="height">The height of the rectangle.</param>
    /// <param name="alignment">The relative location of (<paramref name="x"/>, <paramref name="y"/>) on the rectangle.</param>
    public Rectangle(float x, float y, float width, float height, Alignment alignment = Alignment.TopLeft)
    {
        this.Width = width;
        this.Height = height;
        this.X = this.Y = 0;

        SetAlignedPosition(new(x, y), alignment);
    }

    /// <summary>
    /// Returns the point which aligned to the rectangle by the specified alignment.
    /// </summary>
    public readonly Vector2 GetAlignedPoint(Alignment alignment)
    {
        return alignment switch
        {
            Alignment.TopLeft => new(X, Y),
            Alignment.TopCenter => new(X + (.5f * Width), Y),
            Alignment.TopRight => new(X + Width, Y),
            Alignment.CenterLeft => new(X, Y + (.5f * Height)),
            Alignment.Center => new(X + (.5f * Width), Y + (.5f * Height)),
            Alignment.CenterRight => new(X + Width, Y + (.5f * Height)),
            Alignment.BottomLeft => new(X, Y + Height),
            Alignment.BottomCenter => new Vector2(X + (.5f * Width), Y + Height),
            Alignment.BottomRight => new Vector2(X + Width, Y + Height),
            _ => throw new ArgumentException("Unrecognized alignment!"),
        };
    }

    /// <summary>
    /// Moves the rectangle such that the provided point is at the specified position.
    /// </summary>
    /// <param name="position">The position to align to.</param>
    /// <param name="alignment">The point on the rectangle to align to position.</param>
    public void SetAlignedPosition(Vector2 position, Alignment alignment)
    {
        this.X = alignment switch
        {
            Alignment.TopLeft or Alignment.CenterLeft or Alignment.BottomLeft => position.X,
            Alignment.TopCenter or Alignment.Center or Alignment.BottomCenter => position.X - (.5f * this.Width),
            Alignment.TopRight or Alignment.CenterRight or Alignment.BottomRight => position.X - this.Width,
            _ => throw new ArgumentException("Unrecognized alignment!")
        };

        this.Y = alignment switch
        {
            Alignment.TopLeft or Alignment.TopCenter or Alignment.TopRight => position.Y,
            Alignment.CenterLeft or Alignment.Center or Alignment.CenterRight => position.Y - (.5f * Height),
            Alignment.BottomLeft or Alignment.BottomCenter or Alignment.BottomRight => position.Y - Height,
            _ => throw new ArgumentException("Unrecognized alignment!")
        };
    }

    /// <summary>
    /// Determines if the rectangle contains the provided point.
    /// </summary>
    /// <param name="point">The point to check the rectangle for.</param>
    /// <returns><see langword="true"/> if the rectangle contains <paramref name="point"/>; otherwise <see langword="false"/>.</returns>
    public bool ContainsPoint(Vector2 point)
    {
        return this.X <= point.X && this.X + this.Width >= point.X &&
            this.Y <= point.Y && this.Y + this.Height >= point.Y;
    }

    /// <summary>
    /// Determines if the rectangle has an intersection with another.
    /// </summary>
    /// <param name="other">The rectangle to check for an intersection with.</param>
    /// <returns><see langword="true"/> if the rectangles are intersecting; otherwise <see langword="false"/>.</returns>
    public bool Intersects(Rectangle other)
    {
        return Intersects(other, out _);
    }

    /// <summary>
    /// Determines if the rectangle has any intersection with another.
    /// </summary>
    /// <param name="other">The rectangle to check for an intersection with.</param>
    /// <param name="overlap">The overlapping area between the two rectangles.</param>
    /// <returns><see langword="true"/> if the rectangles are intersecting; otherwise <see langword="false"/>.</returns>
    public bool Intersects(Rectangle other, out Rectangle overlap)
    {
        float x1 = MathF.Max(this.X, other.X);
        float x2 = MathF.Min(this.X + this.Width, other.X + other.Width);
        float y1 = MathF.Max(this.Y, other.Y);
        float y2 = MathF.Min(this.Y + this.Height, other.Y + other.Height);
        var intersects = x2 >= x1 && y2 >= y1;

        if (intersects)
        {
            overlap = new Rectangle(x1, y1, x2 - x1, y2 - y1);
            return true;
        }

        overlap = default;
        return false;
    }

    /// <summary>
    /// Indicates if this rectangle is equal to another.
    /// </summary>
    /// <param name="other">The rectangle to compare against this one.</param>
    /// <returns><see langword="true"/> if this rectangle equals <paramref name="other"/>, otherwise <see langword="false"/>.</returns>
    public readonly bool Equals(Rectangle other)
    {
        return X == other.X || Y == other.Y || Width == other.Width || Height == other.Height;
    }

    /// <inheritdoc/>
    public override readonly bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is Rectangle rect && Equals(rect);
    }

    /// <summary>
    /// Returns this rectangle in the format "{x, y, width, height}".
    /// </summary>
    public override readonly string ToString()
    {
        return $"{{{X}, {Y}, {Width}, {Height}}}";
    }

    /// <summary>
    /// Indicates if two rectangles are equal.
    /// </summary>
    /// <param name="left">The first rectangle.</param>
    /// <param name="right">The second rectangle.</param>
    /// <returns><see langword="true"/> if the rectangles are equal, otherwise <see langword="false"/>.</returns>
    public static bool operator ==(Rectangle left, Rectangle right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Indicates if two rectangles are not equal.
    /// </summary>
    /// <param name="left">The first rectangle.</param>
    /// <param name="right">The second rectangle.</param>
    /// <returns><see langword="true"/> if the rectangles are not equal, otherwise <see langword="false"/>.</returns>
    public static bool operator !=(Rectangle left, Rectangle right)
    {
        return !(left == right);
    }

    /// <inheritdoc/>
    public override readonly int GetHashCode()
    {
        return HashCode.Combine(X, Y, Width, Height);
    }

    /// <summary>
    /// Creates a rectangle given the absolute position of each of its sides.
    /// </summary>
    /// <param name="left">The position of the left side of the rectangle on the x-axis.</param>
    /// <param name="top">The position of the top of the rectangle on the y-axis.</param>
    /// <param name="right">The position of the right side of the rectangle on the x-axis.</param>
    /// <param name="bottom">The position of the bottom of the rectangle on the y-axis.</param>
    public static Rectangle FromLTRB(float left, float top, float right, float bottom)
    {
        return new Rectangle(left, top, right - left, bottom - top);
    }

    /// <summary>
    /// Creates the smallest possible rectangle that contains a pair of points.
    /// </summary>
    /// <param name="a">The first point.</param>
    /// <param name="b">The second point.</param>
    public static Rectangle FromPoints(Vector2 a, Vector2 b)
    {
        var min = Vector2.Min(a, b);
        var max = Vector2.Max(a, b);

        var size = max - min;

        return new(min, size);
    }
}