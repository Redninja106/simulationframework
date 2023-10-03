
using SimulationFramework.Drawing;
using SimulationFramework.SkiaSharp;
using SimulationFramework;
using SkiaSharp;
using System;
using Silk.NET.OpenGL;

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

        backendTexture = new GRBackendTexture(width, height, false, new((uint)GLEnum.Texture2D, glTexture, (uint)SizedInternalFormat.Rgba8));

        surface = SKSurface.Create(provider.backendContext, backendTexture, SKColorType.Rgba8888);
        canvas = new(provider, this, surface.Canvas, true);

        image = SKImage.FromTexture(provider.backendContext, backendTexture, SKColorType.Rgba8888);

        if (!options.HasFlag(TextureOptions.Constant))
        {
            // this.colors = new Color[width * height];
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
        this.canvas.Dispose();
        this.surface.Dispose();
        this.image.Dispose();
        this.backendTexture.Dispose();

        provider.gl.DeleteTexture(this.glTexture);

        base.Dispose();
    }

    private unsafe void UpdateLocalPixels()
    {
        var gl = provider.gl;
        gl.BindTexture(TextureTarget.Texture2D, glTexture);
        gl.GetTexImage(TextureTarget.Texture2D, 0, PixelFormat.Rgba, PixelType.UnsignedByte, colors.AsSpan());
        gl.BindTexture(TextureTarget.Texture2D, 0);
        pixelsDirty = false;
    }

    public unsafe void Update(ReadOnlySpan<Color> pixels)
    {
        var gl = provider.gl;
        gl.BindTexture(TextureTarget.Texture2D, glTexture);
        provider.gl.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, (uint)this.Width, (uint)this.Height, PixelFormat.Rgba, PixelType.UnsignedByte, pixels);
        gl.BindTexture(TextureTarget.Texture2D, 0);
        image.Dispose();
        image = SKImage.FromTexture(provider.backendContext, this.backendTexture, SKColorType.Rgba8888);
    }

    public ICanvas GetCanvas()
    {
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
        return surface;
    }

    public SKImage GetImage()
    {
        return image;
    }

    public uint GetGLTexture()
    {
        return glTexture;
    }
}