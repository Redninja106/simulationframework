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
        canvas.Clear(Color.White);
    }
}