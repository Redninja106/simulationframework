using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing;

/// <summary>
/// Represents a linear gradient.
/// </summary>
public sealed class LinearGradient : Gradient
{
    /// <summary>
    /// The gradients starting point.
    /// </summary>
    public Vector2 From { get; set; }

    /// <summary>
    /// The gradient's end point.
    /// </summary>
    public Vector2 To { get; set; }

    /// <inheritdoc cref="LinearGradient(Vector2, Vector2, Color[], Matrix3x2, TileMode)"/>
    /// <param name="fromX">The X coordinate of the gradient's starting point.</param>
    /// <param name="fromY">The Y coordinate of the gradient's starting point.</param>
    /// <param name="toX">The X coordinate of the gradient's end point.</param>
    /// <param name="toY">The Y coordinate of the gradient's end point.</param>
    public LinearGradient(float fromX, float fromY, float toX, float toY, params Color[] colors) : this(new Vector2(fromX, fromY), new Vector2(toX, toY), colors) 
    { 
    }

    /// <inheritdoc cref="LinearGradient(Vector2, Vector2, Color[], Matrix3x2, TileMode)"/>
    public LinearGradient(Vector2 from, Vector2 to, params Color[] colors) : this(from, to, ColorsToStops(colors)) 
    { 
    }

    /// <inheritdoc cref="LinearGradient(Vector2, Vector2, GradientStop[], Matrix3x2, TileMode)" />
    /// <param name="fromX">The X coordinate of the gradient's starting point.</param>
    /// <param name="fromY">The Y coordinate of the gradient's starting point.</param>
    /// <param name="toX">The X coordinate of the gradient's end point.</param>
    /// <param name="toY">The Y coordinate of the gradient's end point.</param>
    public LinearGradient(float fromX, float fromY, float toX, float toY, params GradientStop[] stops) : this(new Vector2(fromX, fromY), new Vector2(toX, toY), stops) 
    { 
    }

    /// <inheritdoc cref="LinearGradient(Vector2, Vector2, GradientStop[], Matrix3x2, TileMode)" />
    public LinearGradient(Vector2 from, Vector2 to, params GradientStop[] stops) : this(from, to, stops, Matrix3x2.Identity) 
    { 
    }

    /// <inheritdoc cref="LinearGradient(Vector2, Vector2, GradientStop[], Matrix3x2, TileMode)" />
    public LinearGradient(Vector2 from, Vector2 to, Color[] colors, Matrix3x2 transform, TileMode tileMode = TileMode.Clamp) : this(from, to, ColorsToStops(colors), transform, tileMode) 
    { 
    }

    /// <summary>
    /// Creates a new instance of the <see cref="LinearGradient"/> class.
    /// </summary>
    /// <param name="from">The starting point of the gradient.</param>
    /// <param name="to">The end point of the gradient.</param>
    /// <param name="stops">An array of gradient stops.</param>
    /// <param name="transform">The gradient's transformation matrix.</param>
    /// <param name="tileMode">The gradients tile mode.</param>
    public LinearGradient(Vector2 from, Vector2 to, GradientStop[] stops, Matrix3x2 transform, TileMode tileMode = TileMode.Clamp) : base(stops, transform, tileMode)
    {
        this.From = from;
        this.To = to;
    }
}
