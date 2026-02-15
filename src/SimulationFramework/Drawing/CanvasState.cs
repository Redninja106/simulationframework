using System;
using System.Numerics;
using SimulationFramework.Drawing.Shaders;

namespace SimulationFramework.Drawing;

/// <summary>
/// Stores the drawing configuration of the canvas.
/// </summary>
public struct CanvasState
{
    /// <summary>
    /// The transform matrix applied to all rendered shapes.
    /// </summary>
    public Matrix3x2 Transform { get; set; }

    /// <summary>
    /// The screen-space clipping rectangle of the canvas. When this value is not null, rendering is restricted the specified region of the screen.
    /// </summary>
    public Rectangle? ClipRectangle { get; set; }

    /// <summary>
    /// The screen-space mask to use when rendering. 
    /// </summary>
    public IMask? Mask { get; set; }

    /// <summary>
    /// The screen-space mask to write to when rendering.
    /// </summary>
    public IMask? WriteMask { get; set; }

    /// <summary>
    /// The value to write to the screen space mask when rendering. <see langword="null"/> writes depth only (when using a depth mask).
    /// </summary>
    public bool? WriteMaskValue { get; set; }

    /// <summary>
    /// If <see langword="true"/> the canvas renders solid shapes; otherwise the canvas renders outlines.
    /// </summary>
    public bool Fill { get; set; }

    /// <summary>
    /// The color to render geometry as.
    /// </summary>
    public ColorF Color { get; set; }
    
    /// <summary>
    /// The thickness of the outlines rendered when <see cref="Fill"/> is <see langword="false"/>.
    /// </summary>
    public float StrokeWidth { get; set; }

    /// <summary>
    /// The canvas shader to use when rendering. If <see langword="null"/>, rendered shapes will be filled with <see cref="Color"/>.
    /// </summary>
    public CanvasShader? Shader { get; set; }

    /// <summary>
    /// The vertex shader to use when rendering, If <see langword="null"/>, the default vertex shader will be used.
    /// </summary>
    public VertexShader? VertexShader { get; set; }

    /// <summary>
    /// The font to use when rendering text.
    /// </summary>
    public IFont Font { get; set; }

    /// <summary>
    /// The BlendMode to use while rendering.
    /// </summary>
    public BlendMode BlendMode { get; set; }

    /// <summary>
    /// Resets this <see cref="CanvasState"/> to default settings.
    /// </summary>
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
