using SimulationFramework.Desktop;
using SimulationFramework.Drawing.Direct3D11;
using SimulationFramework.SFSL;

namespace Basic3D;

internal class Program
{
    static void Main(string[] args)
    {
//        SimulationFramework.SFSL.Compiler c = new();
//        c.Compile(@"
//using Basic3D;
//void main(Vertex vertex) 
//{
    
//}

//");
//        return;
        var sim = new Basic3DSimulation();
        sim.RunDesktop(hwnd => new D3D11Graphics(hwnd));
    }
}

public struct Vertex
{
    
}
