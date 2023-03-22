using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D11;

namespace SimulationFramework.Drawing.Direct3D11;
internal abstract class GraphicsQueueBase : D3D11Object, IGraphicsQueue
{
    public ID3D11DeviceContext4 DeviceContext;
    public ID3D11Fence Fence;
    private ulong fenceValue;

    public GraphicsQueueBase(DeviceResources resources, ID3D11DeviceContext4 context) : base(resources)
    {
        this.DeviceContext = context;
        
        Fence = resources.Device.CreateFence<ID3D11Fence>(0, 0);
    }

    public void Flush()
    {
        DeviceContext.Flush();
    }

    public void Wait(WaitHandle handle)
    {
        // DeviceContext.Signal(Fence, fenceValue);
        // Fence.SetEventOnCompletion(fenceValue, handle);
        // DeviceContext.Signal();
        // fenceValue++;
    }
}
