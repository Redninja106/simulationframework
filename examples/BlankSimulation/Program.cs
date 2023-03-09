using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.Drawing.Implementations.Canvas;
using SimulationFramework.Shaders;
using System.Numerics;

RendererCanvas canvas = null!;
Simulation.Create(Initialize, Render).Run();

void Initialize(AppConfig config)
{
    canvas = new();
}

void Render(ICanvas c)
{
    canvas.Clear(Color.FromHSV(0,0,.1f));
    canvas.DrawRect(new(0, 0, 1, 1));
}