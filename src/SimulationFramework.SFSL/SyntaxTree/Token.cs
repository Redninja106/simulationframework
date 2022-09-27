using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.ShaderLanguage.SyntaxTree;

internal class TokenNode : Node
{
    public TokenKind Kind { get; private set; }
    public Range Range { get; private set; }
    public Range LeadingTrivia { get; private set; }
    public Range TrailingTrivia { get; private set; }

    public TokenNode(TokenKind kind, Range range, Range leadingTrivia, Range trailingTrivia)
    {
        Kind = kind;
        Range = range;
        LeadingTrivia = leadingTrivia;
        TrailingTrivia = trailingTrivia;
    }

    public static MissingToken Missing(TokenKind kind = TokenKind.Unknown, int location = 0, string? message = null)
    {
        return new MissingToken(kind, location..location, message);
    }
    
    public static UnexpectedToken Unexpected(TokenNode unexpected, string? message = null)
    {
        return new UnexpectedToken(unexpected, message);
    }

    public bool IsMissing => this is MissingToken;
    public bool IsUnexpected => this is UnexpectedToken;
    
    public override void Accept(SyntaxTreeVisitor visitor)
    {
        visitor.VisitToken(this);
    }

    public override string ToString()
    {
        return $"[{Kind}] \"{Range}\"";
    }
    
    public string ToString(ReadOnlySpan<char> source)
    {
        return new(source[Range]);
    }

    public override void VisitChildren(SyntaxTreeVisitor visitor)
    {
    }


}