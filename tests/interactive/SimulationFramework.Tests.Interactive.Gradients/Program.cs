using SimulationFramework.Desktop;
using SimulationFramework.Drawing.Canvas;
using System.Numerics;

namespace SimulationFramework.Tests.Interactive.Gradients;

public class Program : Simulation
{
    private static void Main()
    {
        var program = new Program();
        program.RunDesktop();
    }

    public override void OnInitialize(AppConfig config)
    {
        
    }

    Gradient g = Gradient.CreateLinear(Vector2.Zero, Vector2.One * 100, Color.Red, Color.Blue);
    public override void OnRender(ICanvas canvas)
    {
        canvas.Translate(canvas.Width / 2, canvas.Height / 2);
        canvas.Scale(canvas.Width / 20f);

        canvas.Fill(g);
        canvas.DrawRect(0, 0, 100, 100);
    }
}