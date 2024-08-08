using System.Numerics;

namespace SimulationFramework.Drawing;

/// <summary>
/// Represents a radial gradient.
/// </summary>
public sealed class RadialGradient : Gradient
{
    /// <summary>
    /// The position of the gradient.
    /// </summary>
    public Vector2 Position;

    /// <summary>
    /// The radius of the gradient.
    /// </summary>
    public float Radius;

    /// <summary>
    /// Creates a new instance of the <see cref="RadialGradient"/> class.
    /// </summary>
    /// <param name="x">The X position of the gradient.</param>
    /// <param name="y">The Y position of the gradient.</param>
    /// <param name="radius">The radius of the gradent.</param>
    /// <param name="colors">The gradient's colors.</param>
    public RadialGradient(float x, float y, float radius, params ColorF[] colors) : this(new Vector2(x, y), radius, colors) 
    { 
    }

    /// <summary>
    /// Creates a new instance of the <see cref="RadialGradient"/> class.
    /// </summary>
    /// <param name="position">The position of the gradient.</param>
    /// <param name="radius">The radius of the gradent.</param>
    /// <param name="colors">The gradient's colors.</param>
    public RadialGradient(Vector2 position, float radius, params ColorF[] colors) : this(position, radius, ColorsToStops(colors)) 
    { 
    }

    /// <summary>
    /// Creates a new instance of the <see cref="RadialGradient"/> class.
    /// </summary>
    /// <param name="x">The X position of the gradient.</param>
    /// <param name="y">The Y position of the gradient.</param>
    /// <param name="radius">The radius of the gradent.</param>
    /// <param name="stops">An array of gradient stops.</param>
    public RadialGradient(float x, float y, float radius, params GradientStop[] stops) : this(new Vector2(x, y), radius, stops)
    {
    }
    /// <summary>
    /// Creates a new instance of the <see cref="RadialGradient"/> class.
    /// </summary>
    /// <param name="position">The position of the gradient.</param>
    /// <param name="radius">The radius of the gradent.</param>
    /// <param name="stops">An array of gradient stops.</param>
    public RadialGradient(Vector2 position, float radius, params GradientStop[] stops) : this(position, radius, stops, TileMode.Clamp) 
    { 
    }

    /// <summary>
    /// Creates a new instance of the <see cref="RadialGradient"/> class.
    /// </summary>
    /// <param name="position">The position of the gradient.</param>
    /// <param name="radius">The radius of the gradent.</param>
    /// <param name="colors">Colors</param>
    /// <param name="transform">The gradient's transformation matrix.</param>
    /// <param name="tileMode">The gradients tile mode.</param>
    public RadialGradient(Vector2 position, float radius, ColorF[] colors, TileMode tileMode = TileMode.Clamp) : this(position, radius, ColorsToStops(colors), tileMode) 
    {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="RadialGradient"/> class.
    /// </summary>
    /// <param name="position">The position of the gradient.</param>
    /// <param name="radius">The radius of the gradent.</param>
    /// <param name="stops">An array of gradient stops.</param>
    /// <param name="tileMode">The gradients tile mode.</param>
    public RadialGradient(Vector2 position, float radius, GradientStop[] stops, TileMode tileMode = TileMode.Clamp) : base(stops, tileMode)
    {
        this.Position = position;
        this.Radius = radius;
    }

    public override ColorF GetPixelColor(Vector2 position)
    {
        float distance = Vector2.Distance(this.Position, position);
        float posOnGradient = distance / this.Radius;

        return GetGradientColor(posOnGradient);
    }
}