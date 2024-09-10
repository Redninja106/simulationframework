using SimulationFramework.Drawing;
using SimulationFramework.Drawing.Shaders;
using SimulationFramework.Drawing.Shaders.Compiler;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SimulationFramework.OpenGL;

class UniformHandler
{
    private readonly Dictionary<ShaderVariable, int> uniformLocations = [];
    private readonly Dictionary<ShaderVariable, ShaderArray> buffers = [];
    private readonly uint program;
    
    internal int textureSlot, bufferSlot;

    public UniformHandler(uint program)
    {
        this.program = program;
    }

    public void SetUniforms(ShaderCompilation compilation, Shader shader)
    {
        Reset();
        foreach (var variable in compilation.Variables)
        {
            if (variable.Kind == ShaderVariableKind.Uniform)
            {
                SetUniform(variable, shader);
            }
        }
    }

    public void Reset()
    {
        textureSlot = 0;
        bufferSlot = 0;
    }

    public void SetUniform(ShaderVariable uniform, object parent)
    {
        SetUniform(uniform, parent, null);
    }

    unsafe void SetUniform(ShaderVariable uniform, object parent, string? baseName)
    {
        var name = baseName is null ? uniform.Name.value : baseName + uniform.Name.value;
        var location = GetUniformLocation(uniform, name);

        var field = (FieldInfo?)uniform.BackingMember!;
        var value = field.GetValue(parent)!;

        switch (uniform.Type)
        {
            case ShaderType when uniform.Type.GetPrimitiveKind() is ShaderPrimitiveKind primitiveKind:
                SetMemberUniform(primitiveKind, value!, location);
                break;
            case ShaderStructureType structType:
                foreach (var member in structType.structure.fields)
                {
                    SetUniform(member, value, name + ".");
                }
                break;
            case ShaderArrayType arrayType:
                SetArrayUniform(uniform, arrayType, value);
                break;
        }
    }

    private unsafe void SetArrayUniform(ShaderVariable uniform, ShaderArrayType arrayType, object value)
    {
        var graphics = Application.GetComponent<GLGraphics>();
        if (!buffers.TryGetValue(uniform, out ShaderArray? shaderArray))
        {
            if (graphics.HasGLES31)
            {
                shaderArray = new SSBOShaderArray(program, uniform);
            }
            else
            {
                shaderArray = new TextureShaderArray(program, uniform);
            }
            buffers[uniform] = shaderArray;
        }

        Array array = (Array)value;
        GCHandle arrayHandle = GCHandle.Alloc(array, GCHandleType.Pinned);
        try
        {
            if (array != null)
            {
                void* ptr = Unsafe.AsPointer(ref MemoryMarshal.GetArrayDataReference(array));
                shaderArray.WriteData(ptr, array.Length);
                shaderArray.Bind(this);
            }
            else
            {
                if (graphics.HasGLES31)
                {
                    glBindBufferBase(GL_SHADER_STORAGE_BUFFER, (uint)bufferSlot, 0);
                    glBindBuffer(GL_SHADER_STORAGE_BUFFER, 0);
                    bufferSlot++;
                }
                else
                {
                    glActiveTexture(GL_TEXTURE0 + (uint)textureSlot);
                    glBindTexture(GL_TEXTURE_2D, 0);
                    glUniform1i(GetUniformLocation(uniform), textureSlot);
                    glUniform1i(GetUniformLocation(uniform, $"_{uniform.Name}_length"), 0);
                    textureSlot++;
                }
            }
        }
        finally
        {
            arrayHandle.Free();
        }

        bufferSlot++;
    }

    private void SetTextureUniform(uint id, int location)
    {
        glActiveTexture(GL_TEXTURE0 + (uint)textureSlot);
        glBindTexture(GL_TEXTURE_2D, id);
        glUniform1i(location, textureSlot);
        textureSlot++;
    }

    private unsafe void SetMemberUniform(ShaderPrimitiveKind primitive, object value, int location)
    {
        if (primitive is ShaderPrimitiveKind.Texture)
        {
            SetTextureUniform(((GLTexture?)value)?.GetID() ?? 0, location);
            return;
        }

        if (primitive is ShaderPrimitiveKind.DepthMask)
        {
            SetTextureUniform(((GLDepthMask?)value)?.GetID() ?? 0, location);
            return;
        }

        GCHandle valueHandle = GCHandle.Alloc(value, GCHandleType.Pinned);
        void* ptr = (void*)valueHandle.AddrOfPinnedObject();

        switch (primitive)
        {
            case ShaderPrimitiveKind.Bool:
                glUniform1i(location, *(byte*)ptr != 0 ? 1 : 0);
                break;
            case ShaderPrimitiveKind.Int:
                glUniform1iv(location, 1, (int*)ptr);
                break;
            case ShaderPrimitiveKind.Int2:
                glUniform2iv(location, 1, (int*)ptr);
                break;
            case ShaderPrimitiveKind.Int3:
                glUniform3iv(location, 1, (int*)ptr);
                break;
            case ShaderPrimitiveKind.Int4:
                glUniform4iv(location, 1, (int*)ptr);
                break;
            case ShaderPrimitiveKind.Float:
                glUniform1fv(location, 1, (float*)ptr);
                break;
            case ShaderPrimitiveKind.Float2:
                glUniform2fv(location, 1, (float*)ptr);
                break;
            case ShaderPrimitiveKind.Float3:
                glUniform3fv(location, 1, (float*)ptr);
                break;
            case ShaderPrimitiveKind.Float4:
                glUniform4fv(location, 1, (float*)ptr);
                break;
            case ShaderPrimitiveKind.Matrix4x4:
                glUniformMatrix4fv(location, 1, (byte)GL_FALSE, (float*)ptr);
                break;
            case ShaderPrimitiveKind.Matrix3x2:
                glUniformMatrix3x2fv(location, 1, (byte)GL_FALSE, (float*)ptr);
                break;
            default:
                throw new NotImplementedException();
        }

        valueHandle.Free();
    }

    internal unsafe int GetUniformLocation(ShaderVariable uniform, string? nameOverride = null)
    {
        if (nameOverride != null && uniformLocations.TryGetValue(uniform, out int result))
        {
            return result;
        }

        fixed (byte* namePtr = Encoding.UTF8.GetBytes(nameOverride ?? uniform.Name.value))
        {
            result = glGetUniformLocation(program, namePtr);
            uniformLocations[uniform] = result;
        }

        return result;
    }

    internal ShaderArray GetShaderArray(ShaderVariable uniform)
    {
        return buffers[uniform];
    }
}
