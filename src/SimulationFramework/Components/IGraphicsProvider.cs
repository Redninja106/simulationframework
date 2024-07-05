﻿using System.Diagnostics.CodeAnalysis;
using SimulationFramework.Drawing;

namespace SimulationFramework.Components;

/// <summary>
/// Interface which provides graphics functionality as a component of a simulation.
/// </summary>
public interface IGraphicsProvider : ISimulationComponent
{
    /// <summary>
    /// Returns an instance of the default font.
    /// </summary>
    IFont DefaultFont { get; }

    /// <summary>
    /// Gets the canvas for the current frame.
    /// </summary>
    /// <returns>
    /// The canvas which draws to the current frame. This object should never be saved, as it may be different every frame.
    /// </returns>
    ICanvas GetFrameCanvas();

    /// <summary>
    /// Creates a new bitmap with the provided data.
    /// </summary>
    ITexture CreateTexture(int width, int height, ReadOnlySpan<Color> pixels, TextureOptions options);

    /// <summary>
    /// Loads a bitmap from a image file.
    /// </summary>
    ITexture LoadTexture(ReadOnlySpan<byte> encodedData, TextureOptions options);

    /// <summary>
    /// Loads a font from a font file.
    /// </summary>
    bool TryLoadFont(ReadOnlySpan<byte> encodedData, [NotNullWhen(true)] out IFont? font);

    /// <summary>
    /// Attempts to load a system font by name.
    /// </summary>
    bool TryLoadSystemFont(string name, [NotNullWhen(true)] out IFont? font);
}