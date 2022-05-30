using System;

namespace SimulationFramework.Drawing.Canvas;

/// <summary>
/// Options for configuring the behavior of an <see cref="ICanvas"/> when drawing shapes. See <see cref="ICanvas.SetDrawMode(DrawMode)"/>.
/// </summary>
public enum DrawMode
{
    /// <summary>
    /// Shapes should be filled in completely.
    /// </summary>
    Fill,
    /// <summary>
    /// Shapes should be drawn as an outline. See <see cref="ICanvas.SetStrokeWidth(float)"/> to set the thinkness of the outline.
    /// </summary>
    Stroke,
    /// <summary>
    /// Shapes should be filled with a gradient.
    /// </summary>
    Gradient,
    /// <summary>
    /// Shapes should be filled with a texture. 
    /// </summary>
    Textured
}