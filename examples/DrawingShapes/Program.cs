using System.Numerics;
using System.Runtime.CompilerServices;
using SimulationFramework;
using SimulationFramework.Desktop;
using SimulationFramework.Gradients;
using SimulationFramework.IMGUI;

using var sim = new DrawingShapesSimulation();
sim.RunWindowed("Shapes!", 1920, 1080, false);

class DrawingShapesSimulation : Simulation
{
    public override void OnInitialize(AppConfig config)
    {
    }

    public override void OnRender(ICanvas canvas)
    {
        canvas.Clear(Color.LightGray);

        canvas.Translate(canvas.Width / 2, canvas.Height / 2);

        canvas.Fill(Color.Red);
        canvas.DrawRect(0, 0, 10, 10);

        canvas.Fill(Color.Red);
        canvas.DrawCircle(new Circle(15, 15, 5));
    }


    public override void OnUnitialize()
    {
    }
}