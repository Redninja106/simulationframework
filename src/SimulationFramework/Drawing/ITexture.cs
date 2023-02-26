using SimulationFramework.Shaders;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing;

[ShaderIntrinsic]
public interface ITexture<T> : IResource where T : unmanaged
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

        return ref Pixels[(y * Width) + x];
    }

    /// <summary>
    /// Applies any changes made do the texture's data using <see cref="Pixels"/> or <see cref="GetPixel(int, int)"/>.
    /// </summary>
    void ApplyChanges();

    // MIPMAPS: 
    // // pass 0 for mips all the way to 1x1. size must be power of 2 in both directions
    // void GenerateMipmaps(int levels = 0);
    // 
    // // gets the mipmap above or below this one.
    // ITexture<T> GetMipmap(int mipmap);

    // ref T this[int x, int y] { get; }
}