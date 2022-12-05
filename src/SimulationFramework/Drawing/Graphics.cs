using SimulationFramework.Serialization.PNG;
using SimulationFramework.Shaders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing;

/// <summary>
/// Provides graphical functionality to the application.
/// </summary>
public static class Graphics
{
    private static IGraphicsProvider Provider => Application.Current?.GetComponent<IGraphicsProvider>() ?? throw Exceptions.CoreComponentNotFound();

    /// <summary>
    /// Gets canvas which draws to the current frame.
    /// </summary>
    /// <returns></returns>
    public static ICanvas GetFrameCanvas()
    {
        return Provider.GetFrameCanvas();
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
    public static ITexture<Color> LoadTexture(string file, ResourceOptions options = ResourceOptions.None)
    {
        var fileData = File.ReadAllBytes(file);

        return LoadTexture(fileData, options);
    }

    /// <summary>
    /// Loads a texture from raw, encoded file data.
    /// </summary>
    /// <param name="encodedBytes">An array of the bytes of a supported image file.</param>
    /// <param name="options">A <see cref="ResourceOptions"/> value which influences the behavior of the texture.</param>
    /// <returns>The new texture.</returns>
    public static ITexture<Color> LoadTexture(byte[] encodedBytes, ResourceOptions options = ResourceOptions.None)
    {
        return LoadTexture(encodedBytes.AsSpan(), options);
    }

    /// <summary>
    /// Loads a texture from raw, encoded file data.
    /// </summary>
    /// <param name="encodedBytes">A span of the bytes of a supported image file.</param>
    /// <param name="options">A <see cref="ResourceOptions"/> value which influences the behavior of the texture.</param>
    /// <returns>The new texture.</returns>
    public static unsafe ITexture<Color> LoadTexture(Span<byte> encodedBytes, ResourceOptions options = ResourceOptions.None)
    {
        fixed (byte* encodedBytesPtr = encodedBytes)
        {
            using var stream = new UnmanagedMemoryStream(encodedBytesPtr, encodedBytes.Length);
            var decoder = new PNGDecoder(stream);

            var metadata = decoder.Metadata;

            stream.Position = 0;

            var texture = CreateTexture<Color>((int)metadata.Width, (int)metadata.Height);

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
    /// <param name="options">A <see cref="ResourceOptions"/> value which influences the behavior of the texture.</param>
    /// <returns>The new texture.</returns>
    public static ITexture<T> CreateTexture<T>(int width, int height, ResourceOptions options = ResourceOptions.None) where T : unmanaged
    {
        return Provider.CreateTexture<T>(width, height, null, options);
    }

    /// <summary>
    /// Creates a texture of the provided size and fills it with the provided colors.
    /// </summary>
    /// <param name="width">The width of the texture, in pixels.</param>
    /// <param name="height">The height of the texture, in pixels.</param>
    /// <param name="colors">The data of to fill the texture with. Must be of length <paramref name="width"/> * <paramref name="height"/>.</param>
    /// <param name="options">A <see cref="ResourceOptions"/> value which influences the behavior of the texture.</param>
    /// <returns>The new texture.</returns>
    public static ITexture<T> CreateTexture<T>(int width, int height, Span<T> colors, ResourceOptions options = ResourceOptions.None) where T : unmanaged
    {
        return Provider.CreateTexture<T>(width, height, colors, options);
    }

    /// <summary>
    /// Creates a texture of the provided size and fills it with the provided colors.
    /// </summary>
    /// <param name="width">The width of the texture, in pixels.</param>
    /// <param name="height">The height of the texture, in pixels.</param>
    /// <param name="colors">The data of to fill the texture with. Must be of length <paramref name="width"/> * <paramref name="height"/>.</param>
    /// <param name="options">A <see cref="ResourceOptions"/> value which influences the behavior of the texture.</param>
    /// <returns>The new texture.</returns>
    public static ITexture<Color> CreateTexture(int width, int height, Color[] colors, ResourceOptions options = ResourceOptions.None)
    {
        return Provider.CreateTexture(width, height, colors.AsSpan(), options);
    }

    public static IBuffer<T> CreateBuffer<T>(int size) where T : unmanaged
    {
        return Provider.CreateBuffer<T>(size, 0);
    }

    public static IBuffer<T> CreateBuffer<T>(Span<T> data) where T : unmanaged
    {
        var buffer = Provider.CreateBuffer<T>(data.Length, 0);
        buffer.SetData(data);
        return buffer;
    }

    public static void DispatchCompute<T>(T shader, int threads, IRenderer? renderer = null) where T : struct, IShader
    {
        throw new NotImplementedException();
    }

    public static void DispatchCompute<T>(T shader, int threadsX, int threadsY, int threadsZ, IRenderer? renderer = null) where T : struct, IShader
    {
        throw new NotImplementedException();
    }

    public static int GetVertexCount(PrimitiveKind kind, int primitiveCount) => kind switch
    {
        _ when primitiveCount is 0 => 0,
        PrimitiveKind.Points => primitiveCount,
        PrimitiveKind.Lines => primitiveCount * 2,
        PrimitiveKind.Triangles => primitiveCount * 3,
        PrimitiveKind.LineStrip => primitiveCount + 1,
        PrimitiveKind.TriangleStrip => primitiveCount * 2 + 1,
        _ => throw new ArgumentException(null, nameof(kind)),
    };

    public static int GetPrimitiveCount(PrimitiveKind kind, int vertexCount) => kind switch
    {
        _ when vertexCount is 0 => 0,
        PrimitiveKind.Points => vertexCount,
        PrimitiveKind.Lines => vertexCount / 2,
        PrimitiveKind.Triangles => vertexCount / 3,
        PrimitiveKind.LineStrip => vertexCount - 1,
        PrimitiveKind.TriangleStrip => (vertexCount - 1) / 2,
        _ => throw new ArgumentException(null, nameof(kind)),
    };

    public static ITexture<TTo> ReinterpretTexture<TFrom, TTo>(ITexture<TFrom> texture)
        where TFrom : unmanaged
        where TTo : unmanaged
    {
        throw new NotImplementedException();
    }

    public static IBuffer<TTo> ReinterpretBuffer<TFrom, TTo>(IBuffer<TFrom> buffer) 
        where TFrom : unmanaged 
        where TTo : unmanaged
    {
        throw new NotImplementedException();
    }

    public static void CopyTexture<T>(ITexture<T> source, ITexture<T> destination) where T : unmanaged
    {
        throw new NotImplementedException();
    }

    public static void CopyBuffer<T>(ITexture<T> source, ITexture<T> destination) where T : unmanaged
    {
        throw new NotImplementedException();
    }

    public static ITexture<Color> GetDefaultRenderTarget()
    {
        return Provider.GetDefaultRenderTarget();
    }

    public static ITexture<float> GetDefaultDepthTarget()
    {
        return Provider.GetDefaultDepthTarget();
    }

    public static ITexture<byte> GetDefaultStencilTarget()
    {
        throw new NotImplementedException();
    }
}