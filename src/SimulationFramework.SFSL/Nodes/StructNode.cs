using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.SFSL.Nodes;

public record StructNode(TokenNode structKeyword, TokenNode identifierNode, TokenNode openBracket, VariableNode[] Members, TokenNode closeBracket) : Node
{
    public override void Accept(DocumentVisitor visitor)
    {
        visitor.Visit(this);

        foreach (var member in Members)
        {
            member.Accept(visitor);
        }
    }
}
