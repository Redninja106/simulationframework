using SimulationFramework.OpenGL.Geometry.Streams;

namespace SimulationFramework.OpenGL.Geometry;

struct GeometryChunk
{
    // buffers
    public GeometryBuffer vertexBuffer;
    public GeometryBuffer? indexBuffer;
    public GeometryBuffer? instanceBuffer;

    // vertex information
    public VertexLayout vertexLayout;
    public bool largeIndices;
    public bool triangles;

    // draw info
    public int offset;
    public int count;
    public int baseVertex;
    public int instanceCount;

    public unsafe void Draw()
    {
        vertexBuffer.Bind();
        vertexLayout.Bind();

        if (instanceBuffer != null)
        {
            instanceBuffer.Bind();
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

            uint indexType = largeIndices ? GL_UNSIGNED_INT : GL_UNSIGNED_SHORT;
            int indexOffset = offset * (largeIndices ? sizeof(uint) : sizeof(ushort));

            if (instanceBuffer == null)
            {
                glDrawElementsBaseVertex(mode, count, indexType, (void*)indexOffset, baseVertex);
            }
            else
            {
                glDrawElementsInstancedBaseVertex(mode, count, indexType, (void*)indexOffset, instanceCount, baseVertex);
            }
        }
    }

    public static GeometryChunk Create(GeometryBuffer vertexBuffer, GeometryStream geometryStream, int offset, int count, bool triangles)
    {
        return new()
        {
            vertexBuffer = vertexBuffer,
            offset = offset,
            count = count,
            triangles = triangles,
            vertexLayout = geometryStream.VertexLayout,
        };
    }
}