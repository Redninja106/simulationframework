using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFSLPrototype.Nodes;

public record BufferNode(TokenNode BufferKeyword, TokenNode OpenCaret, TokenNode Type, TokenNode CloseCaret, TokenNode Name, TokenNode Semicolon) : Node()
{
    public override void Accept(DocumentVisitor visitor)
    {
        visitor.Visit(this);

        BufferKeyword.Accept(visitor);
        OpenCaret.Accept(visitor);
        Type.Accept(visitor);
        CloseCaret.Accept(visitor);
        Semicolon.Accept(visitor);
    }
}