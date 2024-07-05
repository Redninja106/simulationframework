namespace SimulationFramework.Drawing.Shaders.Compiler.ControlFlow;

class LoopNode : ControlFlowNode, ISubgraphContainer
{
    public ControlFlowNode Header { get; set; }
    public ControlFlowNode Tail { get; set; }

    private Loop loop;

    public ControlFlowNode BreakTarget;
    public List<ControlFlowNode> BreakNodes;

    public ControlFlowNode ContinueTarget;
    public List<ControlFlowNode> ContinueNodes;

    public ControlFlowGraph Subgraph => loop.subgraph;

    public LoopNode(ControlFlowGraph graph, Loop loop) : base(graph)
    {
        this.loop = loop;

        Header = loop.header;
        Tail = loop.tail;

        ContinueTarget = Header;
        BreakTarget = loop.header.Successors.Single(n => !loop.subgraph.Nodes.Contains(n)); // Should be postdominator

        BreakNodes = loop.subgraph.Nodes.Where(b => b.Successors.SingleOrDefault(s => s == BreakTarget) is not null).ToList();
        ContinueNodes = loop.subgraph.Nodes.Where(b => b.Successors.SingleOrDefault(s => s == ContinueTarget) is not null).ToList();

        foreach (var pred in Header.Predecessors.ToArray())
        {
            if (loop.subgraph.Nodes.Contains(pred))
                continue;

            pred.RemoveSuccessor(Header);
            pred.AddSuccessor(this);
        }

        foreach (var succ in Header.Successors.ToArray())
        {
            if (loop.subgraph.Nodes.Contains(succ))
                continue;

            succ.RemovePredecessor(Header);
            succ.AddPredecessor(this);
        }
    }
}
