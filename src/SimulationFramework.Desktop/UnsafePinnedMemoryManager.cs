using System.Buffers;

namespace SimulationFramework.Desktop;

internal unsafe class UnsafePinnedMemoryManager<T> : MemoryManager<T> where T : unmanaged
{
    private readonly T* pointer;
    private readonly int length;

    public UnsafePinnedMemoryManager(T* pointer, int length)
    {
        this.pointer = pointer;
        this.length = length;
    }

    public override Span<T> GetSpan()
    {
        return new(pointer, length);
    }

    public override MemoryHandle Pin(int elementIndex = 0)
    {
        return new MemoryHandle(pointer);
    }

    public override void Unpin()
    {
    }

    protected override void Dispose(bool disposing)
    {
    }
}