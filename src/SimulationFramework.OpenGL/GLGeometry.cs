using SimulationFramework.Drawing;
using SimulationFramework.Drawing.Shaders.Compiler;
using SimulationFramework.OpenGL.Geometry;
using System;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.OpenGL;
unsafe internal class GLGeometry : IGeometry
{
    private GLGraphics graphics;
    public GeometryChunk chunk;

    public GLGeometry(GLGraphics graphics, GeometryChunk chunk)
    {
        this.graphics = graphics;
        this.chunk = chunk;
    }

    //public unsafe void SetIndices(ReadOnlySpan<uint> indices)
    //{
    //    uint buffer = 0;
    //    glCreateBuffers(1, &buffer);
    //    indexBuffer = buffer;
    //    fixed (uint* indicesPtr = indices) 
    //    {
    //        glNamedBufferStorage(buffer, indices.Length * sizeof(uint), indicesPtr, 0);
    //    }
    //    count = indices.Length;
    //}

    //public void AddVertexBuffer<T>(ReadOnlySpan<T> vertices)
    //    where T : unmanaged
    //{
    //    fixed (T* firstVertex = vertices)
    //    {
    //        uint buffer = 0;
    //        glCreateBuffers(1, &buffer);
    //        glNamedBufferStorage(buffer, vertices.Length * sizeof(T), firstVertex, 0);

    //        vertexBuffers.Add(typeof(T), new(buffer, vertices.Length, sizeof(T), typeof(T)));

    //        if (indexBuffer == 0)
    //        {
    //            count = vertices.Length;
    //        }
    //    }
    //}

    public void Dispose()
    {
    //    if (indexBuffer != 0)
    //    {
    //        uint idxBuf = indexBuffer;  
    //        glDeleteBuffers(1, &idxBuf);
    //    }

    //    foreach (var (_, vb) in vertexBuffers)
    //    {
    //        glDeleteBuffers(1, &vb.buffer);
    //    }
    }

    public uint[]? GetIndices()
    {
        throw new NotImplementedException();
    }
    public TVertex[] GetVertices<TVertex>() where TVertex : unmanaged
    {
        TVertex[] array = new TVertex[chunk.vertexBuffer.size / Unsafe.SizeOf<TVertex>()];
        fixed (TVertex* vertexPtr = array)
        {
            glGetNamedBufferSubData(chunk.vertexBuffer.buffer, 0, chunk.vertexBuffer.size, vertexPtr);
        }
        return array;
    }

    //public void Draw(ref readonly CanvasState state)
    //{
    //    var vb = vertexBuffers.Single().Value;
    //    var stream = graphics.streams.GetCustomVertexGeometryStream(vb.type, in state);
    //    glBindBuffer(GL_ARRAY_BUFFER, vb.buffer);
    //    if (indexBuffer != 0)
    //    {
    //        glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, indexBuffer);
    //    }

    //    stream.BindVertexArray();
    //    if (indexBuffer != 0)
    //    {
    //        glDrawElements(GL_TRIANGLES, count, GL_UNSIGNED_INT, null);
    //    }
    //    else
    //    {
    //        glDrawArrays(GL_TRIANGLES, 0, count);
    //    }
    //}

    internal void DrawInstances<TInstances>(GLCanvas canvas, ReadOnlySpan<Matrix3x2> instances)
    {
        throw new NotImplementedException();
    }

    internal void DrawInstances<TInstance>(GLCanvas canvas, ReadOnlySpan<TInstance> instances) where TInstance : unmanaged
    {
        throw new NotImplementedException();
    }

    internal static GLGeometry Create<TVertex>(GLGraphics graphics, ReadOnlySpan<TVertex> vertices) where TVertex : unmanaged
    {
        GeometryBuffer vertexBuffer = new(Unsafe.SizeOf<TVertex>() * vertices.Length);
        vertexBuffer.Upload(vertices);

        GeometryChunk chunk = new()
        {
            count = vertices.Length,
            vertexBuffer = vertexBuffer,
            triangles = true,
            vertexLayout = VertexLayout.Get(typeof(TVertex))
        };

        return new(graphics, chunk);
    }

    internal static GLGeometry CreateIndexed<TVertex>(GLGraphics graphics, ReadOnlySpan<TVertex> vertices, ReadOnlySpan<uint> indices) where TVertex : unmanaged
    {
        GeometryBuffer vertexBuffer = new(Unsafe.SizeOf<TVertex>() * vertices.Length);
        vertexBuffer.Upload(vertices);

        GeometryBuffer indexBuffer = new(sizeof(uint) * indices.Length);
        indexBuffer.Upload(indices);

        GeometryChunk chunk = new()
        {
            count = indices.Length,
            vertexBuffer = vertexBuffer,
            triangles = true,
            vertexLayout = VertexLayout.Get(typeof(TVertex)),
            indexBuffer = indexBuffer,
        };

        return new(graphics, chunk);
    }

    public void SetVertices<TVertex>(TVertex[] vertices) where TVertex : unmanaged
    {

        chunk.vertexBuffer.Upload<TVertex>(vertices);
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
