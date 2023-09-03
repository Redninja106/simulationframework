using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler;

internal class CompilerPipeline
{
    private readonly CompilerStage[] stages;

    public CompilerPipeline(CompilerStage[] stages)
    {
        this.stages = stages;
    }

    public void Run(ShaderCompilation compilation)
    {
        foreach (var stage in stages)
        {
            stage.Run(compilation);
        }
    }
}