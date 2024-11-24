using SimulationFramework.Drawing;
using SimulationFramework.Drawing.Shaders.Compiler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SimulationFramework.OpenGL;

unsafe class SSBOShaderArray : ShaderArray
{
    private uint buffer;
    private int size = 128;

    public SSBOShaderArray()
    {
    }

    public override void Bind(ShaderVariable uniform, UniformHandler handler)
    {
        glBindBufferBase(GL_SHADER_STORAGE_BUFFER, (uint)handler.bufferSlot++, buffer);
        glBindBuffer(GL_SHADER_STORAGE_BUFFER, 0);
    }

    public uint GetBuffer()
    {
        return buffer;
    }

    private void SetBufferSize(int sizeInBytes)
    {
        fixed (uint* bufferPtr = &buffer)
        {
            int currentSize = 0;
            if (buffer != 0)
            {
                glBindBuffer(GL_SHADER_STORAGE_BUFFER, buffer);
                glGetBufferParameteriv(GL_SHADER_STORAGE_BUFFER, GL_BUFFER_SIZE, &currentSize);
            }
            if (sizeInBytes != currentSize)
            {
                if (buffer != 0)
                {
                    glDeleteBuffers(1, bufferPtr);
                }

                glGenBuffers(1, bufferPtr);
                glBindBuffer(GL_SHADER_STORAGE_BUFFER, buffer);
                glBufferData(GL_SHADER_STORAGE_BUFFER, sizeInBytes, null, GL_DYNAMIC_DRAW);
            }
            size = sizeInBytes;
        }
    }

    public override void WriteData(ShaderTypeLayout layout, void* data, int count)
    {
        SetBufferSize(count * layout.bufferStride * 4);

        if (count != 0)
        {
            glBindBuffer(GL_SHADER_STORAGE_BUFFER, buffer);
            void* mappedBuf = glMapBufferRange(GL_SHADER_STORAGE_BUFFER, 0, size, GL_MAP_WRITE_BIT);
            layout.CopyArrayToBuffer(data, mappedBuf, count);
            glUnmapBuffer(GL_SHADER_STORAGE_BUFFER);
        }
    }

    public override void ReadData(ShaderTypeLayout layout, void* outData, int count)
    {
        glBindBuffer(GL_SHADER_STORAGE_BUFFER, buffer);
        void* mappedBuf = glMapBufferRange(GL_SHADER_STORAGE_BUFFER, 0, size, GL_MAP_READ_BIT);
        layout.CopyBufferToArray(outData, mappedBuf, count);
        glUnmapBuffer(GL_SHADER_STORAGE_BUFFER);
    }

    ~SSBOShaderArray()
    {
        var graphics = Application.GetComponent<GLGraphics>();
        graphics.deleteQueue.AddBuffer(this.buffer);
    }
}
