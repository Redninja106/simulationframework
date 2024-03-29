﻿using System.Numerics;

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

    /// <summary>
    /// Creates a new instance of the <see cref="LinearGradient"/> class.
    /// </summary>
    /// <param name="fromX">The X coordinate of the gradient's starting point.</param>
    /// <param name="fromY">The Y coordinate of the gradient's starting point.</param>
    /// <param name="toX">The X coordinate of the gradient's end point.</param>
    /// <param name="toY">The Y coordinate of the gradient's end point.</param>
    /// <param name="colors">An array of colors.</param>
    public LinearGradient(float fromX, float fromY, float toX, float toY, params Color[] colors) : this(new Vector2(fromX, fromY), new Vector2(toX, toY), colors) 
    { 
    }

    /// <summary>
    /// Creates a new instance of the <see cref="LinearGradient"/> class.
    /// </summary>
    /// <param name="from">The starting point of the gradient.</param>
    /// <param name="to">The end point of the gradient.</param>
    /// <param name="colors">An array of colors.</param>
    public LinearGradient(Vector2 from, Vector2 to, params Color[] colors) : this(from, to, ColorsToStops(colors)) 
    { 
    }

    /// <summary>
    /// Creates a new instance of the <see cref="LinearGradient"/> class.
    /// </summary>
    /// <param name="fromX">The X coordinate of the gradient's starting point.</param>
    /// <param name="fromY">The Y coordinate of the gradient's starting point.</param>
    /// <param name="toX">The X coordinate of the gradient's end point.</param>
    /// <param name="toY">The Y coordinate of the gradient's end point.</param>
    /// <param name="stops">An array of gradient stops.</param>
    public LinearGradient(float fromX, float fromY, float toX, float toY, params GradientStop[] stops) : this(new Vector2(fromX, fromY), new Vector2(toX, toY), stops) 
    { 
    }

    /// <summary>
    /// Creates a new instance of the <see cref="LinearGradient"/> class.
    /// </summary>
    /// <param name="from">The starting point of the gradient.</param>
    /// <param name="to">The end point of the gradient.</param>
    /// <param name="stops">An array of gradient stops.</param>
    public LinearGradient(Vector2 from, Vector2 to, params GradientStop[] stops) : this(from, to, stops, Matrix3x2.Identity) 
    { 
    }

    /// <summary>
    /// Creates a new instance of the <see cref="LinearGradient"/> class.
    /// </summary>
    /// <param name="from">The starting point of the gradient.</param>
    /// <param name="to">The end point of the gradient.</param>
    /// <param name="colors">An array of colors.</param>
    /// <param name="transform">The gradient's transformation matrix.</param>
    /// <param name="tileMode">The gradients tile mode.</param>
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
