using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.SFSL.Nodes;

public record NestedExpression(TokenNode OpenParen, Expression expression, TokenNode CloseParen) : Expression()
{
}
