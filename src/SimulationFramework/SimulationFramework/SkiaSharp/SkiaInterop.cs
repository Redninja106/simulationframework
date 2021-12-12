using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;

namespace SimulationFramework.SkiaSharp;

public static class SkiaInterop
{
    public static SKCanvas GetCanvas(ICanvas canvas)
    {
        if (canvas is not SkiaCanvas skiaCanvas)
            throw new ArgumentException("'canvas' must be a canvas created using the SkiaSharp graphics backend!");

        return skiaCanvas.canvas;
    }

    public static SKSurface GetSurface(ISurface surface)
    {
        if (surface is not SkiaSurface skiaSurface)
            throw new ArgumentException("'surface' must be a surface created using the SkiaSharp graphics backend!");

        return skiaSurface.surface;
    }

    public static GRContext GetBackendContext(IGraphics graphics)
    {
        if (graphics is not SkiaGraphics skiaGraphics)
            throw new ArgumentException("'graphics' must be a graphics context created using the SkiaSharp graphics backend!");

        return skiaGraphics.backendContext;
    }
}