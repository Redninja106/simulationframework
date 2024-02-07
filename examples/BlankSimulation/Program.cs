using Silk.NET.OpenGL;
using SimulationFramework;
using SimulationFramework.Desktop;
using SimulationFramework.Drawing;
using SimulationFramework.SkiaSharp;
using SkiaSharp;

Start<Program>(new DesktopPlatform());

partial class Program : Simulation
{
    SKSurface surface;

    public override unsafe void OnInitialize()
    {
        var graphics = Application.GetComponent<SkiaGraphicsProvider>();
        surface = SKSurface.Create(graphics.backendContext, true, new SKImageInfo(100, 100));
    }

    public override void OnRender(ICanvas canvas)
    {
        SKRuntimeEffect effect = SKRuntimeEffect.Create(@"
float4 main(float2 pos) {
    return float4(0,1,0,1);
}
", out string errors);

        surface.Canvas.Clear(new SKColor(255, 0, 0));
        surface.Canvas.DrawCircle(50, 50, 50, new()
        {
            Shader = effect.ToShader(false),
        });
        // gl.BindFramebuffer(FramebufferTarget.Framebuffer, fbo);
        // gl.ClearColor(1, 0, 0, 1);
        // gl.Clear(ClearBufferMask.ColorBufferBit);

        SkiaInterop.GetCanvas(canvas).DrawImage(surface.Snapshot(), new SKPoint(0, 0));
    }
}