﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Direct3D11;
internal enum BindingUsage
{
    ShaderResource,
    UnorderedAccess,
    VertexBuffer,
    IndexBuffer,
    RenderTarget,
    DepthStencilTarget,
}