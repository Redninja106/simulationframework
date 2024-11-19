using SimulationFramework.Drawing;
using SimulationFramework.Drawing.Shaders;
using SimulationFramework.Drawing.Shaders.Compiler;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace SimulationFramework.OpenGL;

class UniformHandler
{
    private readonly Dictionary<ShaderVariable, int> variableUniformLocations = [];
    private readonly Dictionary<string, int> stringUniformLocations = [];
    private readonly uint program;
    
    internal int textureSlot, bufferSlot;

    public uint ShaderProgram => program;

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
                SetArrayUniform(uniform, value);
                break;
        }
    }

    private unsafe void SetArrayUniform(ShaderVariable uniform, object? value)
    {
        var graphics = Application.GetComponent<GLGraphics>();

        Array? array = ShaderArrayManager.ResolveArray(value);

        if (array is null)
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
            return;
        }

        ShaderArray shaderArray = graphics.arrayManager.Get(array);
        if (shaderArray.Synchronized)
        {
            graphics.arrayManager.UploadArray(array);
        }

        shaderArray.Bind(uniform, this);

        if (array.Rank > 1)
        {
            for (int i = 0; i < array.Rank; i++)
            {
                int location = GetUniformLocation($"_sf_{uniform.Name}_size_{"xyzw"[i]}");
                glUniform1ui(location, (uint)array.GetLength(i));
            }
        }
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
        if (nameOverride == null && variableUniformLocations.TryGetValue(uniform, out int result))
        {
            return result;
        }

        var graphics = Application.GetComponent<GLGraphics>();
        if (graphics.HasGLES31 && uniform.Type is ShaderArrayType arrayType)
        {
            fixed (byte* namePtr = Encoding.UTF8.GetBytes(nameOverride ?? ("_buf_" + uniform.Name.value)))
            {
                int index = (int)glGetProgramResourceIndex(program, GL_SHADER_STORAGE_BLOCK, namePtr);
                variableUniformLocations[uniform] = index;
                return index;
            }
        }

        fixed (byte* namePtr = Encoding.UTF8.GetBytes(nameOverride ?? uniform.Name.value))
        {
            result = glGetUniformLocation(program, namePtr);
            variableUniformLocations[uniform] = result;
        }

        return result;
    }

    public unsafe int GetUniformLocation(string name)
    {
        int location;

        if (stringUniformLocations.TryGetValue(name, out location))
        {
            return location;
        }

        fixed (byte* namePtr = Encoding.UTF8.GetBytes(name))
        {
            location = glGetUniformLocation(program, namePtr);
            stringUniformLocations[name] = location;
        }

        return location;
    }
}
