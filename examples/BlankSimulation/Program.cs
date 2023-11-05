using SimulationFramework;
using SimulationFramework.Components;
using SimulationFramework.Desktop;
using SimulationFramework.Drawing;
using SimulationFramework.Input;

Start<Program>(new CustomDesktopPlatform());

class CustomDesktopPlatform : DesktopPlatform
{
    protected override IGraphicsProvider? CreateGraphicsProvider()
    {
        return null;
    }
}

partial class Program : Simulation
{
    public override void OnInitialize()
    {
    }

    public override void OnRender(ICanvas canvas)
    {
    }
}