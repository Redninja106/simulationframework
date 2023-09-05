using System;

namespace SimulationFramework.Drawing;

/// <summary>
/// A 2D bitmap used for rendering. Textures can be created using <see cref="Graphics.CreateTexture(int, int, TextureOptions)"/> or loaded using <see cref="Graphics.LoadTexture(string, TextureOptions)"/>.
/// </summary>
public interface ITexture : IDisposable
{
    /// <summary>
    /// The width of the texture, in pixels.
    /// </summary>
    int Width { get; }

    /// <summary>
    /// The height of the texture, in pixels.
    /// </summary>
    int Height { get; }
     
    /// <summary>
    /// A span of colors making up texture's data
    /// <para>
    /// If changes are made to the texture's data, they may not be applied until <see cref="ApplyChanges"/> is called.
    /// </para>
    /// </summary>
    Span<Color> Pixels { get; }

    /// <summary>
    /// Gets a reference to the element of <see cref="Pixels"/> at the provided <paramref name="x"/> and <paramref name="y"/> coordinates.
    /// <para>
    /// If changes are made to the texture's data, they may not be applied until <see cref="ApplyChanges"/> is called.
    /// </para>
    /// </summary>
    /// <param name="x">The x-coordinate of the pixel.</param>
    /// <param name="y">The y-coordinate of the pixel.</param>
    sealed ref Color GetPixel(int x, int y)
    {
        if (x < 0 || x >= Width)
            throw new ArgumentOutOfRangeException(nameof(x));
        
        if (y < 0 || y >= Height)
            throw new ArgumentOutOfRangeException(nameof(y));
        
        return ref Pixels[y * Width + x];
    }

    /// <summary>
    /// Gets a canvas which draws to this texture.
    /// </summary>
    ICanvas GetCanvas();

    /// <summary>
    /// Applies any cpu-side changes made do the bitmap's data using <see cref="Pixels"/> or <see cref="GetPixel(int, int)"/>.
    /// </summary>
    void ApplyChanges();
}