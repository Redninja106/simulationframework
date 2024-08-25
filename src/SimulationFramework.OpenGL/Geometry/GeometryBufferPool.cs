using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.OpenGL.Geometry;
internal class GeometryBufferPool
{
    public const int BufferSize = 1024 * 64; // 64kb

    private Queue<GeometryBuffer> usedBuffers = [];
    private Queue<GeometryBuffer> freeBuffers = [];

    private List<GeometryBuffer> usedLargeBuffers = [];
    private List<GeometryBuffer> freeLargeBuffers = [];

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

    public void Reset()
    {
        while (usedBuffers.Count > 0)
        {
            var buffer = usedBuffers.Dequeue();
            buffer.Reset();
            freeBuffers.Enqueue(buffer);
        }

        foreach (var buffer in usedLargeBuffers)
        {
            buffer.Reset();
        }

        freeLargeBuffers.AddRange(usedLargeBuffers);
        usedLargeBuffers.Clear();
    }
}
