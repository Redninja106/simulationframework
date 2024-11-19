using SimulationFramework.Drawing.Shaders;
using SimulationFramework.Drawing.Shaders.Compiler;
using System.Runtime.CompilerServices;

namespace SimulationFramework.OpenGL.Shaders.Compute;

class ExplicitGroupSizeComputeShaderProgram : ComputeShaderProgram
{
    private uint program;
    private ConditionalWeakTable<ComputeShader, ComputeShaderEffect> effects = [];
    ThreadGroupSizeAttribute groupSize;

    public ExplicitGroupSizeComputeShaderProgram(string shaderVersion, ShaderCompilation compilation, ThreadGroupSizeAttribute groupSize) : base(shaderVersion, compilation)
    {
        this.groupSize = groupSize;

        program = CreateProgram(groupSize.CountX, groupSize.CountY, groupSize.CountZ);
    }

    public override ComputeShaderEffect GetEffect(ComputeShader shader, int threadCountX, int threadCountY, int threadCountZ)
    {
        return effects.GetValue(shader, shader => new ComputeShaderEffect(program, compilation, groupSize.CountX, groupSize.CountY, groupSize.CountZ));
    }
}