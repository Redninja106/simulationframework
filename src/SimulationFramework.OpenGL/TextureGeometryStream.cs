﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.OpenGL;
internal class TextureGeometryStream : GeometryStream
{
    private List<TextureVertex> vertices = [];
    private int texCoordPosition;
    private uint vao;

    public unsafe TextureGeometryStream()
    {
        fixed (uint* vaoPtr = &vao)
        {
            glGenVertexArrays(1, vaoPtr);
            glBindVertexArray(vao);

            glVertexAttribPointer(0, 2, GL_FLOAT, (byte)GL_FALSE, Unsafe.SizeOf<TextureVertex>(), null);
            glEnableVertexAttribArray(0);
            glVertexAttribPointer(1, 2, GL_FLOAT, (byte)GL_FALSE, Unsafe.SizeOf<TextureVertex>(), (void*)(sizeof(float) * 2));
            glEnableVertexAttribArray(1);

            glBindVertexArray(0);
        }
    }

    public override void BindVertexArray()
    {
        glBindVertexArray(vao);
    }

    public override void Clear()
    {
        vertices.Clear();
    }

    public override int GetVertexCount()
    {
        return vertices.Count;
    }

    public override void Upload(GeometryBuffer buffer)
    {
        buffer.UpdateData(MemoryMarshal.AsBytes(CollectionsMarshal.AsSpan(this.vertices)));
    }

    public override void WriteVertex(Vector2 position)
    {
        throw new InvalidOperationException();

        WriteVertex(position, Vector2.Zero);
    }

    public void WriteVertex(Vector2 position, Vector2 textureCoordinate)
    {
        vertices.Add(new()
        {
            position = position,
            textureCoordinate = textureCoordinate,
        });
    }

    private struct TextureVertex
    {
        public Vector2 position;
        public Vector2 textureCoordinate;
    }
}