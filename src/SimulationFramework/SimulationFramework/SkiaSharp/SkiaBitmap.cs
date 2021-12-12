using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;

namespace SimulationFramework.SkiaSharp;

internal class SkiaBitmap : IBitmap
{
    public int Width => bitmap.Width;
    public int Height => bitmap.Height;

    internal SKBitmap bitmap;

    public SkiaBitmap(SkiaGraphics service, int width, int height)
    {
        bitmap = new SKBitmap(width, height);
    }

    public ICanvas OpenCanvas()
    {
        return new SkiaCanvas(new SKCanvas(this.bitmap), true);
    }

    public void Dispose()
    {
        bitmap.Dispose();
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