using SimulationFramework.Desktop;
using SimulationFramework.Drawing.Direct3D11;

namespace BlankSimulation;

internal class Program
{
    static void Main(string[] args)
    {
        var sim = new BlankSimulation();
        sim.RunDesktop(hwnd => new D3D11Graphics(hwnd));
    }
}
