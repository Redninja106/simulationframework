using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("BlankSimulation")]

namespace SimulationFramework.Shaders.Compiler.Branching;

internal abstract class Branch
{
    private readonly List<Branch> predecessors = new();
    private readonly List<Branch> successors = new();
    private readonly List<Branch> dominators = new();

    public IReadOnlyList<Branch> Predecessors => predecessors;
    public IReadOnlyList<Branch> Successors => successors;
    public IReadOnlyList<Branch> Dominators => dominators;

    public BranchGraph Graph { get; }

    public Branch(BranchGraph graph)
    {
        Graph = graph;
    }

    public void AddPredecessor(Branch predecessor)
    {
        predecessors.Add(predecessor);
        predecessor.successors.Add(this);
    }

    public void AddSuccessor(Branch successor)
    {
        successors.Add(successor);
        successor.predecessors.Add(this);
    }

    public void RemovePredecessor(Branch predecessor)
    {
        predecessors.Remove(predecessor);
        predecessor.successors.Remove(this);
    }

    public void RemoveSuccessor(Branch successor)
    {
        successors.Remove(successor);
        successor.predecessors.Remove(this);
    }

    internal void ComputeDominators()
    {
        
    }
}
