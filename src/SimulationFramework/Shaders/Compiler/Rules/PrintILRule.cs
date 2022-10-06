using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders.Compiler.Rules;
internal class PrintILRule : CompilerRule
{
    public override void CheckMethod(CompilationContext context, CompiledMethod compiledMethod)
    {
        foreach (var i in compiledMethod.Disassembly.instructions)
        {
            Console.WriteLine(i);
        }

        Console.WriteLine(new string('=', 100));

        base.CheckMethod(context, compiledMethod);
    }
}
