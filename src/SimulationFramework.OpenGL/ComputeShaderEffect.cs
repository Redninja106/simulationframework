﻿using SimulationFramework.Drawing.Shaders;
using SimulationFramework.Drawing.Shaders.Compiler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.OpenGL;
internal class ComputeShaderEffect
{
    public Type type;
    public uint program;
    private ShaderCompilation compilation;
    private UniformHandler uniforms;

    public ComputeShaderEffect(Type type, ShaderCompilation compilation)
    {
        this.type = type;
        this.compilation = compilation;

        var sw = new StringWriter();
        var emitter = new GLSLShaderEmitter(sw);
        emitter.Emit(compilation);
        var compiledSrc = sw.ToString();

        var src = $$"""
#version 450

layout(local_size_x = 1, local_size_y = 1, local_size_z = 1) in;

{{compiledSrc}}

void main() 
{
    RunThread(int(gl_GlobalInvocationID.x), int(gl_GlobalInvocationID.y), int(gl_GlobalInvocationID.z));
}

""";
        var shader = glCreateShader(GL_COMPUTE_SHADER);
        program = glCreateProgram();
        GeometryEffect.ShaderSource(shader, src);
        glAttachShader(program, shader);
        glLinkProgram(program);
        glDeleteShader(shader);

        this.uniforms = new(program);
    }

    public unsafe void Dispatch(ComputeShader shader, int threadCountI, int threadCountJ, int threadCountK)
    {
        Debug.Assert(shader.GetType() == type);

        glUseProgram(program);
        uniforms.SetUniforms(compilation, shader);
        glDispatchCompute((uint)threadCountI, (uint)threadCountJ, (uint)threadCountK);
        glMemoryBarrier(GL_ALL_BARRIER_BITS);
        glFinish();

        foreach (var uniform in compilation.Variables)
        {
            if (uniform.Kind != ShaderVariableKind.Uniform)
            {
                continue;
            }

            var field = (FieldInfo)uniform.BackingMember!;

            if (!field.FieldType.IsArray)
            {
                continue;
            }

            Array array = (Array)(uniform.BackingMember as FieldInfo)!.GetValue(shader)!;
            ref byte data = ref MemoryMarshal.GetArrayDataReference(array);
            void* dataPtr = Unsafe.AsPointer(ref data);

            SSBO buffer = uniforms.GetSSBO(uniform);
            buffer.Read(dataPtr, array.Length);
        }
    }
}