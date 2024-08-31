using System;
using System.Numerics;
using SimulationFramework.Drawing.Shaders;

namespace SimulationFramework.Drawing;

public struct CanvasState
{
    public Matrix3x2 Transform { get; set; }
    public Rectangle? ClipRectangle { get; set; }

    public IMask? Mask { get; set; }

    public IMask? WriteMask { get; set; }
    public bool WriteMaskValue { get; set; }

    public bool Fill { get; set; }
    public ColorF Color { get; set; }
    public float StrokeWidth { get; set; }

    public CanvasShader? Shader { get; set; }
    public VertexShader? VertexShader { get; set; }
    public IFont Font { get; set; }

    public CanvasState Clone()
    {
        return (CanvasState)this.MemberwiseClone();
    }

    public void Reset()
    {
        Transform = Matrix3x2.Identity;
        ClipRectangle = null;
        Mask = null;
        WriteMask = null;
        WriteMaskValue = true;

        Fill = true;
        Color = ColorF.White;
        StrokeWidth = 0;

        Shader = null;
        VertexShader = null;

        Font = Graphics.DefaultFont;
    }
}

enum BlendMode
{
    Alpha,
    Overwrite,
}