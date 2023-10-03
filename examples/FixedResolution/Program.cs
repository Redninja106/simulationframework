using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.Input;

Start<Program>();

partial class Program : Simulation
{
    float radius = 10;

    public override void OnInitialize()
    {
        SetFixedResolution(100, 100, new Color(25, 25, 25), subpixelInput: true);
    }

    public override void OnRender(ICanvas canvas)
    {
        radius += Mouse.ScrollWheelDelta;
        radius = Math.Clamp(radius, 1, 25);

        if (Mouse.IsButtonDown(MouseButton.Left))
        {
            canvas.Fill(Color.White);
            canvas.DrawCircle(Mouse.Position, radius);
        }

        if (Mouse.IsButtonDown(MouseButton.Right))
        {
            canvas.Fill(Color.Black);
            canvas.DrawCircle(Mouse.Position, radius);
        }
    }
}