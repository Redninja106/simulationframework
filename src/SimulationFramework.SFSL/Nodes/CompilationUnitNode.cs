using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.SFSL.Nodes;

public record CompilationUnitNode() : Node()
{
    public List<Node> Children { get; } = new();

    public void AddChild(Node child)
    {
        Children.Add(child);
    }

    public override void Accept(DocumentVisitor visitor)
    {
        visitor.Visit(this);

        foreach (var child in Children)
        {
            child.Accept(visitor);
        }
    }
}