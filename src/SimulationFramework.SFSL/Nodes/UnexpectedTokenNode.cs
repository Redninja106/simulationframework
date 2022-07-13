using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.SFSL.Nodes;

public record UnexpectedTokenNode(TokenNode Unexpected) : Node()
{
    public override void Accept(DocumentVisitor visitor)
    {
        visitor.Visit(this);

        Unexpected.Accept(visitor);
    }
}
