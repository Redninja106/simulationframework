using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.SFSL.Nodes;

public record VariableDeclarationStatement(TokenNode Type, TokenNode Name, TokenNode EqualsOrSemicolon, Expression Initializer, TokenNode Semicolon) : Statement
{
    public override void Accept(DocumentVisitor visitor)
    {
    }
}
