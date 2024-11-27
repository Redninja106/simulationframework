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
        // only case that we need basic block clone is chained && and || operators in conditionals
        // these are the only two instructions that need to be cloneable for those cases.
        //IsTrivallyClonable = 
        //    Instructions.Count == 1 && 
        //    Instructions[0].OpCode is OpCode.Ldc_I4_0 or OpCode.Ldc_I4_1;

        IsTrivallyClonable = true;
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
