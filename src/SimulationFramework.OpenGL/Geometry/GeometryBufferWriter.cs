using System;
using System.Collections.Generic;

namespace SimulationFramework.OpenGL.Geometry;

class GeometryBufferWriter
{
    private int bufferSize = 1024 * 64;

    GeometryBuffer? currentBuffer;
    private Queue<GeometryBuffer> usedBuffers = [];
    private Queue<GeometryBuffer> freeBuffers = [];

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

            currentBuffer.Write(bytes, alignment, out offset, out count);
        }

        return currentBuffer;
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
    }
}
