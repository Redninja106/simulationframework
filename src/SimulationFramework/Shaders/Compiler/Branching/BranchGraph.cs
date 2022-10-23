using SimulationFramework.Shaders.Compiler.ILDisassembler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders.Compiler.Branching;
internal class BranchGraph
{
    public Branch Entry { get; }
    public Branch Exit { get; }
    public MethodDisassembly Disassembly { get; set; }

    public readonly List<UnitBranch> unitBranches = new();

    public BranchGraph(MethodDisassembly disassembly)
    {
        Entry = new UnitBranch(this, null);
        Exit = new UnitBranch(this, null);

        this.Disassembly = disassembly;
        Entry.AddSuccessor(GetUnitBranch(disassembly.instructions[0]));
    }

    public UnitBranch GetUnitBranch(Instruction startInstruction)
    {
        var branch = unitBranches.SingleOrDefault(br => br.Instructions.Contains(startInstruction));

        if (branch is null)
        {
            var stream = new InstructionStream(Disassembly);
            stream.Position = startInstruction.Location;
            branch = new UnitBranch(this, stream);
            unitBranches.Add(branch);
        }

        return branch;
    }

    public void InsertLoopBranches()
    {
        Branch loopBegin;
        
    }

    
}
