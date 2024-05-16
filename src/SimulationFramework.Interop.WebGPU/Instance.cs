using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebGPU;
public unsafe partial class Instance
{
    public static Instance Create()
    {
        InstanceDescriptor.Native descriptor = default;
        return new(Native.wgpuCreateInstance(&descriptor));
    }

    public Adapter RequestAdapter(RequestAdapterOptions? options)
    {
        Adapter? adapter = null;
        RequestAdapterStatus? status = null;
        string? message = null; ;

        using EventWaitHandle waitHandle = new(false, EventResetMode.ManualReset);

        this.RequestAdapter(options, (s, a, m) =>
        {
            status = s;
            adapter = a;
            message = m;
            waitHandle.Set();
        });

        waitHandle.WaitOne();

        if (status != RequestAdapterStatus.Success)
        {
            throw new Exception(message);
        }

        return adapter!;
    }
}
