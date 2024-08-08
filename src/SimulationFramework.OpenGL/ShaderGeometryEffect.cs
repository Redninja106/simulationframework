using ImGuiNET;
using SimulationFramework.Drawing;
using SimulationFramework.Drawing.Shaders;
using SimulationFramework.Drawing.Shaders.Compiler;
using System;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Threading.Tasks;

namespace SimulationFramework.OpenGL;
internal class ShaderGeometryEffect : GeometryEffect
{
    public CanvasShader? Shader { get; internal set; }
    public VertexShader? VertexShader { get; internal set; }
    internal ShaderCompilation compilation;
    internal ShaderCompilation? vsCompilation;

    private const string vert = @"
#version 330 core
layout (location = 0) in vec2 _pos;

uniform mat4 _vertex_transform;

void main() {
    gl_Position = _vertex_transform * vec4(_pos.xy, 0, 1.0);
}
";

    uint program;
    UniformHandler uniformHandler;

    public ShaderGeometryEffect(ShaderCompilation compilation, string compiledSource, ShaderCompilation? vsCompilation, string? vsSource)
    {
        this.compilation = compilation;
        this.vsCompilation = vsCompilation;
        
        string fragShader = $$"""
#version 450 core
out vec4 FragColor;

uniform mat4 _inv_transform;

in layout(origin_upper_left) vec4 gl_FragCoord;

{{compiledSource}}

void main() {
    vec4 transformedFragCoord = _inv_transform * vec4(gl_FragCoord.x, gl_FragCoord.y, 0.0, 1.0);
    FragColor = GetPixelColor(transformedFragCoord.xy);
} 
""";

        string vertShader = vert;

        if (vsSource != null)
        {
            if (vsCompilation!.EntryPoint.BackingMethod!.GetCustomAttribute<UseClipSpaceAttribute>() != null)
            {
                vertShader = $$"""

#version 450 core

{{vsSource}}

void main() {
    gl_Position = GetVertexPosition();
}

""";
            }
            else
            {
                vertShader = $$"""

#version 450 core

uniform mat4 _vertex_transform;

{{vsSource}}

void main() {
    gl_Position = _vertex_transform * GetVertexPosition();
}

""";
            }
        }

        if (GLGraphicsProvider.DumpShaders)
        {
            Console.WriteLine(new string('=', 20) + " CANVAS SHADER " + new string('=', 20));
            Console.WriteLine(string.Join("\n", fragShader.Split('\n').Select((s, i) => $"{i+1,-3:d}|{s}")));
            if (vsSource != null)
            {
                Console.WriteLine(new string('=', 20) + " VERTEX SHADER " + new string('=', 20));
                Console.WriteLine(string.Join("\n", vertShader.Split('\n').Select((s, i) => $"{i + 1,-3:d}|{s}")));
            }
        }
        program = MakeProgram(vertShader, fragShader);

        uniformHandler = new(program);
    }

    public unsafe override void ApplyState(CanvasState state, Matrix4x4 matrix)
    {
        var loc = glGetUniformLocation(program, ToPointer("_vertex_transform"u8));
        glUniformMatrix4fv(loc, 1, 0, (float*)&matrix);
        var loc2 = glGetUniformLocation(program, ToPointer("_inv_transform"u8));

        Matrix3x2.Invert(Shader.TransformMatrix * state.Transform, out var inv);
        Matrix4x4 invTransform = new(
            inv.M11, inv.M12, 0, 0,
            inv.M21, inv.M22, 0, 0,
            0, 0, 1, 0,
            inv.M31, inv.M32, 0, 1
            );
        glUniformMatrix4fv(loc2, 1, 0, (float*)&invTransform);

        uniformHandler.Reset();

        foreach (var variable in compilation.Variables)
        {
            if (variable.Kind == ShaderVariableKind.Uniform)
            {
                uniformHandler.SetUniform(variable, Shader);
            }
        }

        foreach (var variable in vsCompilation?.Variables ?? [])
        {
            if (variable.Kind == ShaderVariableKind.Uniform)
            {
                uniformHandler.SetUniform(variable, VertexShader!);
            }
        }
    }

    private unsafe byte* ToPointer(ReadOnlySpan<byte> str)
    {
        fixed (byte* a = str)
        {
            return a;
        }
    }

    public override void Use()
    {
        glUseProgram(program);
    }
}
