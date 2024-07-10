using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.Input;
using SimulationFramework.OpenGL;
using System.Numerics;

Start<Program>();

partial class Program : Simulation
{
    public override void OnInitialize()
    {
    }

    char typed;
    public override void OnRender(ICanvas canvas)
    {
        canvas.Clear(Color.FromHSV(0, 0, .15f));
        canvas.FontSize(32);
        canvas.Translate(Mouse.Position);
        canvas.Stroke(Color.RebeccaPurple);
        canvas.StrokeWidth(10);
        canvas.DrawCircle(0, 0, 50);

        // canvas.DrawText("This text is drawn using a custom font!", 0, 0);
    }
}