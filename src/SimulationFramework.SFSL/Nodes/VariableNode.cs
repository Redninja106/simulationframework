using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.SFSL.Nodes;

public record VariableNode(TokenNode InOrOutModifier, TokenNode Type, TokenNode Name, TokenNode Semicolon) : Node
{
    public VariableModifier Modifier => InOrOutModifier?.Kind switch
    {
        TokenKind.InKeyword => VariableModifier.In,
        TokenKind.OutKeyword => VariableModifier.Out,
        null => VariableModifier.None,
        _ => throw new Exception(),
    };

    public override void Accept(DocumentVisitor visitor)
    {
        visitor.Visit(this);

        InOrOutModifier?.Accept(visitor);
        Type.Accept(visitor);
        Name.Accept(visitor);
        Semicolon.Accept(visitor);
    }
}
