using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;

namespace SimulationFramework.SkiaSharp;

internal sealed class SkiaGraphics : IGraphicsProvider
{
    internal readonly GRGlInterface glInterface;
    internal readonly GRContext backendContext;
    internal readonly ISkiaFrameProvider frameProvider;

    internal readonly SkiaSurface frameSurfaceAdapter;
    internal SkiaCanvas frameCanvas;

    public SkiaGraphics(GRGlGetProcedureAddressDelegate getProcAddress, ISkiaFrameProvider frameProvider)
    {
        this.frameProvider = frameProvider;

        glInterface = GRGlInterface.CreateOpenGl(getProcAddress);
        backendContext = GRContext.CreateGl(glInterface);

        frameProvider.SetContext(this.backendContext);

        frameSurfaceAdapter = new SkiaSurface(this, null, false);
    }

    public IBitmap CreateBitmap(int width, int height)
    {
        return CreateBitmap(width, height, null);
    }

    public IBitmap CreateBitmap(int width, int height, Span<Color> data)
    {
        var bitmap = new SkiaBitmap(this, width, height);

        if (!data.IsEmpty)
        {
        }

        return bitmap;
    }

    public ICanvas GetFrameCanvas()
    {
        var canvas = frameProvider.GetCurrentFrame();

        if (frameCanvas is null || canvas != frameCanvas.canvas)
        {
            frameCanvas?.Dispose();
            frameCanvas = new SkiaCanvas(canvas, false);
        }

        return frameCanvas;
    }

    public ISurface LoadSurface(string path)
    {
        throw new NotImplementedException();
    }

    public ISurface LoadSurface(Span<byte> encodedData)
    {
        throw new NotImplementedException();
    }
}