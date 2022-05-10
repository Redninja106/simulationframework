using SimulationFramework;
using SimulationFramework.Desktop;
using SimulationFramework.Gradients;

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
        canvas.Translate(canvas.Width / 2f, canvas.Height / 2f);
        canvas.DrawRect(100, 100, 100, 100);
    }

    public override void OnUnitialize()
    {
    }
}