using SimulationFramework.Shaders.Compiler.ILDisassembler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders.Compiler.ControlFlow;
internal class Graph
{
    public GraphNode Entry { get; }
    public GraphNode Exit { get; }
    public MethodDisassembly Disassembly { get; set; }

    public readonly List<BasicBlockNode> BasicBlocks = new();

    public Graph(MethodDisassembly disassembly)
    {
        Entry = new BasicBlockNode(this, null);
        Exit = new BasicBlockNode(this, null);

        this.Disassembly = disassembly;
        Entry.AddSuccessor(GetBasicBlock(disassembly.instructions[0]));
    }

    public BasicBlockNode GetBasicBlock(Instruction startInstruction)
    {
        var branch = BasicBlocks.SingleOrDefault(br => br.Instructions.Contains(startInstruction));

        if (branch is null)
        {
            var stream = new InstructionStream(Disassembly);
            stream.Position = startInstruction.Location;
            branch = new BasicBlockNode(this, stream);
            BasicBlocks.Add(branch);
        }

        return branch;
    }
}