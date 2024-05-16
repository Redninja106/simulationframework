using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WebGPU;
internal unsafe partial class Native
{
    const string LIBRARY_NAME = "wgpu_native.dll";

    [DllImport(LIBRARY_NAME)]
    public static extern nint wgpuGetProcAddress(nint device, byte* procName);
}
