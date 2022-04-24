using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFSLPrototype.Nodes;
using SFSLPrototype.Emit;
using SFSLPrototype.Emit.HLSL;

namespace SFSLPrototype;

public class Compiler
{
    public CompilationResult Compile(string source)
    {
        var context = new CompilationContext();

        var lexer = new Lexer(source);

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
