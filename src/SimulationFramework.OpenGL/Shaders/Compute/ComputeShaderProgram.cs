using SimulationFramework.Drawing.Shaders;
using SimulationFramework.Drawing.Shaders.Compiler;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace SimulationFramework.OpenGL.Shaders.Compute;

abstract class ComputeShaderProgram
{
    public readonly ShaderCompilation compilation;
    private string compiledSource;
    private string shaderVersion;

    public abstract ComputeShaderEffect GetEffect(ComputeShader shader, int threadCountX, int threadCountY, int threadCountZ);

    public ComputeShaderProgram(string shaderVersion, ShaderCompilation compilation)
    {
        if (!Application.GetComponent<GLGraphics>().HasGLES31)
        {
            throw new NotSupportedException("Compute shaders are not supported on this device!");
        }

        this.compilation = compilation;
        this.shaderVersion = shaderVersion;

        var sw = new StringWriter();
        var emitter = new GLSLShaderEmitter(sw);
        emitter.Emit(compilation);
        compiledSource = sw.ToString();
    }

    private string GetShaderSource(int groupSizeX, int groupSizeY, int groupSizeZ)
    {
        string result = $$"""
{{shaderVersion}}

layout(local_size_x = {{groupSizeX}}, local_size_y = {{groupSizeY}}, local_size_z = {{groupSizeZ}}) in;

uniform uvec3 _sf_thread_count;

{{compiledSource}}

void main() 
{
    if (any(lessThanEqual(_sf_thread_count, gl_GlobalInvocationID))) {
        return;
    }

    RunThread(int(gl_GlobalInvocationID.x), int(gl_GlobalInvocationID.y), int(gl_GlobalInvocationID.z));
}
""";

        ShaderCompiler.LogShaderSource($"COMPUTE SHADER {groupSizeX}x{groupSizeY}x{groupSizeZ}", result);

        return result;
    }

    protected unsafe uint CreateProgram(int groupSizeX, int groupSizeY, int groupSizeZ)
    {
        string source = GetShaderSource(groupSizeX, groupSizeY, groupSizeZ);
        var shader = glCreateShader(GL_COMPUTE_SHADER);
        uint program = glCreateProgram();
        ShaderProgram.ShaderSource(shader, source);
        glAttachShader(program, shader);
        glLinkProgram(program);
        int success = 0;
        glGetProgramiv(program, GL_LINK_STATUS, &success);
        if (success == 0)
        {
            byte[] infoLog = new byte[512];
            fixed (byte* infoLogPtr = infoLog)
            {
                glGetProgramInfoLog(program, 512, null, infoLogPtr);
                throw new(Marshal.PtrToStringUTF8((nint)infoLogPtr));
            }
        }
        glDeleteShader(shader);
        return program;
    }

}
