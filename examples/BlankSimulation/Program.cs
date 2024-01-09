using SimulationFramework;
using SimulationFramework.Desktop;
using SimulationFramework.Drawing;
using SimulationFramework.Input;
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
        canvas.Fill(Color.White);
        canvas.DrawRect(Mouse.Position, new(100f, 100f), Alignment.Center);
    }
}