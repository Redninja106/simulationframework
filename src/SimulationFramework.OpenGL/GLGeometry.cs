using SimulationFramework.Drawing;
using SimulationFramework.Drawing.Shaders.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//namespace SimulationFramework.OpenGL;
//unsafe internal class GLGeometry : IGeometry
//{
//    private Dictionary<Type, VertexBuffer> vertexBuffers = [];
//    private int vertexCount;
//    private uint indexBuffer;

//    public GLGeometry()
//    {

//    }

//    public void AddVertexBuffer<T>(ReadOnlySpan<T> vertices)
//        where T : unmanaged
//    {
//        fixed (T* firstVertex = vertices) 
//        {
//            uint buffer = 0;
//            glCreateBuffers(1, &buffer);
//            glNamedBufferStorage(buffer, vertices.Length * sizeof(T), firstVertex, 0);

//            vertexBuffers.Add(typeof(T), new(buffer, sizeof(T)));

//            vertexCount = vertexBuffers.Count;
//        }
//    }

//    public void Dispose()
//    {
//        throw new NotImplementedException();
//    }

//    public uint[]? GetIndices()
//    {
//        throw new NotImplementedException();
//    }

//    public TVertex[] GetVertices<TVertex>() where TVertex : unmanaged
//    {
//        throw new NotImplementedException();
//    }

//    public void Draw(GLCanvas canvas)
//    {

//        foreach (var (type, buffer) in vertexBuffers)
//        {
//            glBindBuffer(GL_ARRAY_BUFFER, buffer.buffer);
//            var stream = canvas.GetCustomVertexGeometryStream(type);
//            stream.BindVertexArray();
//            glDrawArrays(GL_TRIANGLES, 0, vertexCount / 3);
//        }


//        // CustomVertexGeometryStream.SetVertexLayout(, 0, ref location, ref offset);
//    }

//    struct VertexBuffer
//    {
//        public uint buffer;
//        public int stride;

//        public VertexBuffer(uint buffer, int stride)
//        {
//            this.buffer = buffer;
//            this.stride = stride;
//        }
//    }
//}
