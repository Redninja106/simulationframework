﻿using System;

namespace SimulationFramework;

/// <summary>
/// Options for configuring the behavior of an <see cref="ICanvas"/> when drawing shapes. See <see cref="ICanvas.DrawMode"/>.
/// </summary>
public enum DrawMode
{
    /// <summary>
    /// Shapes should be filled in completely.
    /// </summary>
    Fill,
    /// <summary>
    /// Shapes should be outlined. See <see cref="ICanvas.StrokeWidth"/> to set the thinkness of the outline.
    /// </summary>
    Stroke,
    /// <summary>
    /// Shapes should be filled with a gradient.
    /// </summary>
    FillGradient,
    /// <summary>
    /// Shapes should be outlined with a gradient.
    /// </summary>
    StrokeGradient,
    /// <summary>
    /// Shapes should be filled with a texture. 
    /// </summary>
    Textured
}