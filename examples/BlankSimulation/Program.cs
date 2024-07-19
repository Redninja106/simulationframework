using SimulationFramework;
using SimulationFramework.Desktop;
using SimulationFramework.Drawing;
using SimulationFramework.Input;

Start<Program>(new DesktopPlatform());

partial class Program : Simulation
{
    public override unsafe void OnInitialize()
    {
    }

    public override void OnRender(ICanvas canvas)
    {
        canvas.Clear(Color.Black);
    }
}