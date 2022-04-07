using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D;

namespace SimulationFramework.Drawing.Direct3D11;

internal static class D3DUtils
{
    // finds the number of vertices in a group of primitives
    public static int GetVertexCount(this PrimitiveKind primitiveKind, int count)
    {
        return primitiveKind switch
        {
            PrimitiveKind.Triangles => count * 3,
            _ => throw new NotImplementedException(),
        };
    }

    // converts PrimitiveKind (our type) -> PrimitiveTopology (d3d type)
    public static PrimitiveTopology AsPrimitiveTopology(this PrimitiveKind primitiveKind)
    {
        return primitiveKind switch
        {
            PrimitiveKind.Triangles => PrimitiveTopology.TriangleList,
            _ => throw new NotImplementedException(),
        };
    }
}
