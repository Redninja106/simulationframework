using SimulationFramework;

namespace SkiaSimulation;

internal static class Program
{
    private static void Main(string[] args)
    {
        using var skiaSimulation = new SkiaSimulation();
        Simulation.Run(skiaSimulation);
    }
}