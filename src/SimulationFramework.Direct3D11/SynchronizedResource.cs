using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Direct3D11;

// Supports resource synchronizations between the cpu and gpu
internal abstract class SynchronizedResource : D3D11Object, IBindableResource
{
    private ulong cpuVersion = 0, gpuVersion = 0;
    // set when the resource is not being used by the gpu
    private readonly EventWaitHandle gpuWaitHandle;

    protected abstract void UpdateGPU();
    protected abstract void UpdateCPU();

    public SynchronizedResource(DeviceResources deviceResources) : base(deviceResources)
    {
        gpuWaitHandle = new(true, EventResetMode.ManualReset);
    }

    public void Synchronize()
    {

    }

    public void WaitForAccess()
    {
        gpuWaitHandle.WaitOne();
    }

    public virtual void NotifyBound(GraphicsQueueBase queue, BindingUsage usage, bool mayWrite)
    {
        gpuWaitHandle.Reset();
        queue.Wait(gpuWaitHandle);
    }

    public void NotifyUnbound(GraphicsQueueBase queue)
    {
        throw new NotImplementedException();
    }
}
