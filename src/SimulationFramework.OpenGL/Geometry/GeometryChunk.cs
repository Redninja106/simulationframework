using SimulationFramework.OpenGL.Geometry.Streams;
using System;

namespace SimulationFramework.OpenGL.Geometry;

struct GeometryChunk : IDisposable
{
    // buffers
    public GeometryBuffer vertexBuffer;
    public GeometryBuffer? indexBuffer;
    public GeometryBuffer? instanceBuffer;

    // vertex information
    public VertexLayout vertexLayout;
    public VertexLayout? instanceLayout;
    public bool triangles;
    public bool wireframe;

    // draw info
    public int offset;
    public int count;
    //public int baseVertex;
    public int instanceCount;

    public unsafe void Draw()
    {
        vertexBuffer.Bind();
        vertexLayout.Bind(0, false);

        // glPolygonMode(GL_FRONT_AND_BACK, wireframe ? GL_LINE : GL_FILL);

        if (instanceBuffer != null)
        {
            instanceBuffer.Bind();
            instanceLayout!.Bind(vertexLayout.AttributeCount, true);
        }

        uint mode = triangles ? GL_TRIANGLES : GL_LINES;
        if (indexBuffer == null)
        {
            if (instanceBuffer == null)
            {
                glDrawArrays(mode, offset, count);
            }
            else
            {
                glDrawArraysInstanced(mode, offset, count, instanceCount);
            }
        }
        else
        {
            indexBuffer.BindAsIndexBuffer();

            int indexOffset = offset * sizeof(uint);

            if (instanceBuffer == null)
            {
                glDrawElements(mode, count, GL_UNSIGNED_INT, (void*)indexOffset);
            }
            else
            {
                glDrawElementsInstanced(mode, count, GL_UNSIGNED_INT, (void*)indexOffset, instanceCount);
            }
        }
    }

    public static GeometryChunk Create(GeometryBuffer vertexBuffer, VertexLayout vertexLayout, int offset, int count, bool triangles)
    {
        return new()
        {
            vertexBuffer = vertexBuffer,
            offset = offset,
            count = count,
            triangles = triangles,
            vertexLayout = vertexLayout,
        };
    }

    public static GeometryChunk CreateIndexed(GeometryBuffer vertexBuffer, GeometryBuffer indexBuffer, VertexLayout vertexLayout, int offset, int count, bool triangles)
    {
        return new()
        {
            vertexBuffer = vertexBuffer,
            offset = offset,
            count = count,
            triangles = triangles,
            vertexLayout = vertexLayout,
            indexBuffer = indexBuffer,
        };
    }

    public void Dispose()
    {
        vertexBuffer.Dispose();
        indexBuffer?.Dispose();
        instanceBuffer?.Dispose();
    }
}