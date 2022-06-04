using SimulationFramework.Drawing.Canvas;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing;

/// <summary>
/// A 2D bitmap that can be rendered, or rendered to.
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
    /// Opens a new canvas which draws to this texture.
    /// </summary>
    /// <returns>An <see cref="ICanvas"/> which draws onto this texture.</returns>
    ICanvas OpenCanvas();

    /// <summary>
    /// Applies any changes made do the bitmap's data using <see cref="Pixels"/> or <see cref="GetPixel(int, int)"/>.
    /// </summary>
    void ApplyChanges();
}