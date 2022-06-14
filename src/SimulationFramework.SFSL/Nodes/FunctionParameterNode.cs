using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.SFSL.Nodes;

public record FunctionParameterNode(TokenNode Type, TokenNode Name, TokenNode Comma) : Node
{
    public override void Accept(DocumentVisitor visitor)
    {
    }
}
