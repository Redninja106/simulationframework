using System.Numerics;

namespace SimulationFramework.Drawing;

/// <summary>
/// Describes a color transition in a gradient.
/// </summary>
public struct GradientStop 
{
    public ColorF Color;
    public float Position;

    public GradientStop(ColorF color, float position)
    {
        this.Color = color;
        this.Position = position;
    }
}