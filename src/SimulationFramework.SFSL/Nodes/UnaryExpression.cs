﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.SFSL.Nodes;

public record UnaryExpression(TokenNode Operator, Expression Target) : Expression
{
}
