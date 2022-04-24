using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFSLPrototype;

public enum Precedence
{
    None = 0,
    Assignment,
    Conditional,
    ConditionalOr,
    ConditionalAnd,
    LogicalOr,
    LogicalXor,
    LogicalAnd,
    Equality,
    Relational,
    Shift,
    Additive,
    Multiplicative,
    Switch,
    Unary,
    Primary
}
