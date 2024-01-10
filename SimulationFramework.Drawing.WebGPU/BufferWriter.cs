using System.Runtime.CompilerServices;
using WebGPU;

namespace SimulationFramework.Drawing.WebGPU;

class BufferWriter<T> where T : unmanaged
{
    private readonly GraphicsResources resources;
    private readonly Buffer buffer;
    private readonly T[] bufferData;

    public int Count { get; private set; }
    public int Capacity => bufferData.Length;

    public BufferWriter(GraphicsResources resources, BufferUsage usage, int capacity = 512)
    {
        this.resources = resources;
        
        bufferData = new T[capacity];
        
        buffer = resources.Device.CreateBuffer(new()
        {
            MappedAtCreation = false,
            Size = (ulong)(Unsafe.SizeOf<T>() * bufferData.Length),
            Usage = BufferUsage.CopyDst | usage,
        });
    }

    public bool HasCapacity(int count)
    {
        return this.Count + count < Capacity;
    }

    public void Write(ReadOnlySpan<T> elements)
    {
        var destSpan = bufferData.AsSpan(Count, elements.Length);
        elements.CopyTo(destSpan);
        Count += elements.Length;
    }

    public Buffer GetBuffer()
    {
        return buffer;
    }

    public Span<T> GetWritableSpan(int length)
    {
        var result = bufferData.AsSpan(Count, length);
        Count += length;
        return result;
    }

    public void Upload()
    {
        resources.Queue.WriteBuffer(buffer, 0, bufferData);
    }

    public void Reset()
    {
        Count = 0;
    }
}