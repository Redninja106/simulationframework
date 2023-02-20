using SimulationFramework.Serialization.PNG;
using SimulationFramework.Shaders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
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

    public static IGraphicsQueue ImmediateQueue => Provider.ImmediateQueue;

    public static GraphicsCapabilities Capabilities => Provider.Capabilities;

    public static IRenderer CreateRenderer(IGraphicsQueue? queue = null)
    {
        queue ??= ImmediateQueue;

        return Provider.CreateRenderer(queue);
    }

    public static IGraphicsQueue CreateDeferredQueue()
    {
        return Provider.CreateDeferredQueue();
    }

    public static ICanvas CreateCanvas(IGraphicsQueue? queue = null)
    {
        queue ??= ImmediateQueue;

        throw new NotImplementedException();
    }

    public static IGeometry LoadGeometry(string path)
    {
        throw new NotImplementedException();
    }

    public static IGeometry LoadGeometry(byte[] encodedBytes)
    {
        throw new NotImplementedException();
    }

    public static IGeometry CreateGeometry<T>(VertexData<T> data)
        where T : unmanaged, IVertex
    {
        throw new NotImplementedException();
    }

    public static IGeometry CreateGeometry(PrimitiveKind kind, Span<Vector2> vertices)
    {
        throw new NotImplementedException();
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

    public static void DispatchComputeShader(IShader? shader, int groups, IGraphicsQueue? queue = null)
    {
        queue ??= ImmediateQueue;

        DispatchComputeShader(shader, groups, 1, 1, queue);
    }

    public static void DispatchComputeShader(IShader? shader, int groupsX, int groupsY, IGraphicsQueue? queue = null)
    {
        queue ??= ImmediateQueue;

        DispatchComputeShader(shader, groupsX, groupsY, 1, queue);
    }

    public static void DispatchComputeShader(IShader? shader, int groupsX, int groupsY, int groupsZ, IGraphicsQueue? queue = null)
    {
        queue ??= ImmediateQueue;

        Provider.DispatchComputeShader(shader, groupsX, groupsY, groupsZ, queue);
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

    public static void CopyTexture<T>(ITexture<T> source, ITexture<T> destination, IGraphicsQueue? queue = null) where T : unmanaged
    {
        throw new NotImplementedException();
    }

    public static void CopyTexture<T>(ITexture<T> source, Rectangle sourceRect, ITexture<T> destination, Rectangle destinationRect, IGraphicsQueue? queue = null) where T : unmanaged
    {
        int sourceWidth = (int)sourceRect.Width;
        int sourceHeight = (int)sourceRect.Height;

        int destinationWidth = (int)destinationRect.Width;
        int destinationHeight = (int)destinationRect.Height;

        if (sourceWidth != destinationWidth || sourceHeight != destinationHeight)
            throw new ArgumentException();

        throw new NotImplementedException();
    }

    public static void CopyBuffer<T>(IBuffer<T> source, IBuffer<T> destination, IGraphicsQueue? queue = null) where T : unmanaged
    {
        throw new NotImplementedException();
    }

    public static void CopyBuffer<T>(IBuffer<T> source, Range sourceRange, IBuffer<T> destination, Range destinationRange, IGraphicsQueue? queue = null) where T : unmanaged
    {
        var (_, sourceLength) = sourceRange.GetOffsetAndLength(source.Length);
        var (_, destinationLength) = sourceRange.GetOffsetAndLength(source.Length);

        if (sourceLength != destinationLength)
            throw new ArgumentException();

        throw new NotImplementedException();
    }

    public static void CopyVolume<T>(IVolume<T> source, IVolume<T> destination, IGraphicsQueue? queue = null) where T : unmanaged
    {
        throw new NotImplementedException();
    }

    public static void CopyVolume<T>(IVolume<T> source, Box sourceBox, IVolume<T> destination, Box destinationBox, IGraphicsQueue? queue = null) where T : unmanaged
    {
        int sourceWidth = (int)sourceBox.Width;
        int sourceHeight = (int)sourceBox.Height;
        int sourceDepth = (int)sourceBox.Depth;

        int destinationWidth = (int)destinationBox.Width;
        int destinationHeight = (int)destinationBox.Height;
        int destinationDepth = (int)destinationBox.Depth;

        if (sourceWidth != destinationWidth || sourceHeight != destinationHeight || sourceDepth != destinationHeight)
            throw new ArgumentException();

        throw new NotImplementedException();
    }


    public static ITexture<Color> DefaultRenderTarget => Provider.GetDefaultRenderTarget();

    public static ITexture<float> DefaultDepthTarget => Provider.GetDefaultDepthTarget();

    public static ITexture<byte> GetDefaultStencilTarget()
    {
        throw new NotImplementedException();
    }
}