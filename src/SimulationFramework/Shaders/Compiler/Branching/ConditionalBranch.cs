using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders.Compiler.Branching;
internal class ConditionalBranch : Branch
{
    public ConditionalBranch(BranchGraph graph) : base(graph)
    {
    }

    public Branch Condition { get; }
    public Branch TrueCase { get; }
    public Branch? FalseCase { get; }
}
