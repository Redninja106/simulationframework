using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.Input;

Start<Program>();

partial class Program : Simulation
{
    public override unsafe void OnInitialize()
    {
    }

    public override void OnRender(ICanvas canvas)
    {
        canvas.Clear(Color.Black);
    }
}
