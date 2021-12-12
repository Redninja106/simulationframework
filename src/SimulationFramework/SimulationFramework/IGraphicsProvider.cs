using System;

namespace SimulationFramework;

public interface IGraphicsProvider
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
    /// <param name="data">The initial bitmap data. Must be of length <paramref name="width"/> * <paramref name="height"/>.</param>
    /// <returns>The new <see cref="IBitmap"/>.</returns>
    IBitmap CreateBitmap(int width, int height, Span<Color> data);
    
    ISurface LoadSurface(Span<byte> encodedData);
}