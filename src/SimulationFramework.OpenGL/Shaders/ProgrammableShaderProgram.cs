using SimulationFramework.Drawing.Shaders;
using SimulationFramework.Drawing.Shaders.Compiler;
using System;
using System.Linq;
using System.Reflection;

namespace SimulationFramework.OpenGL.Shaders;

class ProgrammableShaderProgram : ShaderProgram
{
    private const string defaultVert = @"
#version 330 core

layout (location = 0) in vec2 _pos;

uniform mat4 _vertex_transform;

void main() {
    gl_Position = _vertex_transform * vec4(_pos.xy, 0, 1.0);
}
";
    internal ShaderCompilation compilation;
    internal ShaderCompilation? vsCompilation;


    public ProgrammableShaderProgram(ShaderCompilation compilation, string source, ShaderCompilation? vsCompilation, string? vsSource)
        : base(GetVertexShaderSource(vsCompilation, vsSource), GetFragmentShaderSource(compilation, source))
    {

        this.compilation = compilation;
        this.vsCompilation = vsCompilation;

        if (ShaderCompiler.DumpShaders)
        {
            var vertShader = GetVertexShaderSource(vsCompilation, vsSource);
            Console.WriteLine(new string('=', 20) + " VERTEX SHADER " + new string('=', 20));
            Console.WriteLine(string.Join("\n", vertShader.Split('\n').Select((s, i) => $"{i + 1,-3:d}|{s}")));
        }

        if (ShaderCompiler.DumpShaders)
        {
            var fragShader = GetFragmentShaderSource(compilation, source);
            Console.WriteLine(new string('=', 20) + " CANVAS SHADER " + new string('=', 20));
            Console.WriteLine(string.Join("\n", fragShader.Split('\n').Select((s, i) => $"{i + 1,-3:d}|{s}")));
        }
    }

    private static string GetFragmentShaderSource(ShaderCompilation compilation, string source)
    {
        return $$"""
#version 450 core
out vec4 FragColor;

uniform mat4 _inv_transform;

in vec4 gl_FragCoord;

{{source}}

void main() {
    vec4 transformedFragCoord = _inv_transform * vec4(gl_FragCoord.x, gl_FragCoord.y, 0.0, 1.0);
    FragColor = GetPixelColor(transformedFragCoord.xy);
} 
""";
    }

    private static string GetVertexShaderSource(ShaderCompilation? compilation, string? source)
    {
        if (compilation == null || source == null)
            return defaultVert;

        return $$"""

#version 450 core

uniform mat4 _vertex_transform;

{{source}}

void main() {
gl_Position = _vertex_transform * GetVertexPosition();
}

""";
    }
}
