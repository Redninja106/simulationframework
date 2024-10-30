using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.OpenGL.Geometry;
internal class GeometryBufferPool : IDisposable
{
    public const int BufferSize = 1024 * 64; // 64kb

    private Queue<GeometryBuffer> usedBuffers = [];
    private Queue<GeometryBuffer> freeBuffers = [];

    private List<GeometryBuffer> usedLargeBuffers = [];
    private List<GeometryBuffer> freeLargeBuffers = [];

    public GeometryBufferPool()
    {
    }

    public GeometryBuffer GetLargeBuffer(int size)
    {
        // 'bytes' is huge!
        GeometryBuffer? largeBuffer = null;
        int smallestSizeDifference = int.MaxValue;
        for (int i = 0; i < freeLargeBuffers.Count; i++)
        {
            GeometryBuffer gb = freeLargeBuffers[i];
            int sizeDifference = gb.size - size;
            if (sizeDifference < smallestSizeDifference)
            {
                smallestSizeDifference = sizeDifference;
                largeBuffer = gb;
            }
        }

        if (largeBuffer is null)
        {
            largeBuffer = new GeometryBuffer(size);
        }
        else
        {
            freeLargeBuffers.Remove(largeBuffer);
        }

        usedLargeBuffers.Add(largeBuffer);
        return largeBuffer;
    }

    public GeometryBuffer Rent()
    {
        if (!freeBuffers.TryDequeue(out GeometryBuffer? buffer))
        {
            buffer = new GeometryBuffer(BufferSize);
        }

        usedBuffers.Enqueue(buffer);
        return buffer;
    }

    public GeometryBuffer Rent(int minSize)
    {
        if (minSize > BufferSize)
        {
            return GetLargeBuffer(minSize);
        }
        else
        {
            return Rent();
        }
    }

    public void Reset()
    {
        while (usedBuffers.Count > 0)
        {
            var buffer = usedBuffers.Dequeue();
            freeBuffers.Enqueue(buffer);
        }

        freeLargeBuffers.AddRange(usedLargeBuffers);
        usedLargeBuffers.Clear();
    }

    public void Dispose()
    {
        foreach (var buffer in usedBuffers)
        {
            buffer.Dispose();
        }
        foreach (var buffer in freeBuffers)
        {
            buffer.Dispose();
        }
        foreach (var buffer in freeLargeBuffers)
        {
            buffer.Dispose();
        }
        foreach (var buffer in usedLargeBuffers)
        {
            buffer.Dispose();
        }
    }
}
