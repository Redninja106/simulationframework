using SimulationFramework;
using SimulationFramework.Desktop;
namespace DrawingShapes;

internal class Program
{
    static void Main()
    {
        Simulation.Start<DrawingShapesSimulation>(new DesktopPlatform());
    }
}