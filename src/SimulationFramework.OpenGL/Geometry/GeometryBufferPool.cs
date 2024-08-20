using System.Collections.Generic;

namespace SimulationFramework.OpenGL.Geometry;

class GeometryBufferPool
{
    private readonly Queue<GeometryBuffer> freeBuffers = [];
    private readonly List<GeometryBuffer> usedBuffers = [];

    public GeometryBufferPool()
    {
    }

    public GeometryBuffer Rent(int remainingBytes, int alignment)
    {
        GeometryBuffer buffer;
        if (freeBuffers.Count > 0)
        {
            buffer = freeBuffers.Dequeue();
        }
        else
        {
            buffer = new GeometryBuffer(64 * 1024);
        }

        usedBuffers.Add(buffer);
        return buffer;
    }

    public void Return(GeometryBuffer buffer)
    {
        usedBuffers.Remove(buffer);
        freeBuffers.Enqueue(buffer);
    }

    public void Reset()
    {
        while (usedBuffers.Count > 0)
        {
            var buffer = usedBuffers[0];
            freeBuffers.Enqueue(buffer);
            usedBuffers.RemoveAt(0);
        }
    }
}
