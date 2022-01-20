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
    /// The size of the rectangle
    /// </summary>
    public Vector2 Size { get => (Width, Height); set => (Width, Height) = value; }

    public Vector2 Position { get => (X, Y); set => (X, Y) = value; }

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

        this.X = alignment switch
        {
            Alignment.TopLeft or Alignment.CenterLeft or Alignment.BottomLeft => x,
            Alignment.TopCenter or Alignment.Center or Alignment.BottomCenter => x - .5f * width,
            Alignment.TopRight or Alignment.CenterRight or Alignment.BottomRight => x - width,
            _ => throw new ArgumentException("Unrecognized alignment!")
        };

        this.Y = alignment switch
        {
            Alignment.TopLeft or Alignment.TopCenter or Alignment.TopRight => y,
            Alignment.CenterLeft or Alignment.Center or Alignment.CenterRight => y - .5f * height,
            Alignment.BottomLeft or Alignment.BottomCenter or Alignment.BottomRight => y - height,
            _ => throw new ArgumentException("Unrecognized alignment!")
        };
    }

    /// <summary>
    /// Returns the point which aligned to the rectangle by the specified alignment.
    /// </summary>
    public Vector2 GetAlignedPoint(Alignment alignment)
    {
        return alignment switch
        {
            Alignment.TopLeft => new(X, Y),
            Alignment.TopCenter => new(X + .5f * Width, Y),
            Alignment.TopRight => new(X + Width, Y),
            Alignment.CenterLeft => new(X, Y + .5f * Height),
            Alignment.Center => new(X + .5f * Width, Y + .5f * Height),
            Alignment.CenterRight => new(X + Width, Y + .5f * Height),
            Alignment.BottomLeft => new(X, Y + Height),
            Alignment.BottomCenter => new Vector2(X + .5f * Width, Y + Height),
            Alignment.BottomRight => new Vector2(X + Width, Y + Height),
            _ => throw new ArgumentException("Unrecognized alignment!"),
        };
    }

    /// <summary>
    /// Determines if the rectangle contains the provided point.
    /// </summary>
    /// <param name="point">The point to check the rectangle for.</param>
    /// <returns>True if the rectangle contains <paramref name="point"/>, otherwise false.</returns>
    public bool ContainsPoint(Vector2 point)
    {
        return this.X <= point.X && this.X + this.Width >= point.X &&
            this.Y <= point.Y && this.Y + this.Height >= point.Y;
    }

    /// <summary>
    /// Determines if the rectangle has any intersection with another.
    /// </summary>
    /// <param name="other">The rectangle to check for an intersection with.</param>
    /// <returns>True if the rectangles are intersecting, otherwise false.</returns>
    public bool Intersects(Rectangle other)
    {
        return Intersects(other, out _);
    }

    /// <summary>
    /// Determines if the rectangle has any intersection with another.
    /// </summary>
    /// <param name="other">The rectangle to check for an intersection with.</param>
    /// <param name="intersection">The intersection area between the two rectangles.</param>
    /// <returns>True if the rectangles are intersecting, otherwise false.</returns>
    public bool Intersects(Rectangle other, out Rectangle intersection)
    {
        float x1 = MathF.Max(this.X, other.X);
        float x2 = MathF.Min(this.X + this.Width, other.X + other.Width);
        float y1 = MathF.Max(this.Y, other.X);
        float y2 = MathF.Min(this.Y + this.Height, other.Y + other.Height);
        var intersects = x2 >= x1 && y2 >= y1;

        if (intersects)
        {
            intersection = new Rectangle(x1, y1, x2 - x1, y2 - y1);
            return true;
        }

        intersection = default;
        return false;
    }

    public static implicit operator (float, float, float, float)(Rectangle rectangle)
    {
        return (rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
    }

    public static implicit operator Rectangle((float x, float y, float width, float height) rectangle)
    {
        return new(rectangle.x, rectangle.y, rectangle.width, rectangle.height);
    }

    public static implicit operator (Vector2, Vector2)(Rectangle rectangle)
    {
        return (rectangle.Position, rectangle.Size);
    }

    public static implicit operator Rectangle((Vector2 position, Vector2 size) rectangle)
    {
        return new(rectangle.position, rectangle.size);
    }

    ///
    public bool Equals(Rectangle other)
    {
        return X == other.X || Y == other.Y || Width == other.Width || Height == other.Height;
    }

    ///
    public override bool Equals([NotNullWhen(true)] object obj)
    {
        if (obj is Rectangle rect)
            return Equals(rect);

        return false;
    }

    ///
    public override string ToString()
    {
        return $"{{{X}, {Y}, {Width}, {Height}}}";
    }

    ///
    public static bool operator ==(Rectangle left, Rectangle right)
    {
        return left.Equals(right);
    }
    
    ///
    public static bool operator !=(Rectangle left, Rectangle right)
    {
        return !(left == right);
    }

    ///
    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Width, Height);
    }

    /// <summary>
    /// Creates a rectangle from the position of each side.
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public static Rectangle CreateLTRB((float left, float top, float right, float bottom) args)
    {
        return CreateLTRB(args.left, args.top, args.right, args.bottom);
    }

    /// <summary>
    /// Creates a rectangle from the position of each side.
    /// </summary>
    /// <param name="left">The position of the left side of the rectangle on the x-axis.</param>
    /// <param name="top">The position of the top of the rectangle on the y-axis.</param>
    /// <param name="right">The position of the right side of the rectangle on the x-axis.</param>
    /// <param name="bottom">The position of the bottom of the rectangle on the y-axis.</param>
    public static Rectangle CreateLTRB(float left, float top, float right, float bottom)
    {
        return new Rectangle(left, top, right - left, bottom - top);
    }
}