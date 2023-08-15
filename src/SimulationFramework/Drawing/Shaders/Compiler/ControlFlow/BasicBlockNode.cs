using SimulationFramework.Drawing.Shaders.Compiler.ILDisassembler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.ControlFlow;
internal class BasicBlockNode : ControlFlowNode
{
    public readonly List<Instruction> Instructions = new();


    public BasicBlockNode(ControlFlowGraph graph) : base(graph)
    {
    }
}
