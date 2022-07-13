using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Direct3D11;

internal abstract class D3D11Object : IDisposable
{
    public DeviceResources Resources { get; private set; }

    public D3D11Object(DeviceResources deviceResources)
    {
        this.Resources = deviceResources;
    }
    
    public virtual void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    ~D3D11Object()
    {
        Dispose();
    }
}
