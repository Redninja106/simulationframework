using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.ShaderLanguage.SyntaxTree;

internal class UnexpectedToken : TokenNode
{
    public TokenNode Token { get; set; }
    public string? Message { get; set; }

    public UnexpectedToken(TokenNode token, string? message) : base(token.Kind, token.Range, token.LeadingTrivia, token.TrailingTrivia)
    {
        this.Token = token;
        this.Message = message;
    }
}