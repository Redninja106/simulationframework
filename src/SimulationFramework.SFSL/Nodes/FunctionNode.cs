using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.SFSL.Nodes;

public record FunctionNode(
    TokenNode ReturnType, 
    TokenNode Name, 
    TokenNode OpenParen, 
    FunctionParameterNode[] Parameters, 
    TokenNode CloseParen, 
    Statement Body) : Node()
{
    public override void Accept(DocumentVisitor visitor)
    {
    }
}
