using SimulationFramework.Desktop;
using SimulationFramework.Drawing.Direct3D11;

namespace Basic3D;

internal class Program
{
    static void Main(string[] args)
    {
        var sim = new Basic3DSimulation();
        sim.RunDesktop(hwnd => new D3D11Graphics(hwnd));
    }
}
