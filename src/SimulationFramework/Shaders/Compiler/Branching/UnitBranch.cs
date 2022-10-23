using SimulationFramework.Shaders.Compiler.ILDisassembler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders.Compiler.Branching;
internal class UnitBranch : Branch
{
    public readonly List<Instruction> Instructions = new();

    public UnitBranch(BranchGraph graph, InstructionStream? instructions) : base(graph)
    {
        if (instructions is null)
            return;

        while (!instructions.IsAtEnd)
        {
            var instruction = instructions.Peek();

            if (Instructions.Count > 1 && instruction.IsBranchTarget())
            {
                AddSuccessor(Graph.GetUnitBranch(instruction));
                break;
            }

            Instructions.Add(instructions.Read());

            var branchBehavior = instruction.OpCode.GetBranchBehavior();
            if (branchBehavior is BranchBehavior.Branch or BranchBehavior.BranchOrContinue)
            {
                var target = instruction.GetBranchTarget();
                Debug.Assert(target is not null);
                AddSuccessor(Graph.GetUnitBranch(target));

                if (branchBehavior is BranchBehavior.BranchOrContinue)
                {
                    AddSuccessor(Graph.GetUnitBranch(instructions.Peek()));
                }

                break;
            }

            if (branchBehavior is BranchBehavior.Return)
            {
                AddSuccessor(graph.Exit);
                break;
            }
        }
    }
}
