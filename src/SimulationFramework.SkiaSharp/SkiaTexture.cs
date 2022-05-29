using System;
using SimulationFramework.Drawing.Canvas;
using SkiaSharp;

namespace SimulationFramework.SkiaSharp;

internal sealed class SkiaTexture : ITexture
{
    private readonly SkiaGraphicsProvider provider;
    private readonly SKBitmap bitmap;
    private readonly bool owner;

    private Color[] colors;

    public SkiaTexture(SkiaGraphicsProvider provider, SKBitmap bitmap, bool owner)
    {
        this.provider = provider;
        this.bitmap = bitmap;
        this.owner = owner;
    }

    public int Width => bitmap.Width;
    public int Height => bitmap.Height;

    public unsafe Span<Color> Pixels => new(bitmap.GetPixels().ToPointer(), Width * Height);

    public void Dispose()
    {
        if (owner)
            bitmap.Dispose();
    }

    public ICanvas OpenCanvas()
    {
        return new SkiaCanvas(this.provider, this, new SKCanvas(bitmap), false);
    }

    public ref Color GetPixel(int x, int y)
    {
        return ref this.Pixels[y * Width + x];
    }

    public void ApplyChanges()
    {
        bitmap.NotifyPixelsChanged();
    }

    public SKBitmap GetBitmap()
    {
        return bitmap;
    }
}