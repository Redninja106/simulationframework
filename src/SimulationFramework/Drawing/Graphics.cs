﻿using System.Diagnostics.CodeAnalysis;
using System.IO;
using SimulationFramework.Components;
using SimulationFramework.Drawing.Shaders;

namespace SimulationFramework.Drawing;

/// <summary>
/// Provides graphical functionality to the application.
/// </summary>
public static class Graphics
{
    private static IGraphicsProvider Provider => Application.GetComponent<IGraphicsProvider>();
    private static IFullscreenProvider FullscreenProvider => Application.GetComponent<IFullscreenProvider>();

    /// <summary>
    /// The default simulation font.
    /// </summary>
    public static IFont DefaultFont => Provider.DefaultFont;

    /// <summary>
    /// The swap interval used when presenting frames.
    /// </summary>
    public static int SwapInterval 
    { 
        get => FullscreenProvider.SwapInterval;
        set => FullscreenProvider.SwapInterval = value;
    }

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

        return Provider.GetWindowCanvas();
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
        return Provider.LoadTexture(encodedData, options);
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
        return Provider.CreateTexture(width, height, colors, options); ;
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

        return Provider.LoadSystemFont(name);
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

        return Provider.LoadFont(encodedData);
    }

    public static void Dispatch(ComputeShader computeShader, int lengthI, int lengthJ, int lengthK)
    {
        Provider.Dispatch(computeShader, lengthI, lengthJ, lengthK);
    }

    //public static IGeometry CreateGeometry<TVertex>(ReadOnlySpan<TVertex> vertices)
    //    where TVertex : unmanaged
    //{
    //    return Provider.CreateGeometry(vertices, []);
    //}

    //public static IGeometry CreateGeometry<TVertex>(ReadOnlySpan<TVertex> vertices, ReadOnlySpan<uint> indices)
    //    where TVertex : unmanaged
    //{
    //    return Provider.CreateGeometry(vertices, indices);
    //}

    public static IMask CreateMask(int width, int height)
    {
        return Provider.CreateMask(width, height);
    }

    public static IDepthMask CreateDepthMask(int width, int height)
    {
        return Provider.CreateDepthMask(width, height);
    }

    public static void CopyToGPU<T>(T[] array) { }
    public static void CopyFromGPU<T>(T[] array) { }
}