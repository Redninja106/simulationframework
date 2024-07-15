namespace SimulationFramework.Drawing.Shaders.Compiler.ControlFlow;

class DummyNode(bool exit = false) : ControlFlowNode
{
    public override bool PrecedesExit => exit;
}