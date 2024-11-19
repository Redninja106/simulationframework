using SimulationFramework.Drawing.Shaders;
using SimulationFramework.Drawing.Shaders.Compiler;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace SimulationFramework.OpenGL.Shaders.Compute;

class AutoGroupSizeComputeShaderProgram : ComputeShaderProgram
{
    private uint[] programs = new uint[8];
    private ConditionalWeakTable<ComputeShader, ComputeShaderEffect?[]> effects = [];

    public AutoGroupSizeComputeShaderProgram(string shaderVersion, ShaderCompilation compilation) : base(shaderVersion, compilation)
    {
    }


    public override ComputeShaderEffect GetEffect(ComputeShader shader, int threadCountX, int threadCountY, int threadCountZ)
    {
        int index = GetIndexForThreadCount(threadCountX, threadCountY, threadCountZ);
        ComputeShaderEffect?[] effectArray = effects.GetValue(shader, _ => new ComputeShaderEffect[8]);

        if (effectArray[index] is null)
        {
            var (groupSizeX, groupSizeY, groupSizeZ) = index switch
            {
                0 => (1, 1, 1),
                1 => (64, 1, 1),
                2 => (1, 64, 1),
                3 => (8, 8, 1),
                4 => (1, 1, 64),
                5 => (8, 1, 8),
                6 => (1, 8, 8),
                7 => (4, 4, 4),
                _ => throw new UnreachableException()
            };

            if (programs[index] == 0)
            {
                programs[index] = CreateProgram(groupSizeX, groupSizeY, groupSizeZ);
            }

            effectArray[index] = new ComputeShaderEffect(programs[index], compilation, groupSizeX, groupSizeY, groupSizeZ);
        }

        return effectArray[index]!;
    }
    public int GetIndexForThreadCount(int countX, int countY, int countZ)
    {
        int index = 0;
        if (countX > 1)
        {
            index |= 1 << 0;
        }
        if (countY > 1)
        {
            index |= 1 << 1;
        }
        if (countZ > 1)
        {
            index |= 1 << 2;
        }
        return index;
    }
}
