using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders;

struct PointStream<T> : IPrimitiveStream<T>
    where T : unmanaged
{
    public void EmitVertex(T vertex)
    {
        throw new NotImplementedException();
    }

    public void EndPrimitive()
    {
        throw new NotImplementedException();
    }
}