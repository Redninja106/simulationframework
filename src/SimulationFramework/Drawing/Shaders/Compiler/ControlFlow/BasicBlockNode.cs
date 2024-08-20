using SimulationFramework.Drawing.Shaders.Compiler.Disassembler;
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

    public override bool PrecedesExit
    {
        get
        {
            if (Successors.Count == 1)
            {
                var succ = Successors.Single();
                if (succ.Predecessors.Count == 1)
                {
                    return succ.PrecedesExit;
                }
            }
            return false;
        }
    }

    public BasicBlockNode() : base()
    {
    }
}
