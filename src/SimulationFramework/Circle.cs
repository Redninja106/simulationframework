using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace SimulationFramework;

/// <summary>
/// A floating point 2-dimensional circle.
/// </summary>
public struct Circle : IEquatable<Circle>
{
    /// <summary>
    /// The unit circle at (0, 0).
    /// </summary>
    public static readonly Circle Unit = new(0, 0, 1f);

    /// <summary>
    /// The position of the circle.
    /// </summary>
    public Vector2 Position;

    /// <summary>
    /// The radius of the circle.
    /// </summary>
    public float Radius;

    /// <summary>
    /// Creates a new circle given its position and radius.
    /// </summary>
    /// <param name="x">The x position of the circle.</param>
    /// <param name="y">The y position of the circle.</param>
    /// <param name="radius">The radius of the circle.</param>
    /// <param name="alignment">The location of <paramref name="x"/> and <paramref name="y"/> on the circle's bounding box.</param>
    public Circle(float x, float y, float radius, Alignment alignment = Alignment.Center) : this(new(x, y), radius, alignment)
    {
    }

    /// <summary>
    /// Creates a new circle given its position and radius.
    /// </summary>
    /// <param name="position">The position of the circle.</param>
    /// <param name="radius">The radius of the circle.</param>
    /// <param name="alignment">The location of <paramref name="position"/> on the circle's bounding box.</param>
    public Circle(Vector2 position, float radius, Alignment alignment = Alignment.Center)
    {
        // compute circle bounds to find center
        Rectangle bounds = new(position, new(radius * 2), alignment);
        Position = bounds.Center;

        Radius = radius;
    }

    /// <summary>
    /// Computes the bounding rectangle of this circle.
    /// </summary>
    /// <returns>The bounds of this circle.</returns>
    public Rectangle GetBounds()
    {
        return new(this.Position, new(this.Radius * 2), Alignment.Center);
    }

    /// <summary>
    /// Computes a point on this circle given an angle.
    /// </summary>
    /// <returns>The computed point.</returns>
    public Vector2 GetPoint(float angle)
    {
        return Position + new Vector2(MathF.Cos(angle) * Radius, MathF.Sin(angle) * Radius);
    }


    /// <summary>
    /// Computes the distance from this circle to a given point.
    /// </summary>
    /// <param name="point">The point to compute the distance to.</param>
    /// <returns>The computed distance, or 0 if the point lies within this circle.</returns>
    public float Distance(Vector2 point)
    {
        return Distance(new Circle(point, 0));
    }


    /// <summary>
    /// Computes the distance from this circle to another.
    /// </summary>
    /// <param name="other">The circle to compute the distance to.</param>
    /// <returns>The computed distance, or 0 if the point lies within this circle.</returns>
    public float Distance(Circle other)
    {
        return MathF.Max(0, SignedDistance(other));
    }

    /// <summary>
    /// Computes the signed distance from this circle to a given point.
    /// </summary>
    /// <param name="point">The point to compute the distance to.</param>
    /// <returns>The computed distance.</returns>
    public float SignedDistance(Vector2 point)
    {
        return SignedDistance(new Circle(point, 0));
    }

    /// <summary>
    /// Computes the signed distance from this circle to another.
    /// </summary>
    /// <param name="other">The circle to compute the distance to.</param>
    /// <returns>The computed distance.</returns>
    public float SignedDistance(Circle other)
    {
        return Vector2.Distance(this.Position, other.Position) - (this.Radius + other.Radius);
    }

    /// <summary>
    /// Indicates if this circle is equal to another.
    /// </summary>
    /// <param name="other">The circle to compare with this circle.</param>
    /// <returns><see langword="true"/> if this circle equals <paramref name="other"/>, otherwise <see langword="false"/>.</returns>
    public bool Equals(Circle other)
    {
        return this.Radius == other.Radius && this.Position == other.Position;
    }

    /// <summary>
    /// Determines if a point lies within this circle.
    /// </summary>
    /// <param name="point">The point to check the circle for.</param>
    /// <returns><see langword="true"/> if this circle contains the provided point; otherwise <see langword="false"/></returns>
    public bool ContainsPoint(Vector2 point)
    {
        return this.Intersects(new Circle(point, 0));
    }

    /// <summary>
    /// Determines if two circles are intersecting.
    /// </summary>
    /// <param name="other">The circle to check for an intersection with.</param>
    /// <returns><see langword="true"/> if the circles are intersecting; otherwise <see langword="false"/></returns>
    public bool Intersects(Circle other)
    {
        return Vector2.DistanceSquared(this.Position, other.Position) <= (this.Radius + other.Radius);
    }

    /// <inheritdoc/>
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is Circle circle && Equals(circle);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(this.Position, this.Radius);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{{{this.Position}, {this.Radius}}}";
    }

    /// <inheritdoc/>
    public static bool operator ==(Circle left, Circle right)
    {
        return left.Equals(right);
    }

    /// <inheritdoc/>
    public static bool operator !=(Circle left, Circle right)
    {
        return !(left == right);
    }

    public Vector2[] ToPolygon(int vertices)
    {
        if (vertices < 3)
            throw new ArgumentException("vertices must be >= 3!");

        var result = new Vector2[vertices];

        for (int i = 0; i < result.Length; i++)
        {
            result[i] = this.Position + this.Radius * Angle.ToVector(i / result.Length * MathF.Tau);
        }

        return result;
    }
}