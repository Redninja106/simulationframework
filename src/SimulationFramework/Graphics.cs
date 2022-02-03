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
    /// <param name="flags">A <see cref="TextureFlags"/> value which influences the behavior of the texture.</param>
    /// <returns>The new texture.</returns>
    public static ITexture LoadTexture(string file, TextureFlags flags = TextureFlags.None)
    {
        var fileData = File.ReadAllBytes(file);

        return LoadTexture(fileData, flags);
    }

    /// <summary>
    /// Loads a texture from raw, encoded file data.
    /// </summary>
    /// <param name="encodedBytes">An array of the bytes of a supported image file.</param>
    /// <param name="flags">A <see cref="TextureFlags"/> value which influences the behavior of the texture.</param>
    /// <returns>The new texture.</returns>
    public static ITexture LoadTexture(byte[] encodedBytes, TextureFlags flags = TextureFlags.None)
    {
        return LoadTexture(encodedBytes.AsSpan(), flags);
    }

    /// <summary>
    /// Loads a texture from raw, encoded file data.
    /// </summary>
    /// <param name="encodedBytes">A span of the bytes of a supported image file.</param>
    /// <param name="flags">A <see cref="TextureFlags"/> value which influences the behavior of the texture.</param>
    /// <returns>The new texture.</returns>
    public static ITexture LoadTexture(Span<byte> encodedBytes, TextureFlags flags = TextureFlags.None)
    {
        return Provider.LoadTexture(encodedBytes, flags);
    }

    /// <summary>
    /// Creates a blank texture of the provided size.
    /// </summary>
    /// <param name="width">The width of the texture, in pixels.</param>
    /// <param name="height">The height of the texture, in pixels.</param>
    /// <param name="flags">A <see cref="TextureFlags"/> value which influences the behavior of the texture.</param>
    /// <returns>The new texture.</returns>
    public static ITexture CreateTexture(int width, int height, TextureFlags flags = TextureFlags.None)
    {
        return Provider.CreateTexture(width, height, null, flags);
    }

    /// <summary>
    /// Creates a texture of the provided size and fills it with the provided colors.
    /// </summary>
    /// <param name="width">The width of the texture, in pixels.</param>
    /// <param name="height">The height of the texture, in pixels.</param>
    /// <param name="colors">The data of to fill the texture with. Must be of length <paramref name="width"/> * <paramref name="height"/>.</param>
    /// <param name="flags">A <see cref="TextureFlags"/> value which influences the behavior of the texture.</param>
    /// <returns>The new texture.</returns>
    public static ITexture CreateTexture(int width, int height, Span<Color> colors, TextureFlags flags = TextureFlags.None)
    {
        return Provider.CreateTexture(width, height, colors, flags);
    }

    /// <summary>
    /// Creates a texture of the provided size and fills it with the provided colors.
    /// </summary>
    /// <param name="width">The width of the texture, in pixels.</param>
    /// <param name="height">The height of the texture, in pixels.</param>
    /// <param name="colors">The data of to fill the texture with. Must be of length <paramref name="width"/> * <paramref name="height"/>.</param>
    /// <param name="flags">A <see cref="TextureFlags"/> value which influences the behavior of the texture.</param>
    /// <returns>The new texture.</returns>
    public static ITexture CreateTexture(int width, int height, Color[] colors, TextureFlags flags = TextureFlags.None)
    {
        return Provider.CreateTexture(width, height, colors.AsSpan(), flags);
    }

    /// <summary>
    /// Clears all cached fonts.
    /// </summary>
    public static void ClearFontCache() => Provider.ClearFontCache();
}