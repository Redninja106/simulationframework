using SimulationFramework.Desktop;
using SimulationFramework.Drawing.Direct3D11;
using SimulationFramework.Shaders.Compiler.Branching;
using SimulationFramework.Shaders.Compiler.ILDisassembler;
using System.Net;

namespace BlankSimulation;

internal class Program
{
    static void Main(string[] args)
    {
        var disassembly = new MethodDisassembly(typeof(Program).GetMethod(nameof(test))!);

        foreach (var ins in disassembly.instructions)
        {
            Console.WriteLine(ins);
        }

        Console.WriteLine(new string('=', 100));
        
        var graph = new BranchGraph(disassembly);

        Console.WriteLine("units:");

        foreach (var unit in graph.unitBranches)
        {
            Console.WriteLine("\tinstructions:");

            foreach (var instr in unit.Instructions)
            {
                Console.WriteLine("\t\t" + instr.ToString());
            }
        }

        Console.WriteLine("");

        //var sim = new BlankSimulation();
        //sim.RunDesktop(hwnd => new D3D11Graphics(hwnd));
    }

    public void test(bool c)
    {
        if (c)
        {
            Console.WriteLine("12");
        }
        else
        {
            Console.WriteLine("23");
        }
    }
}
