using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.ImGuiNET;
using System.Numerics;

namespace BlankSimulation;

internal class BlankSimulation : Simulation
{
    public override void OnInitialize(AppConfig config)
    {
        Application!.AddComponent(new ImGuiComponent());
    }

    public override void OnRender(ICanvas canvas)
    {
        IRenderer renderer = Graphics.GetRenderer();

        renderer.Clear(Color.Red);
    }
}