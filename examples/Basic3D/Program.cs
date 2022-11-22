using SimulationFramework.Desktop;
using SimulationFramework.Drawing.Direct3D11;
using SimulationFramework.Drawing.Direct3D11.ShaderGen;
using SimulationFramework.Shaders.Compiler;
using SimulationFramework.Shaders;
using System.Numerics;
using SimulationFramework.Drawing;

namespace Basic3D;

internal class Program
{
    static void Main(string[] args)
    {
        var sim = new Basic3DSimulation();
        sim.Run();
    }
}