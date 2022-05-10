using SimulationFramework.Desktop;

namespace SimulationFramework.Tests.Interactive.Gradients;

public class Program : Simulation
{
    private static void Main()
    {
        using var program = new Program();
        program.RunWindowed("Gradient Tests", 1920, 1080);
    }

    public override void OnInitialize(AppConfig config)
    {
    }

    public override void OnRender(ICanvas canvas)
    {
        canvas.Translate(canvas.Width / 2, canvas.Height / 2);
        canvas.Scale(canvas.Width / 20f);

        canvas.DrawMode = DrawMode.Gradient;

        canvas.SetGradientLinear(Alignment.TopLeft, Alignment.BottomRight, Color.Red, Color.Blue);
        canvas.DrawRect(0, 0, 1, 1, Color.Black, Alignment.Center);
    }
}