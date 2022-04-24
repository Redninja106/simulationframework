using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFSLPrototype.Nodes;

public record BlockStatementNode(TokenNode OpenBracket, Statement[] Nodes, TokenNode CloseBracket) : Statement
{
    public override void Accept(DocumentVisitor visitor)
    {
        visitor.Visit(this);
        foreach (var node in Nodes)
        {
            node.Accept(visitor);
        }
    }
}
