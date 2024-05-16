using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebGPU;

public unsafe partial class Queue
{
    public void WriteBuffer<T>(Buffer buffer, ulong bufferOffset, T[] data) where T : unmanaged
    {
        fixed (T* native_data_ptr = data)
        {
            nint native_buffer = buffer?.NativeHandle ?? 0;
            Native.wgpuQueueWriteBuffer(this.NativeHandle, native_buffer, bufferOffset, (nint)native_data_ptr, (nuint)(data.Length * sizeof(T)));
        }
    }

    public void WriteBuffer(Buffer buffer, ulong bufferOffset, byte[] data)
    {
        fixed (byte* native_data_ptr = data)
        {
            nint native_buffer = buffer?.NativeHandle ?? 0;
            Native.wgpuQueueWriteBuffer(this.NativeHandle, native_buffer, bufferOffset, (nint)native_data_ptr, (nuint)data.Length);
        }
    }
}