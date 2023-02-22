using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders.Compiler.ControlFlow;
internal interface ISubgraphContainer
{
    ControlFlowGraph Subgraph { get; }
}
