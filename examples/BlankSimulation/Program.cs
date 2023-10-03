using SimulationFramework;
using SimulationFramework.Drawing;

Start<Program>();

partial class Program : Simulation
{
    public override void OnInitialize()
    {
    }

    public override void OnRender(ICanvas canvas)
    {
        canvas.Clear(Color.Black);
    }
}