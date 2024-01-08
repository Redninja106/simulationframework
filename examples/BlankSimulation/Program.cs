using Silk.NET.OpenGL;
using SimulationFramework;
using SimulationFramework.Desktop;
using SimulationFramework.Drawing;
using System.Numerics;

Start<Program>(new DesktopPlatform());

partial class Program : Simulation
{

    public override void OnInitialize()
    {
    }

    public override void OnRender(ICanvas canvas)
    {
        // problem: vertex/index buffers being overwriten before command buffer is submitted to queue

        canvas.Clear(Color.FromHSV(0, 0, .1f));
        canvas.Translate(canvas.Width / 2f, canvas.Height / 2f);
        canvas.DrawRect(+.5f, +.5f, .1f, .1f, Alignment.Center);
        canvas.DrawRect(-.5f, +.5f, .1f, .1f, Alignment.Center);
        canvas.DrawRect(+.5f, -.5f, .1f, .1f, Alignment.Center);
        canvas.DrawRect(-.5f, -.5f, .1f, .1f, Alignment.Center);
    }
}