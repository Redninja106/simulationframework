using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.ShaderLanguage.SyntaxTree;
internal class TypeNode : Node
{
    public TokenNode Identifier { get; private set; }

    public TypeNode(TokenNode identifier)
    {
        Identifier = identifier;
    }

    public override void Accept(SyntaxTreeVisitor visitor)
    {
        visitor.VisitTypeNode(this);
    }

    public override void VisitChildren(SyntaxTreeVisitor visitor)
    {
        Identifier.Accept(visitor);
    }

    public static TypeNode Parse(ref TokenStream stream)
    {
        return new(stream.ReadOrMissing(TokenKind.Identifier));
    }
}
