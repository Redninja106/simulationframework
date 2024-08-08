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

    public Gradient(GradientStop[] stops, TileMode tileMode)
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
        if (TileMode == TileMode.Clamp)
        {
            gradientProgress = Math.Clamp(gradientProgress, 0, 1);
        }
        else if (TileMode == TileMode.Repeat)
        {
            // TODO: support % operator on floats
            gradientProgress = ShaderIntrinsics.Mod(gradientProgress, 1f);
        }
        else if (TileMode == TileMode.Mirror)
        {
            gradientProgress = ShaderIntrinsics.Mod(gradientProgress, 2f);
            if (gradientProgress >= 1)
            {
                gradientProgress = 2 - gradientProgress;
            }
        }
        else if (TileMode == TileMode.None)
        {

        }

        int index = 0;
        for (int i = 1; i < Stops.Length; i++)
        {
            if (Stops[i].Position >= gradientProgress)
            {
                index = i;
                i = Stops.Length;
            }
        }

        GradientStop a = this.Stops[index - 1];
        GradientStop b = this.Stops[index];
        return ColorF.Lerp(a.Color, b.Color, (gradientProgress - a.Position) / (b.Position - a.Position));
    }
}