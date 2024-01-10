using SimulationFramework;
using SimulationFramework.Desktop;
using SimulationFramework.Drawing;
using SimulationFramework.Input;
using System.Numerics;

Start<Program>(new DesktopPlatform());

partial class Program : Simulation
{

    List<Vector2> positions = new();

    public override void OnInitialize()
    {
    }

    public override void OnRender(ICanvas canvas)
    {
        canvas.Clear(Color.FromHSV(0, 0, .1f));
        canvas.Fill(Color.White);
        positions.Add(Mouse.Position);
        Window.Title = (1f / Time.DeltaTime).ToString();
        var rng = new Random();
        for (int i = 0; i < MathF.Pow(10, 5); i++)
        {
            canvas.DrawRect(rng.NextVector2() * new Vector2(canvas.Width-100f, canvas.Height-100f), new(100f, 100f), Alignment.TopLeft);
        }
    }
}
