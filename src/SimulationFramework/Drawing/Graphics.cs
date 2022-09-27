using SimulationFramework.Serialization.PNG;
using SimulationFramework.Drawing.RenderPipeline;
using SimulationFramework.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing;

/// <summary>
/// Provides graphical functionality to the application.
/// </summary>
public static class Graphics
{
    private static IGraphicsProvider Provider => Application.Current.GetComponent<IGraphicsProvider>();

    /// <summary>
    /// Gets canvas which draws to the current frame.
    /// </summary>
    /// <returns></returns>
    public static ICanvas GetFrameCanvas()
    {
        return Provider.GetFrameCanvas();
    }

    public static ITexture GetFrameTexture()
    {
        return Provider.GetFrameTexture();
    }

    public static IRenderer GetRenderer()
    {
        return Provider.GetRenderer();
    }

    // public static IShader CreateShader(ShaderKind kind, string source)
    // {
    //     return Provider.CreateShader(kind, source);
    // }

    /// <summary>
    /// Loads a texture from a file.
    /// </summary>
    /// <param name="file">The path to a .PNG image file.</param>
    /// <param name="flags">A <see cref="ResourceOptions"/> value which influences the behavior of the texture.</param>
    /// <returns>The new texture.</returns>
    public static ITexture LoadTexture(string file, ResourceOptions flags = ResourceOptions.None)
    {
        var fileData = File.ReadAllBytes(file);

        return LoadTexture(fileData, flags);
    }

    /// <summary>
    /// Loads a texture from raw, encoded file data.
    /// </summary>
    /// <param name="encodedBytes">An array of the bytes of a supported image file.</param>
    /// <param name="flags">A <see cref="ResourceOptions"/> value which influences the behavior of the texture.</param>
    /// <returns>The new texture.</returns>
    public static ITexture LoadTexture(byte[] encodedBytes, ResourceOptions flags = ResourceOptions.None)
    {
        return LoadTexture(encodedBytes.AsSpan(), flags);
    }

    /// <summary>
    /// Loads a texture from raw, encoded file data.
    /// </summary>
    /// <param name="encodedBytes">A span of the bytes of a supported image file.</param>
    /// <param name="flags">A <see cref="ResourceOptions"/> value which influences the behavior of the texture.</param>
    /// <returns>The new texture.</returns>
    public static unsafe ITexture LoadTexture(Span<byte> encodedBytes, ResourceOptions flags = ResourceOptions.None)
    {
        fixed (byte* encodedBytesPtr = encodedBytes)
        {
            using var stream = new UnmanagedMemoryStream(encodedBytesPtr, encodedBytes.Length);
            var decoder = new PNGDecoder(stream);

            var metadata = decoder.Metadata;

            stream.Position = 0;

            var texture = CreateTexture((int)metadata.Width, (int)metadata.Height);

            decoder.GetColors(texture.Pixels);
            texture.ApplyChanges();

            return texture;
        }
    }

    /// <summary>
    /// Creates a blank texture of the provided size.
    /// </summary>
    /// <param name="width">The width of the texture, in pixels.</param>
    /// <param name="height">The height of the texture, in pixels.</param>
    /// <param name="flags">A <see cref="ResourceOptions"/> value which influences the behavior of the texture.</param>
    /// <returns>The new texture.</returns>
    public static ITexture CreateTexture(int width, int height, ResourceOptions flags = ResourceOptions.None)
    {
        return Provider.CreateTexture(width, height, null, flags);
    }

    /// <summary>
    /// Creates a texture of the provided size and fills it with the provided colors.
    /// </summary>
    /// <param name="width">The width of the texture, in pixels.</param>
    /// <param name="height">The height of the texture, in pixels.</param>
    /// <param name="colors">The data of to fill the texture with. Must be of length <paramref name="width"/> * <paramref name="height"/>.</param>
    /// <param name="flags">A <see cref="ResourceOptions"/> value which influences the behavior of the texture.</param>
    /// <returns>The new texture.</returns>
    public static ITexture CreateTexture(int width, int height, Span<Color> colors, ResourceOptions flags = ResourceOptions.None)
    {
        return Provider.CreateTexture(width, height, colors, flags);
    }

    /// <summary>
    /// Creates a texture of the provided size and fills it with the provided colors.
    /// </summary>
    /// <param name="width">The width of the texture, in pixels.</param>
    /// <param name="height">The height of the texture, in pixels.</param>
    /// <param name="colors">The data of to fill the texture with. Must be of length <paramref name="width"/> * <paramref name="height"/>.</param>
    /// <param name="flags">A <see cref="ResourceOptions"/> value which influences the behavior of the texture.</param>
    /// <returns>The new texture.</returns>
    public static ITexture CreateTexture(int width, int height, Color[] colors, ResourceOptions flags = ResourceOptions.None)
    {
        return Provider.CreateTexture(width, height, colors.AsSpan(), flags);
    }

    public static IBuffer<T> CreateBuffer<T>(int size) where T : unmanaged
    {
        return Provider.CreateBuffer<T>(size, 0);
    }

    public static IBuffer<T> CreateBuffer<T>(T[] data) where T : unmanaged
    {
        var buffer = Provider.CreateBuffer<T>(data.Length, 0);
        buffer.SetData(data);
        return buffer;
    }

}