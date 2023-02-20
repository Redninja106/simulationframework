using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders.Compiler.ControlFlow;

internal class ControlFlowNode
{
    private static int nextDebugID;
    public int DebugID;

    private readonly List<ControlFlowNode> predecessors = new();
    private readonly List<ControlFlowNode> successors = new();
    internal readonly List<ControlFlowNode> dominators = new();

    public IReadOnlyList<ControlFlowNode> Predecessors => predecessors;
    public IReadOnlyList<ControlFlowNode> Successors => successors;

    public ControlFlowGraph Graph { get; }


    public ControlFlowNode(ControlFlowGraph graph)
    {
        this.Graph = graph;
        this.DebugID = nextDebugID++;
    }

    public void AddPredecessor(ControlFlowNode predecessor)
    {
        predecessors.Add(predecessor);
        predecessor.successors.Add(this);
    }

    public void AddSuccessor(ControlFlowNode successor)
    {
        successors.Add(successor);
        successor.predecessors.Add(this);
    }

    public void RemovePredecessor(ControlFlowNode predecessor)
    {
        predecessors.Remove(predecessor);
        predecessor.successors.Remove(this);
    }

    public void RemoveSuccessor(ControlFlowNode successor)
    {
        successors.Remove(successor);
        successor.predecessors.Remove(this);
    }

    //public bool Dominates(ControlFlowNode node)
    //{
    //    if (node == this)
    //        return true;

    //    var dominator = node.ImmediateDominator;
    //    do
    //    {
    //        if (this == dominator)
    //            return true;
    //    }
    //    while (dominator != Graph.Entry);

    //    return false;
    //}

    public override string ToString()
    {
        return $"node {DebugID}";
    }
}
