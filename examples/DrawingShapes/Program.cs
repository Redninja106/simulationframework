using SimulationFramework;
using SimulationFramework.Desktop;
namespace DrawingShapes;

internal class Program
{
    static void Main(string[] args)
    {
        Simulation.Start<DrawingShapesSimulation>(new DesktopPlatform());
    }
}