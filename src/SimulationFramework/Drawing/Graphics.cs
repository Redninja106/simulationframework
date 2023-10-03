using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;

namespace SimulationFramework.Drawing;

/// <summary>
/// Provides graphical functionality to the application.
/// </summary>
public static class Graphics
{
    private static IGraphicsProvider Provider => Application.GetComponent<IGraphicsProvider>();

    public static IFont DefaultFont => Provider.DefaultFont;

    /// <summary>
    /// Gets canvas which draws to the current frame.
    /// </summary>
    /// <returns></returns>
    public static ICanvas GetOutputCanvas()
    {
        var interceptor = Application.GetComponentOrDefault<FixedResolutionInterceptor>();
        if (interceptor is not null)
        {
            return interceptor.FrameBuffer.GetCanvas();
        }

        return Provider.GetFrameCanvas();
    }

    public static ITexture LoadTexture(string file, TextureOptions options = TextureOptions.None)
    {
        var encodedData = File.ReadAllBytes(file);
        return LoadTexture(encodedData, options);
    }

    public static ITexture LoadTexture(byte[] encodedData, TextureOptions options = TextureOptions.None)
    {
        return LoadTexture(encodedData.AsSpan(), options);
    }

    public static ITexture LoadTexture(ReadOnlySpan<byte> encodedData, TextureOptions options = TextureOptions.None)
    {
        return TryLoadTexture(encodedData, options, out ITexture? texture) ? texture : throw new("Error loading texture!");
    }

    public static bool TryLoadTexture(string file, TextureOptions options, [NotNullWhen(true)] out ITexture? texture)
    {
        byte[] encodedData;
        try
        {
            encodedData = File.ReadAllBytes(file);
        }
        catch
        {
            texture = null;
            return false;
        }
        
        return Provider.TryLoadTexture(encodedData, options, out texture);
    }

    public static bool TryLoadTexture(byte[] encodedData, TextureOptions options, [NotNullWhen(true)] out ITexture? texture)
    {
        return TryLoadTexture(encodedData.AsSpan(), options, out texture);
    }

    public static bool TryLoadTexture(ReadOnlySpan<byte> encodedData, TextureOptions options, [NotNullWhen(true)] out ITexture? texture)
    {
        return Provider.TryLoadTexture(encodedData, options, out texture);
    }

    public static ITexture CreateTexture(int width, int height, TextureOptions options = TextureOptions.None)
    {
        return CreateTexture(width, height, ReadOnlySpan<Color>.Empty, options);
    }

    public static ITexture CreateTexture(int width, int height, Color[] colors, TextureOptions options = TextureOptions.None)
    {
        return CreateTexture(width, height, colors.AsSpan(), options);
    }

    public static ITexture CreateTexture(int width, int height, ReadOnlySpan<Color> colors, TextureOptions options = TextureOptions.None)
    {
        return TryCreateTexture(width, height, colors, options, out ITexture? texture) ? texture : throw new("Error creating texture!");
    }

    public static bool TryCreateTexture(int width, int height, TextureOptions options, [NotNullWhen(true)] out ITexture? texture)
    {
        return TryCreateTexture(width, height, ReadOnlySpan<Color>.Empty, options, out texture);
    }

    public static bool TryCreateTexture(int width, int height, Color[] pixels, TextureOptions options, [NotNullWhen(true)] out ITexture? texture)
    {
        return TryCreateTexture(width, height, pixels.AsSpan(), options, out texture);
    }

    public static bool TryCreateTexture(int width, int height, ReadOnlySpan<Color> pixels, TextureOptions options, [NotNullWhen(true)] out ITexture? texture)
    {
        return Provider.TryCreateTexture(width, height, pixels, options, out texture);
    }

    public static IFont LoadFontByName(string name)
    {
        return TryLoadSystemFont(name, out IFont? font) ? font : throw new("Error loading font.");
    }

    public static bool TryLoadSystemFont(string name, [NotNullWhen(true)] out IFont? font)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            font = null;
            return false;
        }

        return Provider.TryLoadSystemFont(name, out font);
    }

    public static IFont LoadFont(string file)
    {
        var encodedData = File.ReadAllBytes(file);
        return LoadFont(encodedData);
    }

    public static IFont LoadFont(ReadOnlySpan<byte> encodedData)
    {
        if (encodedData.IsEmpty)
            throw new ArgumentException($"{nameof(encodedData)} was empty!");

        return TryLoadFont(encodedData, out IFont? result) ? result : throw new Exception("Error loading font!");
    }

    public static bool TryLoadFont(string file, [NotNullWhen(true)] out IFont? font)
    {
        byte[] encodedData;
        try
        {
            encodedData = File.ReadAllBytes(file);
        }
        catch
        {
            font = null;
            return false;
        }

        return TryLoadFont(encodedData, out font);
    }

    public static bool TryLoadFont(ReadOnlySpan<byte> encodedData, [NotNullWhen(true)] out IFont? font)
    {
        if (encodedData.IsEmpty)
        {
            font = null;
            return false;
        }

        return Provider.TryLoadFont(encodedData, out font);
    }
}