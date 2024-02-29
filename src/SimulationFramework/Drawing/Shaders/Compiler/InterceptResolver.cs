using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler;
internal class InterceptResolver
{
    private readonly CompilerContext context;
    private readonly ShaderIntrinsicsManager intrinsicManager;

    public InterceptResolver(CompilerContext context, ShaderIntrinsicsManager intrinsicManager)
    {
        this.context = context;
        this.intrinsicManager = intrinsicManager;
    }
}