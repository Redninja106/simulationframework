using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebGPU;
public partial class Device
{
    public static unsafe nint GetProcAddress(Device device, string procName)
    {
        var native_procName = MarshalUtils.AllocString(procName);
        nint result = Native.wgpuGetProcAddress(device.NativeHandle, native_procName);
        MarshalUtils.FreeString(native_procName);
        return result;
    }
}
