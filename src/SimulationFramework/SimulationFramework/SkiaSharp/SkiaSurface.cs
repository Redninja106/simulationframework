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
    internal SKSurface surface;
    internal readonly bool owner;

    public SkiaSurface(SkiaGraphics context, SKSurface frame, bool owner)
    {
        this.surface = frame;
        this.owner = owner;
    }

    public int Width { get; }
    public int Height { get; }

    public void Dispose()
    {
        if (owner)
            surface.Dispose();
    }

    public ICanvas OpenCanvas()
    {
        return new SkiaCanvas(surface.Canvas, false);
    }
}