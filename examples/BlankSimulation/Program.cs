using SimulationFramework;
using SimulationFramework.Drawing;

Simulation simulation = Simulation.Create(Initialize, Render);
simulation.Run();

void Initialize(AppConfig config)
{
}


void Render(ICanvas canvas)
{
}