
using SimulationFramework.Drawing;
using SimulationFramework.SkiaSharp;
using SimulationFramework;
using SkiaSharp;
using System;

internal sealed class SkiaTexture : SkiaGraphicsObject, ITexture
{
    private readonly SkiaGraphicsProvider provider;
    public int Width { get; }
    public int Height { get; }
    public TextureOptions Options { get; }

    private readonly Color[] colors;
    private bool pixelsDirty;

    private SkiaCanvas canvas;
    private SKImage snapshot;

    private readonly SKSurface surface;

    public unsafe SkiaTexture(SkiaGraphicsProvider provider, int width, int height, TextureOptions options)
    {
        this.provider = provider;
        this.Width = width;
        this.Height = height;
        this.Options = options;

        if (!options.HasFlag(TextureOptions.Constant))
        {
            this.colors = new Color[width * height];
            UpdateLocalPixels();
        }

        surface = SKSurface.Create(provider.backendContext, true, new(width, height));
    }

    public unsafe Span<Color> Pixels
    {
        get
        {
            if (colors is null)
                throw new InvalidOperationException("CPU access is not allowed on this texture!");
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
        this.surface.Dispose();
        this.canvas?.Dispose();
        this.snapshot?.Dispose();
        base.Dispose();
    }

    private unsafe void UpdateLocalPixels()
    {
        if (pixelsDirty)
        {
            fixed (Color* colorPtr = &colors[0])
            {
                surface.ReadPixels(new(Width, Height), (nint)colorPtr, Width * sizeof(Color), 0, 0);
            }
            pixelsDirty = false;
        }
    }

    public unsafe void Update(ReadOnlySpan<Color> pixels)
    {
        var pixmap = surface.PeekPixels();
        var destPixels = pixmap.GetPixelSpan<Color>();
        pixels.CopyTo(destPixels);
    }

    public ICanvas GetCanvas()
    {
        if (Options.HasFlag(TextureOptions.NonRenderTarget))
        {
            throw new InvalidOperationException("Cannot render to a texture with the TextureOptions.NonRenderTarget flag.");
        }

        if (this.canvas is null || this.canvas.IsDisposed)
        {
            this.canvas = new SkiaCanvas(this.provider, this, surface.Canvas, false);
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

    private static int ToArgb(Color color)
    {
        return color.A << 24 | color.R << 16 | color.G << 8 | color.B;
    }

    private static Color ToColor(int argb)
    {
        return new Color((byte)(argb >> 16 & 0xFF), (byte)(argb >> 8 & 0xFF), (byte)(argb & 0xFF), (byte)(argb >> 24 & 0xFF));
    }

    public SKSurface GetSurface()
    {
        return surface;
    }

    public SKImage GetImage()
    {
        if (pixelsDirty || snapshot is null)
        {
            snapshot?.Dispose();
            snapshot = surface.Snapshot();
        }

        return snapshot;
    }
}