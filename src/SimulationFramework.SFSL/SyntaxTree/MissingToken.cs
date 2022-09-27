using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.ShaderLanguage.SyntaxTree;

internal class MissingToken : TokenNode
{
    public string? Message { get; private set; } = null;

    public MissingToken(TokenKind kind, Range range, string? message = null) : base(kind, range, range.Start..range.Start, range.End..range.End)
    {
        this.Message = message;
    }
}