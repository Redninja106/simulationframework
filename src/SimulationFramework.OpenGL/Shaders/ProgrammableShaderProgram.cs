using SimulationFramework.Drawing.Shaders;
using SimulationFramework.Drawing.Shaders.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SimulationFramework.OpenGL.Shaders;

class ProgrammableShaderProgram : ShaderProgram
{
    private const string defaultVert = @"
layout (location = 0) in vec2 _pos;

uniform mat4 _vertex_transform;

void main() {
    gl_Position = _vertex_transform * vec4(_pos.xy, 0, 1.0);
}
";
    internal ShaderCompilation compilation;
    internal ShaderCompilation? vsCompilation;
    
    private UniformHandler uniformHandler;

    public ProgrammableShaderProgram(string shaderVersion, ShaderCompilation compilation, string source, ShaderCompilation? vsCompilation, string? vsSource)
        : base(shaderVersion, GetVertexShaderSource(vsCompilation, vsSource), GetFragmentShaderSource(compilation, source))
    {

        this.compilation = compilation;
        this.vsCompilation = vsCompilation;

        uniformHandler = new(GetID());
    }

    public UniformHandler GetUniformHandler(CanvasShader shader, VertexShader? vertexShader)
    {
        return uniformHandler;
    }

    private static string GetFragmentShaderSource(ShaderCompilation compilation, string source)
    {
        return $$"""
precision highp float;
precision highp int;
precision highp usampler2D;

out vec4 FragColor;

uniform mat4 _inv_transform;

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

uniform mat4 _vertex_transform;

{{source}}

void main() {
    gl_Position = _vertex_transform * (vec4(1,1,1,1) * GetVertexPosition());
}

""";
    }
}
