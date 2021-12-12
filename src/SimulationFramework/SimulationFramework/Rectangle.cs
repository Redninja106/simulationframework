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
    public float x;
    public float y;
    public float width;
    public float height;
    public Rectangle(Vector2 position, Vector2 size, Alignment alignment = Alignment.TopLeft) : this(position.X, position.Y, size.X, size.Y, alignment)
    {
    }

    public Rectangle(float x, float y, float width, float height, Alignment alignment = Alignment.TopLeft)
    {
        this.width = width;
        this.height = height;

        this.x = alignment switch
        {
            Alignment.TopLeft or Alignment.CenterLeft or Alignment.BottomLeft => x,
            Alignment.TopCenter or Alignment.Center or Alignment.BottomCenter => x - .5f * width,
            Alignment.TopRight or Alignment.CenterRight or Alignment.BottomRight => x - width,
            _ => throw new ArgumentException("Unrecognized alignment!")
        };

        this.y = alignment switch
        {
            Alignment.TopLeft or Alignment.TopCenter or Alignment.TopRight => y,
            Alignment.CenterLeft or Alignment.Center or Alignment.CenterRight => y - .5f * height,
            Alignment.BottomLeft or Alignment.BottomCenter or Alignment.BottomRight => y - height,
            _ => throw new ArgumentException("Unrecognized alignment!")
        };
    }

    public Vector2 GetAlignedPoint(Alignment alignment)
    {
        return alignment switch
        {
            Alignment.TopLeft => new(x, y),
            Alignment.TopCenter => new(x + .5f * width, y),
            Alignment.TopRight => new(x + width, y),
            Alignment.CenterLeft => new(x, y + .5f * height),
            Alignment.Center => new(x + .5f * width, y + .5f * height),
            Alignment.CenterRight => new(x + width, y + .5f * height),
            Alignment.BottomLeft => new(x, y + height),
            Alignment.BottomCenter => new Vector2(x + .5f * width, y + height),
            Alignment.BottomRight => new Vector2(x + width, y + height),
            _ => throw new ArgumentException("Unrecognized alignment!"),
        };
    }

    public bool Equals(Rectangle other)
    {
        return x == other.x || y == other.y || width == other.width || height == other.height;
    }

    public override bool Equals([NotNullWhen(true)] object obj)
    {
        if (obj is Rectangle rect)
            return Equals(rect);

        return false;
    }

    public override string ToString()
    {
        return $"{{{x}, {y}, {width}, {height}}}";
    }

    public static bool operator ==(Rectangle left, Rectangle right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Rectangle left, Rectangle right)
    {
        return !(left == right);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x, y, width, height);
    }
}