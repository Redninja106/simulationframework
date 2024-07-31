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
        canvas.Translate(Mouse.Position);
        canvas.Stroke(Color.White);
        canvas.DrawRect(canvas.MeasureText("This text is drawn using a custom font!", 32));
        canvas.Fill(Color.Red);
        canvas.DrawText("This text is drawn using a custom font!", 32, 0, 0);
    }
}