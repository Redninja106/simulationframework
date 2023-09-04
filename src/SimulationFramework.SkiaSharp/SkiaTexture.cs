using System;
using System.Diagnostics;
using SimulationFramework.Drawing;
using SkiaSharp;

namespace SimulationFramework.SkiaSharp;

internal sealed class SkiaTexture : ITexture
{
    private readonly SkiaGraphicsProvider provider;
    private readonly SKBitmap bitmap;
    private readonly bool owner;
    private readonly TextureOptions options;

    private readonly Color[] colors;
    private SkiaCanvas canvas;
    private bool pixelsDirty;

    public SkiaTexture(SkiaGraphicsProvider provider, SKBitmap bitmap, bool owner, TextureOptions options)
    {
        this.provider = provider;
        this.bitmap = bitmap;
        this.owner = owner;
        this.options = options;
        
        if (!options.HasFlag(TextureOptions.NoAccess))
        {
            colors = new Color[bitmap.Width * bitmap.Height];
            UpdateLocalPixels();
        }
    }

    public int Width => bitmap.Width;
    public int Height => bitmap.Height;

    public unsafe Span<Color> Pixels
    {
        get
        {
            if (colors is null)
                throw new InvalidOperationException("CPU access is not allowed on this texture!");

            if (pixelsDirty)
            {
                UpdateLocalPixels();
            }

            return colors.AsSpan();
        }
    }

    public void InvalidatePixels()
    {
        pixelsDirty = true;
    }

    public void Dispose()
    {
        if (owner)
            bitmap.Dispose();

        this.canvas?.Dispose();
    }

    private void UpdateLocalPixels()
    {
        unsafe
        {
            var pixels = (int*)bitmap.GetPixels();
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = ToColor(pixels[i]);
            }
            pixelsDirty = false;
        }
    }

    public ICanvas GetCanvas()
    {
        if (this.canvas is null || this.canvas.IsDisposed)
        {
            this.canvas = new SkiaCanvas(this.provider, this, new SKCanvas(bitmap), true);
        }

        return this.canvas;
    }

    public void ApplyChanges()
    {
        if (pixelsDirty)
        {
            Log.Warn("Texture was changed on the gpu and then ApplyChanges() was called. This could lead to changes being lost.");
        }

        unsafe
        {
            var pixels = (int*)bitmap.GetPixels();
            for (int i = 0; i < colors.Length; i++)
            {
                pixels[i] = ToArgb(colors[i]);
            }

        }

        bitmap.NotifyPixelsChanged();
    }
    
    private static int ToArgb(Color color)
    {
        return color.A << 24 | color.R << 16 | color.G << 8 | color.B;
    }
    
    private static Color ToColor(int argb)
    {
        return new Color((byte)(argb >> 16 & 0xFF), (byte)(argb >> 8 & 0xFF), (byte)(argb & 0xFF), (byte)(argb >> 24 & 0xFF));
    }

    public SKBitmap GetBitmap()
    {
        return bitmap;
    }
}