using SimulationFramework.ShaderLanguage.Emitters;
using SimulationFramework.ShaderLanguage.Emitters.Hlsl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.ShaderLanguage;
public static class Compiler
{
    public static CompilationResult Compile(string source, CompilationTarget target)
    {
        var result = new CompilationResult();
        
        var tokens = Scanner.Scan(source);

        var ast = Parser.Parse(ref tokens);

        result.Output = GetEmitter(target).Emit(ast);

        return result;
    }

    private static Emitter GetEmitter(CompilationTarget target)
    {
        return new HLSLEmitter();
    }
}
