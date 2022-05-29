using SimulationFramework;
using SimulationFramework.Desktop;
using SimulationFramework.Drawing.Canvas;

var sim = new BlankSimulation();
sim.RunDesktop();

internal class BlankSimulation : Simulation
{
    public override void OnInitialize(AppConfig config)
    {
        config.Title = "This is a Blank Simulation!";
        config.Width = 1920;
        config.Height = 1080;
    }

    public override void OnRender(ICanvas canvas)
    {
        canvas.Clear(Color.Black);
        canvas.Translate(canvas.Width / 2f, canvas.Height / 2f);
        canvas.DrawRect(100, 100, 100, 100);
    }
}