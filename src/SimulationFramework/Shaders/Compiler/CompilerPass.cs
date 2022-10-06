using SimulationFramework.Shaders.Compiler.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders.Compiler;

internal class CompilerPass
{
    private readonly List<CompilerRule> rules = new();

    public void Apply(CompilationContext context)
    {
        foreach (var rule in rules)
        {

        }
    }
}
