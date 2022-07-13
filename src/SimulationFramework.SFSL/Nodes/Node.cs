namespace SimulationFramework.SFSL.Nodes;


public abstract record Node
{
    public abstract void Accept(DocumentVisitor visitor);
}