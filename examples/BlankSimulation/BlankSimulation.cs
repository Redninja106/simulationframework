using SimulationFramework;
using SimulationFramework.Drawing;
using System.Numerics;

namespace BlankSimulation;

internal class BlankSimulation : Simulation
{
    public override void OnInitialize(AppConfig config)
    {
    }

    public override void OnRender(ICanvas canvas)
    {
        canvas.Clear(Color.Red);
    }
}