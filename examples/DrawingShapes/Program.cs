using SimulationFramework.Desktop;
namespace DrawingShapes;

internal class Program
{
    static void Main(string[] args)
    {
        var sim = new DrawingShapesSimulation();
        sim.RunDesktop();
    }
}