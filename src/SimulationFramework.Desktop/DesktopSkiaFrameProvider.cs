using Silk.NET.GLFW;
using Silk.NET.Input.Extensions;
using Silk.NET.Windowing;
using SimulationFramework.SkiaSharp;
using SkiaSharp;

namespace SimulationFramework.Desktop;

internal class DesktopSkiaFrameProvider : ISkiaFrameProvider
{
    private GRContext context;
    private int width;
    private int height;

    public DesktopSkiaFrameProvider(int width, int height)
    {
        this.width = width;
        this.height = height;
    }

    private GRBackendRenderTarget backendRenderTarget;
    private SKSurface frameSurface;

    public void Resize(int width, int height)
    {
        backendRenderTarget?.Dispose();
        frameSurface?.Dispose();

        const SKColorType format = SKColorType.Rgba8888;
        backendRenderTarget = new GRBackendRenderTarget(width, height, 1, 32, new GRGlFramebufferInfo { Format = format.ToGlSizedFormat(), FramebufferObjectId = 0 });
        frameSurface = SKSurface.Create(context, backendRenderTarget, format);

        this.width = width;
        this.height = height;
    }

    SKCanvas ISkiaFrameProvider.GetCurrentFrame()
    {
        return frameSurface.Canvas;
    }

    public void SetContext(GRContext context)
    {
        this.context = context;
        Resize(width, height);
    }

    public void GetCurrentFrameSize(out int width, out int height)
    {
        width = this.width;
        height = this.height;
    }
}