using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimulationFramework.SFSL.Nodes;
using SimulationFramework.SFSL.Emit;
using SimulationFramework.SFSL.Emit.HLSL;

namespace SimulationFramework.SFSL;

public class Compiler
{
    public CompilationResult Compile(string source)
    {
        var context = new CompilationContext();

        var lexer = new Lexer(context, source);

        var documentBuilder = new DocumentBuilder(context);

        var reader = new TokenReader(lexer.GetTokens());
        var document = documentBuilder.Build(reader);

        var errorChecker = new DocumentErrorChecker(context);

        document.Accept(errorChecker);

        Emitter emitter = new HlslEmitter(Console.Out);

        document.Accept(emitter);

        emitter.Flush();

        return context.CreateResult();
    }
}
