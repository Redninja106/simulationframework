using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

/// <summary>
/// Provides graphical functionality to the application.
/// </summary>
public static class Graphics
{
    private static IGraphicsProvider Provider => Simulation.Current.GetComponent<IGraphicsProvider>();

    /// <summary>
    /// Gets canvas which draws to the current frame.
    /// </summary>
    /// <returns></returns>
    public static ICanvas GetFrameCanvas()
    {
        return Provider.GetFrameCanvas();
    }

    /// <summary>
    /// Loads a texture from a file.
    /// </summary>
    /// <param name="file">The path to a .PNG image file.</param>
    /// <returns>The new texture.</returns>
    public static ITexture LoadTexture(string file)
    {
        var fileData = File.ReadAllBytes(file);

        return LoadTexture(fileData);
    }

    /// <summary>
    /// Loads a texture from raw, encoded file data.
    /// </summary>
    /// <param name="encodedBytes">An array of the bytes of a supported image file.</param>
    /// <returns>The new texture.</returns>
    public static ITexture LoadTexture(byte[] encodedBytes)
    {
        return LoadTexture(encodedBytes.AsSpan());
    }

    /// <summary>
    /// Loads a texture from raw, encoded file data.
    /// </summary>
    /// <param name="encodedBytes">A span of the bytes of a supported image file.</param>
    /// <returns>The new texture.</returns>
    public static ITexture LoadTexture(Span<byte> encodedBytes)
    {
        return Provider.LoadTexture(encodedBytes);
    }

    /// <summary>
    /// Creates a blank texture of the provided size.
    /// </summary>
    /// <param name="width">The width of the texture, in pixels.</param>
    /// <param name="height">The height of the texture, in pixels.</param>
    /// <returns>The new texture.</returns>
    public static ITexture CreateTexture(int width, int height)
    {
        return Provider.CreateTexture(width, height, null);
    }

    /// <summary>
    /// Creates a texture of the provided size and fills it with the provided colors.
    /// </summary>
    /// <param name="width">The width of the texture, in pixels.</param>
    /// <param name="height">The height of the texture, in pixels.</param>
    /// <param name="colors">The data of to fill the texture with. Must be of length <paramref name="width"/> * <paramref name="height"/>.</param>
    /// <returns>The new texture.</returns>
    public static ITexture CreateTexture(int width, int height, Span<Color> colors)
    {
        return Provider.CreateTexture(width, height, colors);
    }

    /// <summary>
    /// Creates a texture of the provided size and fills it with the provided colors.
    /// </summary>
    /// <param name="width">The width of the texture, in pixels.</param>
    /// <param name="height">The height of the texture, in pixels.</param>
    /// <param name="colors">The data of to fill the texture with. Must be of length <paramref name="width"/> * <paramref name="height"/>.</param>
    /// <returns>The new texture.</returns>
    public static ITexture CreateTexture(int width, int height, Color[] colors)
    {
        return Provider.CreateTexture(width, height, colors.AsSpan());
    }

    /// <summary>
    /// Clears all cached fonts.
    /// </summary>
    public static void ClearFontCache() => Provider.ClearFontCache();
}