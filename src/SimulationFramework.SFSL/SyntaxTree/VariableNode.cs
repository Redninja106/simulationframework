using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.ShaderLanguage.SyntaxTree;
internal class GlobalVariableNode : DeclarationNode
{
    public TokenNode Modifier { get; set; }
    public TypeNode Type { get; set; }
    public TokenNode Name { get; set; }
    public TokenNode Semicolon { get; set; }

    public GlobalVariableNode(TypeNode type, TokenNode name, TokenNode semicolon)
    {
        Type = type;
        Name = name;
        Semicolon = semicolon;
    }

    public static GlobalVariableNode Parse(ref TokenStream stream)
    {
        var type = TypeNode.Parse(ref stream);
        var name = stream.ReadOrMissing(TokenKind.Identifier);
        var semicolon = stream.ReadOrMissing(TokenKind.Semicolon);

        return new(type, name, semicolon);
    }

    public override void Accept(SyntaxTreeVisitor visitor)
    {
        visitor.VisitVariable(this);
    }

    public override void VisitChildren(SyntaxTreeVisitor visitor)
    {
        Type.Accept(visitor);
        Name.Accept(visitor);
        Semicolon.Accept(visitor);
    }
}
