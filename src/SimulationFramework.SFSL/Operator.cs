using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFSLPrototype;

public class Operator
{
    public string Symbol;
    public Precedence Precedence;

    public Operator(string symbol, Precedence precedence)
    {
        this.Symbol = symbol;
        this.Precedence = precedence;
    }
}
