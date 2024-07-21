using SimulationFramework.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.OpenGL;
internal abstract class GeometryEffect
{
    public abstract void Use();
    public abstract void ApplyState(CanvasState state, Matrix4x4 matrix);

    protected static unsafe uint MakeProgram(string vsSource, string fsSource)
    {
        var vs = glCreateShader(GL_VERTEX_SHADER);
        var fs = glCreateShader(GL_FRAGMENT_SHADER);
        ShaderSource(vs, vsSource);

        ShaderSource(fs, fsSource);
        var program = glCreateProgram();
        glAttachShader(program, vs);
        glAttachShader(program, fs);
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
        return program;
    }

    private static unsafe void ShaderSource(uint shader, string source)
    {
        byte[] src = Encoding.UTF8.GetBytes(source);
        int len = src.Length;

        fixed (byte* srcPtr = src)
        {
            glShaderSource(shader, 1, &srcPtr, &len);
        }
        glCompileShader(shader);
        int success;
        glGetShaderiv(shader, GL_COMPILE_STATUS, &success);

        if (success == 0)
        {
            byte[] infoLog = new byte[512];
            fixed (byte* infoLogPtr = infoLog)
            {
                glGetShaderInfoLog(shader, 512, null, infoLogPtr);
                Console.WriteLine(Marshal.PtrToStringUTF8((nint)infoLogPtr)); ;
            }
        }
    }

    protected static unsafe int GetUniformLocation(uint program, ReadOnlySpan<byte> uniformName)
    {
        return glGetUniformLocation(program, ToPointer(uniformName));

        static unsafe byte* ToPointer(ReadOnlySpan<byte> str)
        {
            fixed (byte* a = str)
            {
                return a;
            }
        }
    }
}

class ColorGeometryEffect : GeometryEffect
{
    private const string vert = @"
#version 330 core
layout (location = 0) in vec2 aPos;
layout (location = 1) in vec4 aCol;

uniform mat4 transform;

out vec4 col;

void main()
{
    gl_Position = transform * vec4(aPos.xy, 0, 1.0);
    col = aCol;
}
";


    private const string frag = @"
#version 330 core
out vec4 FragColor;

in vec4 col;

void main()
{
    FragColor = col;
} 
";

    uint program;

    public ColorGeometryEffect()
    {
        program = MakeProgram(vert, frag);
    }

    public override unsafe void ApplyState(CanvasState state, Matrix4x4 matrix)
    {
        var loc = GetUniformLocation(program, "transform"u8);
        glUniformMatrix4fv(loc, 1, 0, (float*)&matrix);
    }


    public override void Use()
    {
        glUseProgram(program);
    }
}