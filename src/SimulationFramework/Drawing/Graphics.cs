using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace SimulationFramework.Drawing;

/// <summary>
/// Provides graphical functionality to the application.
/// </summary>
public static class Graphics
{
    private static IGraphicsProvider Provider => Application.GetComponent<IGraphicsProvider>();

    /// <summary>
    /// The default simulation font.
    /// </summary>
    public static IFont DefaultFont => Provider.DefaultFont;

    /// <summary>
    /// Gets window canvas for the current frame.
    /// </summary>
    public static ICanvas GetOutputCanvas()
    {
        var interceptor = Application.GetComponentOrDefault<FixedResolutionInterceptor>();
        if (interceptor is not null)
        {
            return interceptor.FrameBuffer.GetCanvas();
        }

        return Provider.GetFrameCanvas();
    }

    /// <summary>
    /// Loads a texture from a file.
    /// </summary>
    /// <param name="path">The path to the image file.</param>
    /// <param name="options">Options that configure the allowed behaviors of the texture.</param>
    /// <returns>The texture loaded from the file.</returns>
    public static ITexture LoadTexture(string path, TextureOptions options = TextureOptions.None)
    {
        var encodedData = File.ReadAllBytes(path);
        return LoadTexture(encodedData, options);
    }

    /// <summary>
    /// Loads a texture from an encoded image.
    /// </summary>
    /// <param name="encodedData">The encoded image bytes.</param>
    /// <param name="options">Options that configure the allowed behaviors of the texture.</param>
    /// <returns>The texture loaded from the encoded image.</returns>
    public static ITexture LoadTexture(byte[] encodedData, TextureOptions options = TextureOptions.None)
    {
        return LoadTexture(encodedData.AsSpan(), options);
    }

    /// <summary>
    /// Loads a texture from an encoded image.
    /// </summary>
    /// <param name="encodedData">The encoded image bytes.</param>
    /// <param name="options">Options that configure the allowed behaviors of the texture.</param>
    /// <returns>The texture loaded from the encoded image.</returns>
    public static ITexture LoadTexture(ReadOnlySpan<byte> encodedData, TextureOptions options = TextureOptions.None)
    {
        return Provider.TryLoadTexture(encodedData, options, out ITexture? texture) ? texture : throw new("Error loading texture!");
    }

    /// <summary>
    /// Creates a new texture without any inital data.
    /// </summary>
    /// <param name="width">The width of the texture, in pixels.</param>
    /// <param name="height">The height of the texture, in pixels.</param>
    /// <param name="options">Options that configure the allowed behaviors of the texture.</param>
    /// <returns>The newly created texture.</returns>
    public static ITexture CreateTexture(int width, int height, TextureOptions options = TextureOptions.None)
    {
        return CreateTexture(width, height, ReadOnlySpan<Color>.Empty, options);
    }

    /// <summary>
    /// Creates a new texture with the provided data.
    /// </summary>
    /// <param name="width">The width of the texture.</param>
    /// <param name="height">The height of the texture.</param>
    /// <param name="colors">The initial colors of the texture. Must have exactly <paramref name="width"/> * <paramref name="height"/> elements. If this value is null, the texture will be blank.</param>
    /// <param name="options">Options that configure the allowed behaviors of the texture.</param>
    /// <returns>The newly created texture.</returns>
    public static ITexture CreateTexture(int width, int height, Color[]? colors, TextureOptions options = TextureOptions.None)
    {
        return CreateTexture(width, height, colors is null ? ReadOnlySpan<Color>.Empty : colors.AsSpan(), options);
    }

    /// <summary>
    /// Creates a new texture with the provided data.
    /// </summary>
    /// <param name="width">The width of the texture.</param>
    /// <param name="height">The height of the texture.</param>
    /// <param name="colors">The initial colors of the texture. Must have exactly <paramref name="width"/> * <paramref name="height"/> elements. If this value is null, the texture will be blank.</param>
    /// <param name="options">Options that configure the allowed behaviors of the texture.</param>
    /// <returns>The newly created texture.</returns>
    public static ITexture CreateTexture(int width, int height, ReadOnlySpan<Color> colors, TextureOptions options = TextureOptions.None)
    {
        return Provider.TryCreateTexture(width, height, colors, options, out ITexture? texture) ? texture : throw new("Error creating texture!");
    }

    /// <summary>
    /// Loads a pre-existing font from the operating system. 
    /// <para>
    /// If the font is not present, this method will throw an exception.
    /// </para>
    /// </summary>
    /// <param name="name">The name of the font to load.</param>
    /// <returns>The loaded font.</returns>
    public static IFont LoadSystemFont(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(null, nameof(name));
        }

        return Provider.TryLoadSystemFont(name, out IFont? font) ? font : throw new("Error loading font.");
    }

    /// <summary>
    /// Attempts to load a pre-existing font from the operating system. 
    /// </summary>
    /// <param name="name">The name of the font to load.</param>
    /// <param name="font">The loaded font. Will be non-null if this method returns true.</param>
    /// <returns><see langword="true"/> if the font was loaded; otherwise <see langword="false"/>.</returns>
    public static bool TryLoadSystemFont(string name, [NotNullWhen(true)] out IFont? font)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            font = null;
            return false;
        }

        return Provider.TryLoadSystemFont(name, out font);
    }

    /// <summary>
    /// Loads a font from a file.
    /// </summary>
    /// <param name="path">The path to the font file.</param>
    /// <returns>The loaded font.</returns>
    public static IFont LoadFont(string path)
    {
        var encodedData = File.ReadAllBytes(path);
        return LoadFont(encodedData);
    }

    /// <summary>
    /// Loads a font from encoded font file.
    /// </summary>
    /// <param name="encodedData">The content of the font file.</param>
    /// <returns>The loaded font.</returns>
    public static IFont LoadFont(ReadOnlySpan<byte> encodedData)
    {
        if (encodedData.IsEmpty)
            throw new ArgumentException($"{nameof(encodedData)} was empty!");

        return Provider.TryLoadFont(encodedData, out IFont? result) ? result : throw new Exception("Error loading font!");
    }
}