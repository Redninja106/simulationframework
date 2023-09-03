using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Translation;
internal class RemoveThisOnGlobalsStage : CompilerStage
{
    public RemoveThisOnGlobalsStage(CompilerContext context) : base(context)
    {
    }

    public override void Run(ShaderCompilation compilation)
    {
        foreach (var method in compilation.Methods)
        {
            var param = method.Parameters.First();
            if (param.ParameterType == context.ShaderType && param.ParameterInfo is null)
            {
                method.Parameters.Remove(param);
            }
        }
    }
}
