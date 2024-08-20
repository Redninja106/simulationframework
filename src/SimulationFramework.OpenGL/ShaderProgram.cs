using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SimulationFramework.OpenGL;

class ShaderProgram
{
    private uint programID;

    public unsafe ShaderProgram(string vsSource, string fsSource)
    {
        var vs = glCreateShader(GL_VERTEX_SHADER);
        var fs = glCreateShader(GL_FRAGMENT_SHADER);
        ShaderSource(vs, vsSource);

        ShaderSource(fs, fsSource);
        programID = glCreateProgram();
        glAttachShader(programID, vs);
        glAttachShader(programID, fs);
        glLinkProgram(programID);

        int success = 0;
        glGetProgramiv(programID, GL_LINK_STATUS, &success);
        if (success == 0)
        {
            byte[] infoLog = new byte[512];
            fixed (byte* infoLogPtr = infoLog)
            {
                glGetProgramInfoLog(programID, 512, null, infoLogPtr);
                throw new(Marshal.PtrToStringUTF8((nint)infoLogPtr));
            }
        }
    }

    internal static unsafe void ShaderSource(uint shader, string source)
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

    public unsafe int GetUniformLocation(ReadOnlySpan<byte> uniformName)
    {
        return glGetUniformLocation(programID, ToPointer(uniformName));

        static unsafe byte* ToPointer(ReadOnlySpan<byte> str)
        {
            fixed (byte* a = str)
            {
                return a;
            }
        }
    }

    public void Use()
    {
        glUseProgram(this.programID);
    }

    internal uint GetID()
    {
        return programID;
    }

}