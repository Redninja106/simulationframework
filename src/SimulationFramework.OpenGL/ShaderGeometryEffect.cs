using SimulationFramework.Drawing;
using SimulationFramework.Drawing.Shaders;
using SimulationFramework.Drawing.Shaders.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.OpenGL;
internal class ShaderGeometryEffect : GeometryEffect
{
    public CanvasShader? Shader { get; internal set; }
    private ShaderCompilation compilation;
    private Dictionary<ShaderVariable, int> uniformLocations = [];
    private int textureSlot, bufferSlot;
    private List<uint> buffers = [];

    private const string vert = @"
#version 330 core
layout (location = 0) in vec2 _pos;

uniform mat4 _vertex_transform;

void main() {
    gl_Position = _vertex_transform * vec4(_pos.xy, 0, 1.0);
}
";

    uint program;

    public ShaderGeometryEffect(ShaderCompilation compilation, string compiledSource)
    {
        this.compilation = compilation;

        string fragShader = $$"""
#version 450 core
out vec4 FragColor;

uniform mat4 _inv_transform;

in layout(origin_upper_left) vec4 gl_FragCoord;

{{compiledSource}}

void main() {
    vec4 transformedFragCoord = _inv_transform * vec4(gl_FragCoord.xy, 0.0, 1.0);
    FragColor = GetPixelColor(transformedFragCoord.xy);
} 
""";
        Console.WriteLine(string.Join("\n", fragShader.Split('\n').Select((s, i) => $"{i+1,-3:d}|{s}")));
        program = MakeProgram(vert, fragShader);
    }

    public unsafe override void ApplyState(CanvasState state, Matrix4x4 matrix)
    {
        var loc = glGetUniformLocation(program, ToPointer("_vertex_transform"u8));
        glUniformMatrix4fv(loc, 1, 0, (float*)&matrix);
        var loc2 = glGetUniformLocation(program, ToPointer("_inv_transform"u8));

        Matrix3x2.Invert(state.Transform, out var inv);
        Matrix4x4 invTransform = new(
            inv.M11, inv.M12, 0, 0,
            inv.M21, inv.M22, 0, 0,
            0, 0, 1, 0,
            inv.M31, inv.M32, 0, 1
            );
        glUniformMatrix4fv(loc2, 1, 0, (float*)&invTransform);

        textureSlot = 0;
        bufferSlot = 1;
        foreach (var uniform in compilation.Uniforms)
        {
            SetUniform(uniform, Shader, null);
        }
    }

    private unsafe byte* ToPointer(ReadOnlySpan<byte> str)
    {
        fixed (byte* a = str)
        {
            return a;
        }
    }

    unsafe void SetUniform(ShaderVariable uniform, object parent, string? baseName)
    {
        var name = baseName is null ? uniform.Name.value : baseName + uniform.Name.value;
        var location = GetUniformLocation(uniform, name);

        var field = (FieldInfo?)uniform.BackingMember!;
        var value = field.GetValue(parent)!;

        switch (uniform.Type)
        {
            case ShaderType when uniform.Type.GetPrimitiveKind() is PrimitiveKind primitiveKind:
                SetMemberUniform(primitiveKind, value!, location);
                break;
            case ShaderStructureType structType:
                foreach (var member in structType.structure.fields)
                {
                    SetUniform(member, value, name + ".");
                } 
                break;
            case ShaderArrayType arrayType:
                SetArrayUniform(arrayType, value);
                break;
        }
    }

    private unsafe void SetArrayUniform(ShaderArrayType arrayType, object value)
    {
        uint buffer;
        if (buffers.Count <= bufferSlot - 1)
        {
            glGenBuffers(1, &buffer);
            buffers.Add(buffer);
        }
        else
        {
            buffer = buffers[bufferSlot - 1];
        }

        // TODO: pin array

        Array array = (Array)value;
        GCHandle arrayHandle = GCHandle.Alloc(array, GCHandleType.Pinned);
        try
        {
            void* ptr = Unsafe.AsPointer(ref MemoryMarshal.GetArrayDataReference(array));
            glBindBuffer(GL_SHADER_STORAGE_BUFFER, buffer);
            int currentSize;
            glGetBufferParameteriv(GL_SHADER_STORAGE_BUFFER, GL_BUFFER_SIZE, &currentSize);
            int size = array.Length * Marshal.SizeOf(array.GetType().GetElementType()!);
            if (size != currentSize)
            {
                glBufferData(GL_SHADER_STORAGE_BUFFER, size, ptr, GL_DYNAMIC_COPY);
            }
            else
            {
                glBufferSubData(GL_SHADER_STORAGE_BUFFER, 0, size, ptr);
            }
            glBindBufferBase(GL_SHADER_STORAGE_BUFFER, (uint)bufferSlot, buffer);
            glBindBuffer(GL_SHADER_STORAGE_BUFFER, 0);
        }
        finally
        {
            arrayHandle.Free();
        }

        bufferSlot++;
    }

    private void SetTextureUniform(GLTexture? texture, int location)
    {
        glActiveTexture(GL_TEXTURE0 + (uint)textureSlot);
        glBindTexture(GL_TEXTURE_2D, texture?.GetID() ?? 0);
        glUniform1i(location, textureSlot);
        textureSlot++;
    }

    private unsafe void SetMemberUniform(PrimitiveKind primitive, object value, int location)
    {
        if (primitive is PrimitiveKind.Texture)
        {
            SetTextureUniform((GLTexture?)value, location);
            return;
        }

        GCHandle valueHandle = GCHandle.Alloc(value, GCHandleType.Pinned);
        void* ptr = (void*)valueHandle.AddrOfPinnedObject();

        switch (primitive)
        {
            case PrimitiveKind.Bool:
                throw new NotImplementedException();
            case PrimitiveKind.Int:
                glUniform1iv(location, 1, (int*)ptr);
                break;
            case PrimitiveKind.Int2:
                glUniform2iv(location, 1, (int*)ptr);
                break;
            case PrimitiveKind.Int3:
                glUniform3iv(location, 1, (int*)ptr);
                break;
            case PrimitiveKind.Int4:
                glUniform4iv(location, 1, (int*)ptr);
                break;
            case PrimitiveKind.Float:
                glUniform1fv(location, 1, (float*)ptr);
                break;
            case PrimitiveKind.Float2:
                glUniform2fv(location, 1, (float*)ptr);
                break;
            case PrimitiveKind.Float3:
                glUniform3fv(location, 1, (float*)ptr);
                break;
            case PrimitiveKind.Float4:
                glUniform4fv(location, 1, (float*)ptr);
                break;
            case PrimitiveKind.Matrix4x4:
                glUniformMatrix4fv(location, 1, (byte)GL_FALSE, (float*)ptr);
                break;
            case PrimitiveKind.Matrix3x2:
                glUniformMatrix3x2fv(location, 1, (byte)GL_FALSE, (float*)ptr);
                break;
            default:
                throw new NotImplementedException();
        }

        valueHandle.Free();
    }

    unsafe int GetUniformLocation(ShaderVariable uniform, string? nameOverride = null)
    {
        if (uniformLocations.TryGetValue(uniform, out int result))
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

    public override void Use()
    {
        glUseProgram(program);
    }
}
