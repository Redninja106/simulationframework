using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.SkiaSharp;
public abstract class SkiaGraphicsObject : IDisposable
{
    private bool disposedValue;

    public bool IsDisposed { get; private set; }

    public SkiaGraphicsObject()
    {

    }

    public virtual void Dispose()
    {
        GC.SuppressFinalize(this);
        IsDisposed = true;
    }

    ~SkiaGraphicsObject()
    {
        Console.WriteLine($"[{DateTime.Now}] [{GetType()}] FINALIZED");
        Dispose();
    }
}
