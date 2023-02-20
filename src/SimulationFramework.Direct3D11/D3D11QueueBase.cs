using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D11;

namespace SimulationFramework.Drawing.Direct3D11;
internal abstract class D3D11QueueBase : D3D11Object, IGraphicsQueue
{
    public ID3D11DeviceContext DeviceContext;

    public D3D11QueueBase(DeviceResources resources, ID3D11DeviceContext context) : base(resources)
    {
        this.DeviceContext = context;
    }

    public void Flush()
    {
        DeviceContext.Flush();
    }
}
