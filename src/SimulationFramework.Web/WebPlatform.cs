using Microsoft.JSInterop;
using SimulationFramework.Components;
using SimulationFramework.Messaging;
using SimulationFramework.SkiaSharp;
using System.Runtime.InteropServices.JavaScript;

namespace SimulationFramework.Web;

public class WebPlatform : ISimulationPlatform
{
    public int width, height;
    private JSObject canvas, context;

    public WebPlatform(string canvasId) : this(JSInterop.GetElementById(canvasId))
    {
    }

    public WebPlatform(JSObject canvas)
    {
        if (!OperatingSystem.IsBrowser())
        {
            throw new NotSupportedException("The web platform can only run on browser wasm.");
        }

        this.canvas = canvas;
        width = canvas.GetPropertyAsInt32("width");
        height = canvas.GetPropertyAsInt32("height");
        JSHost.GlobalThis.SetProperty("_sfcanvas", this.canvas);
        var context = JSInterop.GetContext("webgl2");
        // JSHost.GlobalThis.SetProperty("GLctx", context);


        Console.WriteLine($"web platform initialized... [{width}x{height}].");
    }

    public void Initialize(MessageDispatcher dispatcher)
    {
        Application.RegisterComponent<WebSimulationController>(new(this));
        Application.RegisterComponent<SkiaGraphicsProvider>(new(new FrameProvider(width, height), null));
    }

    public void Dispose()
    {
    }
}