using SimulationFramework.ShaderLanguage.SyntaxTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.ShaderLanguage.Emitters;

internal abstract class Emitter
{
    public abstract string Emit(SyntaxTree.ShaderDocument syntaxTree);
}
