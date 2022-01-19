using SimulationFramework;
using SimulationFramework.Desktop;

using var sim = new BlankSimulation();
sim.RunWindowed("This is a Blank Simulation!", 1920, 1080);

internal class BlankSimulation : Simulation
{
    public override void OnInitialize(AppConfig config)
    {
    }

    public override void OnRender(ICanvas canvas)
    {
        canvas.Clear(Color.Black);
    }

    public override void OnUnitialize()
    {
    }
}