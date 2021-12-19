using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

/// <summary>
/// A 2D bitmap that can be rendered, or rendered to.
/// </summary>
public interface ISurface : IDisposable
{
    /// <summary>
    /// The width of the surface, in pixels.
    /// </summary>
    int Width { get; }

    /// <summary>
    /// The height of the surface, in pixels.
    /// </summary>
    int Height { get; }

    /// <summary>
    /// Opens a new canvas which draws to this surface.
    /// </summary>
    /// <returns>An <see cref="ICanvas"/> which draws onto this surface.</returns>
    ICanvas OpenCanvas();

    /// <summary>
    /// Gets the data of this surface.
    /// </summary>
    Span<Color> GetPixels();
}