using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders.Compiler.ControlFlow;
internal class GraphNode
{
    private readonly List<GraphNode> predecessors = new();
    private readonly List<GraphNode> successors = new();
    private readonly List<GraphNode> dominators = new();

    public IReadOnlyList<GraphNode> Predecessors => predecessors;
    public IReadOnlyList<GraphNode> Successors => successors;
    public IReadOnlyList<GraphNode> Dominators => dominators;

    public Graph Graph { get; }

    public GraphNode(Graph graph)
    {
        this.Graph = graph;
    }

    public void AddPredecessor(GraphNode predecessor)
    {
        predecessors.Add(predecessor);
        predecessor.successors.Add(this);
    }

    public void AddSuccessor(GraphNode successor)
    {
        successors.Add(successor);
        successor.predecessors.Add(this);
    }

    public void RemovePredecessor(GraphNode predecessor)
    {
        predecessors.Remove(predecessor);
        predecessor.successors.Remove(this);
    }

    public void RemoveSuccessor(GraphNode successor)
    {
        successors.Remove(successor);
        successor.predecessors.Remove(this);
    }

    private void ComputeDominators()
    {

    }
}
