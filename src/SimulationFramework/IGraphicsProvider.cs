using System;

namespace SimulationFramework;

public interface IGraphicsProvider : ISimulationComponent
{
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
    /// <param name="width">The width of the bitmap, in pixels.</param>
    /// <param name="height">The height of the bitmap, in pixels.</param>
    /// <param name="data">The initial raw bitmap data. Must be of length <paramref name="width"/> * <paramref name="height"/>.</param>
    /// <returns>The new <see cref="ITexture"/>.</returns>
    ITexture CreateTexture(int width, int height, Span<Color> data);

    /// <summary>
    /// Loads a bitmap from it's raw encoded data.
    /// </summary>
    /// <param name="encodedData"></param>
    /// <returns></returns>
    ITexture LoadTexture(Span<byte> encodedData);

    /// <summary>
    /// Clears all cached fonts.
    /// </summary>
    void ClearFontCache();
}