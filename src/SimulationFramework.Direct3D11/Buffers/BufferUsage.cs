using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Direct3D11.Buffers;

internal enum BufferUsage
{
    Default,
    VertexBuffer,
    IndexBuffer,
    ConstantBuffer,
    UnorderedAccessResource,
    ShaderResource,
}