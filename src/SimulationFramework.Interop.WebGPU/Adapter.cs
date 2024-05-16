using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebGPU;
public partial class Adapter
{
    public Device RequestDevice(DeviceDescriptor? descriptor)
    {
        Device? device = null;
        RequestDeviceStatus? status = null;
        string? message = null; ;

        using EventWaitHandle waitHandle = new(false, EventResetMode.ManualReset);

        this.RequestDevice(descriptor, (s, d, m) =>
        {
            status = s;
            device = d;
            message = m;
            waitHandle.Set();
        });

        waitHandle.WaitOne();
        
        if (status != RequestDeviceStatus.Success)
        {
            throw new Exception(message);
        }

        return device!;
    }
}
