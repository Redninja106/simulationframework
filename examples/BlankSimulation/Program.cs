using SimulationFramework;
using SimulationFramework.Desktop;
using SimulationFramework.Drawing;

Simulation.Start<MySimulation>(new DesktopPlatform());

class MySimulation : Simulation
{
    public override void OnInitialize()
    {
    }

    public override void OnRender(ICanvas canvas)
    {
        canvas.Clear(Color.Red);
    }
}