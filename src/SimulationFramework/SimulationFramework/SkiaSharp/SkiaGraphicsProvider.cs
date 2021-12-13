using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;

namespace SimulationFramework.SkiaSharp;

internal sealed class SkiaGraphicsProvider : IGraphicsProvider
{
    internal readonly GRGlInterface glInterface;
    internal readonly GRContext backendContext;
    internal readonly ISkiaFrameProvider frameProvider;

    internal readonly SkiaSurface frameSurfaceAdapter;
    internal SkiaCanvas frameCanvas;

    public SkiaGraphicsProvider(ISkiaFrameProvider frameProvider, GRGlGetProcedureAddressDelegate getProcAddress)
    {
        this.frameProvider = frameProvider;

        glInterface = GRGlInterface.CreateOpenGl(getProcAddress);
        backendContext = GRContext.CreateGl(glInterface);

        frameProvider.SetContext(this.backendContext);

        frameSurfaceAdapter = new SkiaSurface(this, null, false);
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

    public ISurface LoadBitmap(Span<byte> encodedData)
    {
        throw new NotImplementedException();
    }

    public ISurface CreateSurface(int width, int height, Span<Color> data)
    {
        var bitmap = new SkiaSurface(this, new SKBitmap(width, height), true);

        if (!data.IsEmpty)
        {
        }

        return bitmap;
    }

    public ISurface CreateSurface(Span<byte> encodedData)
    {
        throw new NotImplementedException();
    }

    public void Apply(Simulation simulation)
    {
        
    }

    public void Dispose()
    {
    }
}