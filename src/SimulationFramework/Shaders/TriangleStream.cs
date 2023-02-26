using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders;

public struct TriangleStream<TVertex> : IPrimitiveStream<TVertex>
    where TVertex : unmanaged
{
    public void EmitVertex(TVertex vertex)
    {
        throw new NotImplementedException();
    }

    public void EmitPrimitive(TrianglePrimitive<TVertex> primitive)
    {
        EndPrimitive();
        EmitVertex(primitive.Vertex0);
        EmitVertex(primitive.Vertex1);
        EmitVertex(primitive.Vertex2);
    }

    public void EndPrimitive()
    {
        throw new NotImplementedException();
    }
}