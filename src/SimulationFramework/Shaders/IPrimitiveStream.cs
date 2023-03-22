using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders;

public interface IPrimitiveStream<TVertex>
    where TVertex : unmanaged
{
    void EmitVertex(TVertex vertex);
    void EndPrimitive();
}