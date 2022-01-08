using SimulationFramework;
using SimulationFramework.IMGUI;

using var sim = new BlankSimulation();
Simulation.RunWindowed(sim, "This is a Blank Simulation!", 1920, 1080);

internal class BlankSimulation : Simulation
{
    public override void OnInitialize(AppConfig config)
    {
    }

    public override void OnRender(ICanvas canvas)
    {
    }

    public override void OnUnitialize()
    {
    }
}