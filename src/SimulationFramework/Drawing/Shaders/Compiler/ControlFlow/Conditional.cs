namespace SimulationFramework.Drawing.Shaders.Compiler.ControlFlow;

class Conditional
{
    public bool inverted;
    public ControlFlowNode trueBranch;
    public ControlFlowNode falseBranch;
    public ControlFlowGraph subgraph;
}
