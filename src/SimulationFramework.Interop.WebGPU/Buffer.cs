using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WebGPU;
public unsafe partial class Buffer
{
    public Span<T> GetMappedRange<T>() where T : unmanaged
    {
        return GetMappedRange<T>(0, (int)Size);
    }

    public Span<T> GetMappedRange<T>(int offset, int count) where T : unmanaged
    {
        return MemoryMarshal.Cast<byte, T>(GetMappedRange(offset * sizeof(T), count * sizeof(T)));
    }

    public Span<byte> GetMappedRange()
    {
        return GetMappedRange<byte>();
    }

    public Span<byte> GetMappedRange(int offset, int size)
    {
        return new Span<byte>((void*)Native.wgpuBufferGetMappedRange(this.NativeHandle, (nuint)offset, (nuint)size), size);
    }
}
