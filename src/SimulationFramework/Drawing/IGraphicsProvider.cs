using SimulationFramework.Shaders;
using System;

namespace SimulationFramework.Drawing;

/// <summary>
/// Interface which provides graphics functionality as a component of a simulation.
/// </summary>
public interface IGraphicsProvider : IAppComponent
{
    ICanvas GetFrameCanvas();

    /// <summary>
    /// Gets the canvas for the current frame.
    /// </summary>
    /// <returns>
    /// The canvas which draws to the current frame. This object should never be saved, as it may be different every frame.
    /// </returns>
    ITexture GetFrameTexture();

    /// <summary>
    /// Creates a new bitmap with the provided data.
    /// </summary>
    /// <param name="width">The width of the bitmap, in pixels.</param>
    /// <param name="height">The height of the bitmap, in pixels.</param>
    /// <param name="data">The initial raw bitmap data. Must be of length <paramref name="width"/> * <paramref name="height"/>.</param>
    /// <param name="flags"></param>
    /// <returns>The new <see cref="ITexture"/>.</returns>
    ITexture CreateTexture(int width, int height, Span<Color> data, ResourceOptions flags);

    /// <summary>
    /// Loads a bitmap from it's raw encoded data.
    /// </summary>
    /// <param name="encodedData"></param>
    /// <param name="flags"></param>
    /// <returns></returns>
    ITexture LoadTexture(Span<byte> encodedData, ResourceOptions flags);

    IBuffer<T> CreateBuffer<T>(int size, ResourceOptions flags) where T : unmanaged;
    IVolume<T> CreateVolume<T>(int length, int width, int height, ResourceOptions options) where T : unmanaged;

    // gets the main renderer
    IRenderer GetRenderer();

    void InvalidateShader(Type shaderType);
}