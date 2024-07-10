using System;
using System.Numerics;
using SimulationFramework.Drawing.Shaders;

namespace SimulationFramework.Drawing;

public class CanvasState
{
    public Matrix3x2 Transform { get; set; }
    public Rectangle? ClipRectangle { get; set; }

    public bool Fill { get; set; }
    public ColorF Color { get; set; }
    public float strokeWidth { get; set; }

    public CanvasShader? Shader { get; set; }
    public FontStyle FontStyle { get; set; }
    public float FontSize { get; set; }
    public IFont Font { get; set; }

    public CanvasState Clone()
    {
        return (CanvasState)this.MemberwiseClone();
    }

    public void Reset()
    {
        Transform = Matrix3x2.Identity;
        ClipRectangle = null;
        Fill = true;
        Color = ColorF.White;
        strokeWidth = 0;
        Shader = null;

        FontStyle = FontStyle.Normal;
        FontSize = 16;
        Font = Graphics.DefaultFont;
    }
}

enum BlendMode
{
    Alpha,
    Overwrite,
}