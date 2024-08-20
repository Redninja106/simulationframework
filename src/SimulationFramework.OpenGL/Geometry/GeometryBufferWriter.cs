using System;
using System.Collections.Generic;
using System.Linq;

namespace SimulationFramework.OpenGL.Geometry;

class GeometryBufferWriter
{
    private int bufferSize = 1024 * 64; // 64kb

    GeometryBuffer? currentBuffer;
    private Queue<GeometryBuffer> usedBuffers = [];
    private Queue<GeometryBuffer> freeBuffers = [];

    private List<GeometryBuffer> usedLargeBuffers = [];
    private List<GeometryBuffer> freeLargeBuffers = [];

    public GeometryBuffer Write(ReadOnlySpan<byte> bytes, int alignment, out int offset, out int count)
    {
        if (currentBuffer is null || !currentBuffer.TryWrite(bytes, alignment, out offset, out count))
        {
            if (currentBuffer is not null)
            {
                usedBuffers.Enqueue(currentBuffer);
            }

            if (!freeBuffers.TryDequeue(out currentBuffer))
            {
                currentBuffer = new GeometryBuffer(bufferSize);
            }

            if (currentBuffer.TryWrite(bytes, alignment, out offset, out count))
            {
                return currentBuffer;
            }

            // 'bytes' is huge!
            GeometryBuffer? largeBuffer = null;
            int smallestSizeDifference = int.MaxValue;
            for (int i = 0; i < freeLargeBuffers.Count; i++)
            {
                GeometryBuffer gb = freeLargeBuffers[i];
                int sizeDifference = gb.size - bytes.Length;
                if (sizeDifference < smallestSizeDifference)
                {
                    smallestSizeDifference = sizeDifference;
                    largeBuffer = gb;
                }
            }

            if (largeBuffer is null)
            {
                largeBuffer = new GeometryBuffer(bytes.Length);
            }
            else
            {
                freeLargeBuffers.Remove(largeBuffer);
            }

            largeBuffer.Write(bytes, alignment, out offset, out count);
            usedLargeBuffers.Add(largeBuffer);
            return largeBuffer;
        }

        return currentBuffer;
    }

    private int RoundUpPow2(int value)
    {
        // https://graphics.stanford.edu/%7Eseander/bithacks.html#RoundUpPowerOf2
        value--;
        value |= value >> 1;
        value |= value >> 2;
        value |= value >> 4;
        value |= value >> 8;
        value |= value >> 16;
        value++;
        return value;
    }

    public void Reset()
    {
        currentBuffer?.Reset();
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
