using System.Diagnostics;

namespace SimulationFramework.Drawing.Shaders.Compiler.ControlFlow;

class ConditionalNode : ControlFlowNode, ISubgraphContainer
{
    private Conditional conditional;

    public bool Inverted => conditional.inverted;
    public ControlFlowNode TrueBranch => conditional.trueBranch;
    public ControlFlowNode FalseBranch => conditional.falseBranch;
    public ControlFlowGraph Subgraph => this.conditional.subgraph;

    public ConditionalNode(ControlFlowGraph graph, ControlFlowNode startNode) : base(graph)
    {
        this.conditional = FindConditional(graph, startNode);

        foreach (var predecessor in Subgraph.EntryNode.Predecessors.ToArray())
        {
            predecessor.RemoveSuccessor(Subgraph.EntryNode);
            predecessor.AddSuccessor(this);
        }

        foreach (var sucessor in Subgraph.ExitNode.Successors.ToArray())
        {
            sucessor.RemovePredecessor(Subgraph.ExitNode);
            sucessor.AddPredecessor(this);
        }
    }

    Conditional FindConditional(ControlFlowGraph graph, ControlFlowNode startNode)
    {
        // look for idom & two preds
        graph.RecomputeDominators();
        // var endNode = graph.DepthFirstSearch(node =>
        // {
        //     return node.immediateDominator == startNode && node.Predecessors.Count >= 2;
        // }, startNode, true);

        var endNode = FindConditionalConvergence(graph, startNode);

        Debug.Assert(endNode is not null);

        var firstSuccessor = startNode.Successors[0];
        var secondSuccessor = startNode.Successors[1];

        List<ControlFlowNode> nodes = graph.Nodes.Where(node => node.dominators.Contains(startNode) && !node.dominators.Contains(endNode)).ToList();
        nodes.Add(endNode);

        bool inverted = false;
        if (firstSuccessor == endNode)
        {
            (firstSuccessor, secondSuccessor) = (secondSuccessor, firstSuccessor);
            inverted = true;
        }

        if (secondSuccessor == endNode)
        {
            secondSuccessor = null;
        }

        return new Conditional()
        {
            inverted = inverted,
            trueBranch = firstSuccessor,
            falseBranch = secondSuccessor,
            subgraph = ControlFlowGraph.CreateConditionalSubgraph(this, startNode, endNode)
        };
    }

    ControlFlowNode FindConditionalConvergence(ControlFlowGraph graph, ControlFlowNode startNode)
    {
        var firstSuccessor = startNode.Successors[0];
        var secondSuccessor = startNode.Successors[1];

        // this could be better.
        var endnode = graph.BreadthFirstSearch(node =>
        {
            return graph.BreadthFirstSearch(otherNode => otherNode == node, secondSuccessor) is not null;
        }, firstSuccessor);

        return endnode;
    }

    // connections may not be to the correct subgraph after being
    // simplified, walk up bad connection's parent until we are good
    public void FixConnections()
    {
        conditional.trueBranch = FixConnectionsHelper(conditional.trueBranch);

        if (conditional.falseBranch is not null)
            conditional.falseBranch = FixConnectionsHelper(conditional.falseBranch);

        ControlFlowNode FixConnectionsHelper(ControlFlowNode node)
        {
            while (node.Graph != Subgraph)
            {
                node = node.Graph.Parent ?? throw new Exception();
            }
            return node;
        }
    }
}