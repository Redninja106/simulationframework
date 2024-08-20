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
    public WrapMode TileMode;

    public Gradient(GradientStop[] stops, WrapMode tileMode)
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

    protected ColorF GetGradientColor(float gradientProgress)
    {
        if (TileMode == WrapMode.Clamp)
        {
            gradientProgress = Math.Clamp(gradientProgress, 0, 1);
        }
        else if (TileMode == WrapMode.Repeat)
        {
            // TODO: support % operator on floats
            gradientProgress = ShaderIntrinsics.Mod(gradientProgress, 1f);
        }
        else if (TileMode == WrapMode.Mirror)
        {
            gradientProgress = ShaderIntrinsics.Mod(gradientProgress, 2f);
            if (gradientProgress >= 1)
            {
                gradientProgress = 2 - gradientProgress;
            }
        }
        else // WrapMode.None
        {
            if (gradientProgress < 0)
            {
                return ColorF.Transparent;
            }
            if (gradientProgress > 1)
            {
                return ColorF.Transparent;
            }
        }

        int i;
        for (i = 1; i < Stops.Length; i++)
        {
            if (Stops[i].Position >= gradientProgress)
            {
                break;
            }
        }

        GradientStop a = this.Stops[i - 1];
        GradientStop b = this.Stops[i];
        return ColorF.Lerp(a.Color, b.Color, (gradientProgress - a.Position) / (b.Position - a.Position));
    }
}