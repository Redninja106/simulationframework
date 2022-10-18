using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.Messaging;
using System.Numerics;

namespace BlankSimulation;

internal class BlankSimulation : Simulation
{
    public override void OnInitialize(AppConfig config)
    {
    }

    public override void OnRender(ICanvas canvas)
    {
        Application!.Dispatcher.Subscribe<RenderMessage>((msg) => { });
        canvas.Clear(Color.Red);
    }
}