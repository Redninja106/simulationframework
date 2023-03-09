using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D11;

namespace SimulationFramework.Drawing.Direct3D11;
internal class D3D11DeferredQueue : GraphicsQueueBase, IGraphicsQueue
{
    public D3D11DeferredQueue(DeviceResources resources) : base(resources, null)
    {
        throw new NotImplementedException();
    }
}
