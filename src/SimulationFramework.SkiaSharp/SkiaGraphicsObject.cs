namespace SimulationFramework.SkiaSharp;
public abstract class SkiaGraphicsObject : IDisposable
{
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
        Dispose();
    }
}
