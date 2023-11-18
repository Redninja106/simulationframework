using SimulationFramework;
using SimulationFramework.Components;
using SimulationFramework.Desktop;
using SimulationFramework.Drawing;
using SimulationFramework.Input;

Start<Program>(new DesktopPlatform());

partial class Program : Simulation
{
    public override void OnInitialize()
    {
    }

    public override void OnRender(ICanvas canvas)
    {
    }
}