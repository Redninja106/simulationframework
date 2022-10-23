using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders.Compiler.Branching;
internal class LoopBranch : Branch
{
    public LoopBranch(Branch body, BranchGraph graph) : base(graph)
    {
        this.Body = body;
    }

    public Branch Body { get; private set; }
}
