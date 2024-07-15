using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.ControlFlow;

internal class ControlFlowNode
{
    private static int nextDebugID;
    public int DebugID;

    private readonly HashSet<ControlFlowNode> predecessors = new();
    private readonly HashSet<ControlFlowNode> successors = new();
    internal readonly HashSet<ControlFlowNode> dominators = new();
    internal readonly HashSet<ControlFlowNode> postdominators = new();

    public ControlFlowNode immediateDominator = null;
    public ControlFlowNode immediatePostDominator = null;

    public virtual bool PrecedesExit => successors.Count == 1 && successors.Single().PrecedesExit;

    public IReadOnlySet<ControlFlowNode> Predecessors => predecessors;
    public IReadOnlySet<ControlFlowNode> Successors => successors;

    public ControlFlowNode()
    {
        this.DebugID = nextDebugID++;
    }

    public void AddPredecessor(ControlFlowNode predecessor)
    {
        predecessors.Add(predecessor);
        predecessor.successors.Add(this);
    }

    public void AddSuccessor(ControlFlowNode successor)
    {
        successor.AddPredecessor(this);
    }

    public void RemovePredecessor(ControlFlowNode predecessor)
    {
        predecessors.Remove(predecessor);
        predecessor.successors.Remove(this);
    }

    public void RemoveSuccessor(ControlFlowNode successor)
    {
        successor.RemovePredecessor(this);
    }

    public override string ToString()
    {
        return $"{GetType().Name} {DebugID}";
    }
}
