using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.ShaderLanguage;
internal class CompilationContext
{
    private string source;

    public CompilationContext(string source)
    {
        this.source = source;
    }
}
