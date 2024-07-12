using SimulationFramework.Drawing;
using SkiaSharp;
using Silk.NET.OpenGL;
using System.Runtime.InteropServices;
using System.IO;

namespace SimulationFramework.SkiaSharp;

internal sealed class SkiaTexture : SkiaGraphicsObject, ITexture
{
    private readonly SkiaGraphicsProvider provider;
    public int Width { get; }
    public int Height { get; }
    public TextureOptions Options { get; }

    private readonly Color[] colors;
    private bool pixelsDirty;

    private readonly uint glTexture;
    private readonly SKSurface surface;
    private readonly SkiaCanvas canvas;
    private readonly GRBackendTexture backendTexture;
    private SKImage image;

    public unsafe SkiaTexture(SkiaGraphicsProvider provider, int width, int height, TextureOptions options)
    {
        this.provider = provider;
        this.Width = width;
        this.Height = height;
        this.Options = options;

        var gl = provider.gl;
        glTexture = gl.GenTexture();
        this.colors = new Color[width * height];
        gl.BindTexture(GLEnum.Texture2D, glTexture);
        gl.TexImage2D<Color>(GLEnum.Texture2D, 0, (int)SizedInternalFormat.Rgba8, (uint)width, (uint)height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, this.colors.AsSpan());
        gl.TexParameterI(GLEnum.Texture2D, TextureParameterName.TextureMinFilter, (uint)GLEnum.Nearest);

        backendTexture = new GRBackendTexture(width, height, false, new((uint)GLEnum.Texture2D, glTexture, (uint)SizedInternalFormat.Rgba8));

        surface = SKSurface.Create(provider.backendContext, backendTexture, SKColorType.Rgba8888);
        canvas = new(provider, this, surface.Canvas, true);

        image = SKImage.FromTexture(provider.backendContext, backendTexture, SKColorType.Rgba8888);

        if (!options.HasFlag(TextureOptions.Constant))
        {
            UpdateLocalPixels();
        }
    }

    public unsafe Span<Color> Pixels
    {
        get
        {
            if (colors is null)
                throw new InvalidOperationException("CPU access is not allowed on this texture!");
            
            if (pixelsDirty)
                UpdateLocalPixels();
            
            return colors;
        }
    }

    public void InvalidatePixels()
    {
        pixelsDirty = true;
    }

    public override void Dispose()
    {
        if (IsDisposed)
            return;

        this.canvas.Dispose();
        this.image.Dispose();
        this.surface.Dispose();
        this.backendTexture.Dispose();

        provider.gl.DeleteTexture(this.glTexture);

        base.Dispose();
    }

    private unsafe void UpdateLocalPixels()
    {
        Color* buffer = null;
        try
        {
            buffer = (Color*)NativeMemory.Alloc((nuint)colors.Length, (nuint)sizeof(Color));

            var gl = provider.gl;
            gl.Finish();
            gl.BindTexture(TextureTarget.Texture2D, glTexture);
            gl.GetTexImage(TextureTarget.Texture2D, 0, PixelFormat.Rgba, PixelType.UnsignedByte, new Span<Color>(buffer, colors.Length));
            gl.BindTexture(TextureTarget.Texture2D, 0);
            pixelsDirty = false;

            TextureFlip(new(buffer, colors.Length), colors);
        }
        finally
        {
            NativeMemory.Free(buffer);
        }
    }

    public unsafe void Update(ReadOnlySpan<Color> pixels)
    {
        if (IsDisposed)
            throw new InvalidOperationException("Cannot use disposed texture!");

        Color* buffer = null;
        try
        {
            // we need to flip the texture (thanks opengl)
            buffer = (Color*)NativeMemory.Alloc((nuint)pixels.Length, (nuint)sizeof(Color));

            TextureFlip(pixels, new(buffer, pixels.Length));

            var gl = provider.gl;
            gl.BindTexture(TextureTarget.Texture2D, glTexture);
            provider.gl.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, (uint)this.Width, (uint)this.Height, PixelFormat.Rgba, PixelType.UnsignedByte, buffer);
            gl.BindTexture(TextureTarget.Texture2D, 0);

            image.Dispose();
            image = SKImage.FromTexture(provider.backendContext, this.backendTexture, SKColorType.Rgba8888);
        }
        finally
        {
            NativeMemory.Free(buffer);
        }
    }

    private void TextureFlip(ReadOnlySpan<Color> src, Span<Color> dest)
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                var srcIndex = y * Width + x;
                var dstIndex = (Height - y - 1) * Width + x;

                dest[dstIndex] = src[srcIndex];
            }
        }
    }

    public ICanvas GetCanvas()
    {
        if (IsDisposed)
            throw new InvalidOperationException("Cannot use disposed texture!");

        if (Options.HasFlag(TextureOptions.NonRenderTarget))
        {
            throw new InvalidOperationException("Cannot render to a texture with the TextureOptions.NonRenderTarget flag.");
        }

        return this.canvas;
    }

    public void ApplyChanges()
    {
        if (pixelsDirty)
        {
            Log.Warn("Texture was changed on the gpu and then ApplyChanges() was called. This could lead to changes being lost.");
        }

        Update(this.Pixels);
    }

    public SKSurface GetSurface()
    {
        if (IsDisposed)
            throw new InvalidOperationException("Cannot use disposed texture!");

        return surface;
    }

    public SKImage GetImage()
    {
        if (IsDisposed)
            throw new InvalidOperationException("Cannot use disposed texture!");

        return image;
    }

    public uint GetGLTexture()
    {
        if (IsDisposed)
            throw new InvalidOperationException("Cannot use disposed texture!");

        return glTexture;
    }

    public void Encode(Stream destination, TextureEncoding encoding)
    {
        var data = image.Encode(encoding switch
        {
            TextureEncoding.PNG => SKEncodedImageFormat.Png,
            _ => throw new Exception("Unsupported or unrecognized encoding!")
        }, 100);

        data.SaveTo(destination);
    }
}