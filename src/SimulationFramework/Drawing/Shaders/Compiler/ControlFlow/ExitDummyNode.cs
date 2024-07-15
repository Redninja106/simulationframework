using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.ControlFlow;
internal class ExitDummyNode(ControlFlowGraph containingGraph) : DummyNode
{
    public override bool PrecedesExit => containingGraph.PrecedesExit;
}
