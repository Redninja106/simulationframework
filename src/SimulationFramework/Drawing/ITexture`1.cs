using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing;

/// <summary>
/// A 2D bitmap that can be rendered, or rendered to.
/// </summary>
public interface ITexture<T> : IDisposable where T : unmanaged
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
    Span<T> Pixels { get; }

    /// <summary>
    /// Gets a reference to the element of <see cref="Pixels"/> at the provided <paramref name="x"/> and <paramref name="y"/> coordinates.
    /// <para>
    /// If changes are made to the texture's data, they may not be applied until <see cref="ApplyChanges"/> is called.
    /// </para>
    /// </summary>
    /// <param name="x">The x-coordinate of the pixel.</param>
    /// <param name="y">The y-coordinate of the pixel.</param>
    sealed ref T GetPixel(int x, int y)
    {
        if (x < 0 || x >= this.Width)
            throw new ArgumentException(null, nameof(x));

        if (y < 0 || y >= this.Height)
            throw new ArgumentException(null, nameof(y));

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