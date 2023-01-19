using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D11;

namespace SimulationFramework.Drawing.Direct3D11;
internal class D3D11ImmediateQueue : D3D11QueueBase
{
    public D3D11ImmediateQueue(DeviceResources resources) : base(resources, resources.Device.ImmediateContext)
    {
    }
}
