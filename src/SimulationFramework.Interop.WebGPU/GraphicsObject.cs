using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebGPU;
public class GraphicsObject : IDisposable
{
    public GraphicsObject(nint nativeHandle)
    {
        if (nativeHandle is 0)
            throw new ArgumentNullException(nameof(nativeHandle));

        this.NativeHandle = nativeHandle;
    }

    public nint NativeHandle { get; private set; }
    public bool IsDisposed => NativeHandle == 0;

    public virtual void Dispose()
    {
        NativeHandle = 0;
        GC.SuppressFinalize(this);
    }

    ~GraphicsObject()
    {
        Dispose();
    }
}
