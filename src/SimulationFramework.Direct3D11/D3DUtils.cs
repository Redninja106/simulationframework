using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D;
using Vortice.Direct3D11;

namespace SimulationFramework.Drawing.Direct3D11;

internal static class D3DUtils
{
    public const int E_OUTOFMEMORY = unchecked((int)0x8007000E);

    // finds the number of vertices in a group of primitives
    public static int GetVertexCount(this PrimitiveKind primitiveKind, int primitives)
    {
        return primitiveKind switch
        {
            PrimitiveKind.Triangles => primitives * 3,
            _ => throw new NotImplementedException(),
        };
    }

    // converts PrimitiveKind (our type) -> PrimitiveTopology (d3d type)
    public static PrimitiveTopology AsPrimitiveTopology(this PrimitiveKind primitiveKind)
    {
        return primitiveKind switch
        {
            PrimitiveKind.Triangles => PrimitiveTopology.TriangleList,
            PrimitiveKind.TriangleStrip => PrimitiveTopology.TriangleStrip,
            PrimitiveKind.Lines => PrimitiveTopology.LineList,
            PrimitiveKind.LineStrip => PrimitiveTopology.LineStrip,
            _ => throw new NotImplementedException(),
        };
    }

    public static ID3D11DeviceContext GetQueueBase(IGraphicsQueue queue)
    {
        return (queue as GraphicsQueueBase)?.DeviceContext ?? throw new Exception("Graphics queue is not a d3d11 queue");
    }

    public static BindFlags GetBindFlagsFromUsage(BindingUsage usage) => usage switch
    {
        BindingUsage.ShaderResource => BindFlags.ShaderResource,
        BindingUsage.VertexBuffer => BindFlags.VertexBuffer,
        BindingUsage.IndexBuffer => BindFlags.IndexBuffer,
        BindingUsage.UnorderedAccess => BindFlags.UnorderedAccess,
        BindingUsage.DepthStencilTarget => BindFlags.DepthStencil,
        BindingUsage.RenderTarget => BindFlags.RenderTarget,
        _ => throw new Exception()
    };
}
