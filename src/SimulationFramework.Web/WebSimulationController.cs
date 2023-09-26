using SimulationFramework.Components;
using SimulationFramework.Messaging;
using SimulationFramework.SkiaSharp;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Web;
internal class WebSimulationController : ISimulationController
{
    private Action runFrame;
    private double lastTime;
    private readonly WebPlatform platform;

    public WebSimulationController(WebPlatform platform)
    {
        this.platform = platform;
    }


    public void Dispose()
    {
    }

    public void Initialize(MessageDispatcher dispatcher)
    {
    }

    public void Start(Action runFrame)
    {
        this.runFrame = runFrame;

        JSInterop.RequestAnimationFrame(OnAnimationFrame);
    }

    public void OnAnimationFrame(double timestamp)
    {
        double deltaTime = (lastTime - timestamp) / 1000.0;
        lastTime = timestamp;
        runFrame();
        JSInterop.RequestAnimationFrame(OnAnimationFrame);
    }
}

class FrameProvider : ISkiaFrameProvider
{
    private GRContext context;
    private int width;
    private int height;

    public FrameProvider(int width, int height)
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
        backendRenderTarget = new GRBackendRenderTarget(width, height, 1, 0, new GRGlFramebufferInfo { Format = format.ToGlSizedFormat(), FramebufferObjectId = 0 });
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