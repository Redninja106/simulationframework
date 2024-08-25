﻿using SimulationFramework.Drawing;
using SimulationFramework.Drawing.Shaders.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.OpenGL;
unsafe internal class GLGeometry : IGeometry
{
    private Dictionary<Type, VertexBuffer> vertexBuffers = [];
    private int count;
    private uint indexBuffer;
    private GLGraphics graphics;

    public GLGeometry(GLGraphics graphics)
    {
        this.graphics = graphics;
    }

    public unsafe void SetIndices(ReadOnlySpan<uint> indices)
    {
        uint buffer = 0;
        glCreateBuffers(1, &buffer);
        indexBuffer = buffer;
        fixed (uint* indicesPtr = indices) 
        {
            glNamedBufferStorage(buffer, indices.Length * sizeof(uint), indicesPtr, 0);
        }
        count = indices.Length;
    }

    public void AddVertexBuffer<T>(ReadOnlySpan<T> vertices)
        where T : unmanaged
    {
        fixed (T* firstVertex = vertices)
        {
            uint buffer = 0;
            glCreateBuffers(1, &buffer);
            glNamedBufferStorage(buffer, vertices.Length * sizeof(T), firstVertex, 0);

            vertexBuffers.Add(typeof(T), new(buffer, vertices.Length, sizeof(T), typeof(T)));

            if (indexBuffer != 0)
            {
                count = vertices.Length;
            }
        }
    }

    public void Dispose()
    {
        if (indexBuffer != 0)
        {
            uint idxBuf = indexBuffer;  
            glDeleteBuffers(1, &idxBuf);
        }

        foreach (var (_, vb) in vertexBuffers)
        {
            glDeleteBuffers(1, &vb.buffer);
        }
    }

    public uint[]? GetIndices()
    {
        throw new NotImplementedException();
    }
    public TVertex[] GetVertices<TVertex>() where TVertex : unmanaged
    {
        throw new NotImplementedException();
    }

    public void Draw(ref readonly CanvasState state)
    {
        var vb = vertexBuffers.Single().Value;
        var stream = graphics.streams.GetCustomVertexGeometryStream(vb.type, in state);
        glBindBuffer(GL_ARRAY_BUFFER, vb.buffer);
        if (indexBuffer != 0)
        {
            glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, indexBuffer);
        }

        stream.BindVertexArray();
        if (indexBuffer != 0)
        {
            glDrawElements(GL_TRIANGLES, count, GL_UNSIGNED_INT, null);
        }
        else
        {
            glDrawArrays(GL_TRIANGLES, 0, count);
        }
    }

    internal void DrawInstances<TInstances>(GLCanvas canvas, ReadOnlySpan<Matrix3x2> instances)
    {
        throw new NotImplementedException();
    }

    internal void DrawInstances<TInstance>(GLCanvas canvas, ReadOnlySpan<TInstance> instances) where TInstance : unmanaged
    {
        throw new NotImplementedException();
    }

    struct VertexBuffer
    {
        public uint buffer;
        public int count;
        public int stride;
        public Type type;

        public VertexBuffer(uint buffer, int count, int stride, Type type)
        {
            this.buffer = buffer;
            this.count = count;
            this.stride = stride;
            this.type = type;
        }
    }
}
