using SimulationFramework.Desktop;
using System.Numerics;
using SimulationFramework.Drawing;
using SimulationFramework.Drawing.OpenGL;
using SimulationFramework.Drawing.Direct3D11;

namespace Basic3D;

internal class Program
{
    static void Main(string[] args)
    {
        var sim = new Basic3DSimulation();
        sim.Run(new DesktopPlatform());
    }
}