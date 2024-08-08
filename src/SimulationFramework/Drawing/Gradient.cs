using SimulationFramework.Drawing.Shaders;
using System.Numerics;

namespace SimulationFramework.Drawing;

/// <summary>
/// Represents a gradient.
/// </summary>
public abstract class Gradient : CanvasShader
{
    /// <summary>
    /// The gradient's stops.
    /// </summary>
    public GradientStop[] Stops;

    /// <summary>
    /// The gradient's tile mode.
    /// </summary>
    public TileMode TileMode;

    internal Gradient(GradientStop[] stops, Matrix3x2 transform, TileMode tileMode)
    {
        Stops = stops;
        TileMode = tileMode;
    }

    /// <summary>
    /// Converts an array of colors to an array of equidistant stops.
    /// </summary>
    public static GradientStop[] ColorsToStops(ColorF[] colors)
    {
        var stops = new GradientStop[colors.Length];

        for (var i = 0; i < colors.Length; i++)
        {
            stops[i] = new GradientStop(colors[i], i / (float)(colors.Length - 1));
        }

        return stops;
    }
}