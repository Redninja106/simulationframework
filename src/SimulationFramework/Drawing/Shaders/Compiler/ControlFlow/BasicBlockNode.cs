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

    public bool IsTrivallyClonable { get; private set; }

    public BasicBlockNode() : base()
    {
    }

    public void UpdateClonable()
    {
        IsTrivallyClonable = Instructions.Count == 1 && Instructions[0].OpCode == OpCode.Ldc_I4_0;
    }

    // clones this basic block
    // only the blocks instructions are cloned (not links)
    public BasicBlockNode Clone()
    {
        if (!IsTrivallyClonable)
            throw new Exception("Cannot clone non-trivial basic block");

        BasicBlockNode result = new();
        result.Instructions.AddRange(Instructions);
        result.UpdateClonable();
        return result;
    }
}
