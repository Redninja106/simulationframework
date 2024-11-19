using SimulationFramework.Drawing.Shaders;
using SimulationFramework.Drawing.Shaders.Compiler;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.OpenGL.Shaders.Compute;
internal class ComputeShaderEffect
{
    private UniformHandler uniforms;
    private uint program;
    private ShaderCompilation compilation;
    private int groupSizeX, groupSizeY, groupSizeZ;

    public unsafe ComputeShaderEffect(uint program, ShaderCompilation compilation, int groupSizeX, int groupSizeY, int groupSizeZ)
    {
        this.program = program;
        this.compilation = compilation;

        uniforms = new(program);
        this.groupSizeX = groupSizeX;
        this.groupSizeY = groupSizeY;
        this.groupSizeZ = groupSizeZ;
    }

    public unsafe void Dispatch(ComputeShader shader, int threadCountX, int threadCountY, int threadCountZ)
    {
        var graphics = Application.GetComponent<GLGraphics>();

        glUseProgram(program);
        uniforms.SetUniforms(compilation, shader);

        glUniform3ui(uniforms.GetUniformLocation("_sf_thread_count"), (uint)threadCountX, (uint)threadCountY, (uint)threadCountZ);

        uint groupsX = (uint)((threadCountX + groupSizeX - 1) / groupSizeX);
        uint groupsY = (uint)((threadCountY + groupSizeY - 1) / groupSizeY);
        uint groupsZ = (uint)((threadCountZ + groupSizeZ - 1) / groupSizeZ);

        glDispatchCompute(groupsX, groupsY, groupsZ);

        // TODO: don't mark all arrays as modified
        foreach (var variable in compilation.Variables)
        {
            if (variable.Kind == ShaderVariableKind.Uniform && variable.Type is ShaderArrayType)
            {
                Array? array = ShaderArrayManager.ResolveArray((variable.BackingMember as FieldInfo)!.GetValue(shader));
                if (array != null && graphics.arrayManager.TryGet(array, out ShaderArray? shaderArray))
                {
                    shaderArray.Synchronized = false;
                }
            }
        }

        // TODO: pass smarter barrier flags here?
        glMemoryBarrier(GL_ALL_BARRIER_BITS);
    }
}
