using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.OpenGL.Geometry;
internal class GeometryBufferPool : IDisposable
{
    public const int BufferSize = 1024 * 64; // 64kb

    private HashSet<GeometryBuffer> usedBuffers = [];
    private HashSet<GeometryBuffer> freeBuffers = [];

    private HashSet<GeometryBuffer> usedLargeBuffers = [];
    private HashSet<GeometryBuffer> freeLargeBuffers = [];

    public GeometryBufferPool()
    {
    }

    public GeometryBuffer GetLargeBuffer(int size)
    {
        // 'bytes' is huge!
        GeometryBuffer? largeBuffer = null;
        int smallestSizeDifference = int.MaxValue;

        foreach (var gb in freeLargeBuffers)
        {
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
        if (freeBuffers.FirstOrDefault() is not GeometryBuffer buffer)
        {
            buffer = new GeometryBuffer(BufferSize);
        }

        freeBuffers.Remove(buffer);
        usedBuffers.Add(buffer);
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
        foreach (var buffer in usedBuffers)
        {
            freeBuffers.Add(buffer);
        }
        usedBuffers.Clear();

        foreach (var largeBuffer in usedLargeBuffers)
        {
            freeLargeBuffers.Add(largeBuffer);
        }
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

    public void Return(GeometryBuffer? buffer)
    {
        if (buffer is null)
        {
            return;
        }

        if (usedBuffers.Remove(buffer))
        {
            freeBuffers.Add(buffer);
            return;
        }

        if (usedLargeBuffers.Remove(buffer))
        {
            freeLargeBuffers.Add(buffer);
            return;
        }
    }
}
