﻿using SimulationFramework.Shaders;
using System;

namespace SimulationFramework.Drawing;

/// <summary>
/// Interface which provides graphics functionality as a component of a simulation.
/// </summary>
public interface IGraphicsProvider : IApplicationComponent
{
    ICanvas GetFrameCanvas();

    /// <summary>
    /// Gets the canvas for the current frame.
    /// </summary>
    /// <returns>
    /// The canvas which draws to the current frame. This object should never be saved, as it may be different every frame.
    /// </returns>
    ITexture<Color> GetDefaultRenderTarget();
    ITexture<float> GetDefaultDepthTarget();

    /// <summary>
    /// Creates a new bitmap with the provided data.
    /// </summary>
    /// <param name="width">The width of the bitmap, in pixels.</param>
    /// <param name="height">The height of the bitmap, in pixels.</param>
    /// <param name="data">The initial raw bitmap data. Must be of length <paramref name="width"/> * <paramref name="height"/>.</param>
    /// <param name="flags"></param>
    /// <returns>The new <see cref="ITexture"/>.</returns>
    ITexture<T> CreateTexture<T>(int width, int height, Span<T> data, ResourceOptions flags) where T : unmanaged;

    /// <summary>
    /// Loads a bitmap from it's raw encoded data.
    /// </summary>
    /// <param name="encodedData"></param>
    /// <param name="flags"></param>
    /// <returns></returns>
    ITexture<Color> LoadTexture(Span<byte> encodedData, ResourceOptions flags);

    IBuffer<T> CreateBuffer<T>(int size, ResourceOptions flags) where T : unmanaged;

    // gets the main renderer
    IRenderer GetRenderer();

    void InvalidateShader(Type shaderType);
}