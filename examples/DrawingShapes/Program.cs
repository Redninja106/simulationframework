using System.Numerics;
using System.Runtime.CompilerServices;
using SimulationFramework;
using SimulationFramework.Desktop;
using SimulationFramework.Drawing.Canvas;
using SimulationFramework.IMGUI;
using SimulationFramework.Messaging;

var sim = new DrawingShapesSimulation();
sim.RunDesktop();

class DrawingShapesSimulation : Simulation
{
    public override void OnInitialize(AppConfig config)
    {
        this.Application.Dispatcher.Subscribe<ResizeMessage>(m => Console.WriteLine(m.Width + " " + m.Height));
    }

    public override void OnRender(ICanvas canvas)
    {
        canvas.Clear(Color.LightGray);

        canvas.Translate(canvas.Width / 2, canvas.Height / 2);

        canvas.Fill(Color.Red);
        canvas.DrawRect(0, 0, 100, 100);

        canvas.Fill(Color.Red);
        canvas.DrawCircle(new Circle(150, 150, 50));
    }
}