using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.Drawing.RenderPipeline;
using System.Numerics;

namespace BlankSimulation;

internal class BlankSimulation : Simulation
{
    public override void OnInitialize(AppConfig config)
    {
    }

    public override void OnRender(ICanvas canvas)
    {
        // canvas.Clear(Color.Red);
        IRenderer renderer = Graphics.GetRenderer();

        renderer.Clear(Color.Red);
        Console.WriteLine(Performance.Framerate);
    }
}