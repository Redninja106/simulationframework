using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;

namespace SimulationFramework.SkiaSharp;

internal sealed class SkiaSurface : ISurface
{
    internal SkiaGraphicsProvider provider;
    internal SKBitmap bitmap;
    internal readonly bool owner;

    public SkiaSurface(SkiaGraphicsProvider provider, SKBitmap bitmap, bool owner)
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
}