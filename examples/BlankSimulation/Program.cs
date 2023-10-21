using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.Input;

Start<Program>();

partial class Program : Simulation
{
    public override void OnInitialize()
    {
    }

    public override void OnRender(ICanvas canvas)
    {
        canvas.Clear(Color.Black);
        canvas.DrawText(Performance.Framerate.ToString(), 0, 0);
        Performance.FramerateAverageDuration = 5;

        if (Keyboard.IsKeyPressed(Key.Space))
        {
            Graphics.SwapInterval = 1&~Graphics.SwapInterval;
        }
    }
}