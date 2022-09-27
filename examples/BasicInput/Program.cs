using SimulationFramework;
using SimulationFramework.Desktop;
using SimulationFramework.Drawing;

var simulation = Simulation.Create(Init, Render);
simulation.RunDesktop();

void Init(AppConfig config)
{
}

void Render(ICanvas canvas)
{
    canvas.Clear(Color.Red);
}