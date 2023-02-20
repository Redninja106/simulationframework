using SimulationFramework.Shaders.Compiler.ControlFlow;
using SimulationFramework.Shaders.Compiler.ILDisassembler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders.Compiler.ControlFlow;

internal class ControlFlowGraph
{
    public BasicBlockNode EntryNode { get; }
    public BasicBlockNode ExitNode { get; }
    public MethodDisassembly Disassembly { get; set; }
    public List<Loop> Loops { get; set; }

    public readonly List<BasicBlockNode> BasicBlocks = new();

    public ControlFlowGraph(MethodDisassembly disassembly)
    {
        EntryNode = new BasicBlockNode(this);
        ExitNode = new BasicBlockNode(this);
        BasicBlocks.Add(this.EntryNode);
        BasicBlocks.Add(this.ExitNode);

        this.Disassembly = disassembly;
        EntryNode.AddSuccessor(GetBasicBlock(disassembly.instructions[0]));

        RecomputeDominators();
        Loops = ComputeLoops();
    }

    public BasicBlockNode GetBasicBlock(Instruction startInstruction)
    {
        var block = BasicBlocks.SingleOrDefault(br => br.Instructions.Contains(startInstruction));

        if (block is null)
        {
            block = new BasicBlockNode(this);
            BasicBlocks.Add(block);

            var stream = new InstructionStream(Disassembly);
            stream.Position = startInstruction.Location;
            AddNextBlocks(block, stream);
        }

        return block;
    }

    private void AddNextBlocks(BasicBlockNode node, InstructionStream instructions)
    {
        while (!instructions.IsAtEnd)
        {
            var instruction = instructions.Peek();

            if (node.Instructions.Count > 1 && instruction.IsBranchTarget())
            {
                node.AddSuccessor(GetBasicBlock(instruction));
                break;
            }

            node.Instructions.Add(instructions.Read());

            var branchBehavior = instruction.OpCode.GetBranchBehavior();
            if (branchBehavior is BranchBehavior.Branch or BranchBehavior.BranchOrContinue)
            {
                var target = instruction.GetBranchTarget();
                Debug.Assert(target is not null);
                node.AddSuccessor(GetBasicBlock(target));

                if (branchBehavior is BranchBehavior.BranchOrContinue)
                {
                    node.AddSuccessor(GetBasicBlock(instructions.Peek()));
                }

                break;
            }

            if (branchBehavior is BranchBehavior.Return)
            {
                node.AddSuccessor(this.ExitNode);

                if (!this.BasicBlocks.Contains(this.ExitNode))
                    this.BasicBlocks.Add(this.ExitNode);

                break;
            }
        }
    }

    public void RecomputeDominators()
    {
        foreach (var block in this.BasicBlocks)
        {
            block.dominators.Clear();

            if (block == this.EntryNode)
            {
                block.dominators.Add(block);
            }
            else
            {
                block.dominators.AddRange(this.BasicBlocks);
            }
        }

        bool changed;

        do
        {
            changed = false;
            foreach (var block in this.BasicBlocks)
            {
                if (block == this.EntryNode)
                    continue;

                foreach (var pred in block.Predecessors)
                {
                    var intersect = block.dominators.Intersect(pred.dominators).Append(block).Distinct().ToArray();

                    if (!block.dominators.SequenceEqual(intersect))
                    {
                        changed = true;
                        block.dominators.Clear();
                        block.dominators.AddRange(intersect);
                    }
                }
            }
        }
        while (changed);

    }

    Loop CreateLoop(BasicBlockNode header, BasicBlockNode tail)
    {
        Stack<BasicBlockNode> stack = new();
        Loop loop = new();

        loop = new Loop();

        loop.header = header;
        loop.tail = tail;
        loop.blocks.Add(header);

        if (header != tail)
        {
            loop.blocks.Add(tail);
            stack.Push(tail);
        }

        while (stack.Any())
        {
            var block = stack.Pop();
            foreach (var pred in block.Predecessors)
            {
                if (!loop.blocks.Contains(pred))
                {
                    loop.blocks.Add(pred as BasicBlockNode);
                    stack.Push(pred as BasicBlockNode);
                }
            }
        }
        return loop;
    }

    List<Loop> ComputeLoops()
    {
        List<Loop> loops = new();

        foreach (var block in BasicBlocks)
        {
            if (block == EntryNode)
                continue;

            foreach (var succ in block.Successors)
            {
                // Every successor that dominates its predecessor
                // must be the header of a loop.
                // That is, block -> succ is a back edge.

                if (block.dominators.Contains(succ))
                    loops.Add(CreateLoop(succ as BasicBlockNode, block));
            }
        }

        return loops;
    }

    public void Traverse(Action<ControlFlowNode> visitNode)
    {
        HashSet<ControlFlowNode> visited = new();
        TraverseHelper(visitNode, this.EntryNode, visited);

        static void TraverseHelper(Action<ControlFlowNode> visitNode, ControlFlowNode current, HashSet<ControlFlowNode> visited)
        {
            visitNode(current);
            visited.Add(current);

            foreach (var successor in current.Successors)
            {
                if (visited.Contains(successor))
                    continue;

                TraverseHelper(visitNode, successor, visited);
            }
        }
    }
}
class Loop
{
    public BasicBlockNode header;
    public readonly List<BasicBlockNode> blocks = new();
    public BasicBlockNode tail;
}