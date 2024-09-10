using SimulationFramework.Drawing;
using SimulationFramework.Drawing.Shaders.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace SimulationFramework.OpenGL;

unsafe class TextureShaderArray : ShaderArray
{
    private uint tex;
    private int sizeInElements;
    private byte[] bufferData;
    private ShaderVariable uniform;
    private int lengthVarLocation;

    public unsafe TextureShaderArray(uint program, ShaderVariable uniform) : base(program, uniform)
    {
        this.uniform = uniform;

        fixed (uint* texPtr = &tex)
        {
            glGenTextures(1, texPtr);
        }

        fixed (byte* namePtr = Encoding.UTF8.GetBytes($"_{uniform.Name}_length"))
        {
            lengthVarLocation = glGetUniformLocation(program, namePtr);
        }
    }

    public override unsafe void Bind(UniformHandler handler)
    {
        var slot = handler.textureSlot++;
        glActiveTexture(GL_TEXTURE0 + (uint)slot);
        glBindTexture(GL_TEXTURE_2D, tex);
        glUniform1i(handler.GetUniformLocation(uniform), slot);
        glUniform1ui(lengthVarLocation, (uint)sizeInElements);
    }

    protected override Field[] CreateFields(uint program, ShaderVariable uniform, out int arrayStride, out int bufferStride)
    {
        return TexturePackedField.PackFields(uniform, out arrayStride, out bufferStride)
            .Select(f => f.GetField())
            .ToArray();
    }


    private void SetTextureSize(int capacityInElements)
    {
        var graphics = Application.GetComponent<GLGraphics>();

        int capacityInTexels = capacityInElements * (bufferStride / 16);

        int width = Math.Min(capacityInTexels, graphics.MaxTextureSize);
        int height = (int)MathF.Ceiling(capacityInTexels / (float)graphics.MaxTextureSize);

        glBindTexture(GL_TEXTURE_2D, this.tex);
        glTexImage2D(GL_TEXTURE_2D, 0, (int)GL_RGBA32UI, width, height, 0, GL_RGBA_INTEGER, GL_UNSIGNED_INT, null);
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, (int)GL_NEAREST);
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, (int)GL_NEAREST);

        sizeInElements = capacityInElements;
        bufferData = new byte[sizeInElements * bufferStride];
    }

    public override unsafe void ReadData(void* outData, int count)
    {
        throw new NotSupportedException("shader array reads not supported!");
    }

    public override unsafe void WriteData(void* arrayData, int count)
    {
        if (sizeInElements < count)
            SetTextureSize(count);

        sizeInElements = count;

        var graphics = Application.GetComponent<GLGraphics>();

        int capacityInTexels = count * (bufferStride / 16);

        int width = Math.Min(capacityInTexels, graphics.MaxTextureSize);
        int height = (int)MathF.Ceiling(capacityInTexels / (float)graphics.MaxTextureSize);

        glBindTexture(GL_TEXTURE_2D, this.tex);
        fixed (byte* bufferData = this.bufferData)
        {
            CopyArrayToBuffer(arrayData, bufferData, count);
            glTexSubImage2D(GL_TEXTURE_2D, 0, 0, 0, width, height, GL_RGBA_INTEGER, GL_UNSIGNED_INT, bufferData);
        }
    }

    public class TexturePackedField
    {
        public int elementOffset;
        public int channelOffset;
        public int channelCount;
        public int arrayOffset;
        public string Name;
        public string NestedName;
        public ShaderVariable Variable;

        public Field GetField()
        {
            return new()
            {
                arrayOffset = arrayOffset,
                bufferOffset = elementOffset * 16 + channelOffset * 4,
                size = channelCount * 4,
            };
        }

        public static TexturePackedField[] PackFields(ShaderVariable uniform, out int arrayStride, out int bufferStride)
        {
            List<TexturePackedField> fields = [];

            int currentElement = 0;
            int currentChannel = 0;
            int arrayOffset = 0;

            CreateFieldsHelper((uniform.Type as ShaderArrayType)!.ElementType);

            if (currentChannel > 0)
            {
                currentChannel = 0;
                currentElement++;
            }

            arrayStride = arrayOffset;
            bufferStride = currentElement * 16;

            return fields.ToArray();

            void CreateFieldsHelper(ShaderType type, string name = "", string nestedName = "")
            {
                if (type is ShaderStructureType structureType)
                {
                    foreach (var field in structureType.structure.fields)
                    {
                        string nestName = nestedName;
                        if (nestName.Length > 0)
                            nestName += ".";
                        nestName += field.Name.value;

                        CreateFieldsHelper(field.Type, field.Name.value, nestName);
                    }
                    return;
                }

                if (type.GetPrimitiveKind() is ShaderPrimitiveKind primitive)
                {
                    int channels = primitive switch
                    {
                        ShaderPrimitiveKind.Bool => 1,
                        ShaderPrimitiveKind.Float => 1,
                        ShaderPrimitiveKind.Float2 => 2,
                        ShaderPrimitiveKind.Float3 => 3,
                        ShaderPrimitiveKind.Float4 => 4,
                        ShaderPrimitiveKind.Int => 1,
                        ShaderPrimitiveKind.Int2 => 2,
                        ShaderPrimitiveKind.Int3 => 3,
                        ShaderPrimitiveKind.Int4 => 4,
                        _ => throw new($"type {primitive} not allowed in array!"),
                    };

                    if (currentChannel + channels > 4)
                    {
                        currentElement++;
                        currentChannel = 0;
                    }

                    fields.Add(new TexturePackedField()
                    {
                        Name = name,
                        NestedName = nestedName,
                        Variable = uniform,
                        elementOffset = currentElement,
                        channelOffset = currentChannel,
                        channelCount = channels,
                        arrayOffset = arrayOffset,
                    });

                    arrayOffset += ShaderType.CalculateTypeSize(type);
                    currentChannel += channels;

                    return;
                }

                throw new NotSupportedException($"type {type} is not supported in arrays!");
            }
        }
    }
}