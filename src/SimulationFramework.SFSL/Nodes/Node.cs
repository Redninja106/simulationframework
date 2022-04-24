namespace SFSLPrototype.Nodes;


public abstract record Node
{
    public abstract void Accept(DocumentVisitor visitor);
}