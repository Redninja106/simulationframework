using SimulationFramework.ShaderLanguage.SyntaxTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.ShaderLanguage.Emitters.Hlsl;
internal class HLSLEmitter : Emitter
{
    StringBuilder result;

    public override string Emit(ShaderDocument syntaxTree)
    {
        result = new();

        foreach (var decl in syntaxTree.GetDeclarations())
        {
            switch (decl)
            {
                default:
                    break;
            }
        }

        return result.ToString();
    }
}
