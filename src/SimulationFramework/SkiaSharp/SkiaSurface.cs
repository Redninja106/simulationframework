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

    public int Width { get; }
    public int Height { get; }

    public void Dispose()
    {
        if (owner)
            bitmap.Dispose();
    }

    public ICanvas OpenCanvas()
    {
        return new SkiaCanvas(this.provider, this, new SKCanvas(bitmap), false);
    }

    public Span<Color> GetPixels()
    {
        throw new NotImplementedException();
    }

    public IntPtr GetPixelData(out int rowSize, out int rowCount)
    {
        throw new NotImplementedException();
    }
}