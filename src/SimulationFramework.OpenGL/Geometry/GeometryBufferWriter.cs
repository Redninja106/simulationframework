using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace SimulationFramework.OpenGL.Geometry;

class GeometryBufferWriter
{
    GeometryBuffer? currentBuffer;
    GeometryBufferPool pool;

    public GeometryBufferWriter(GeometryBufferPool pool)
    {
        this.pool = pool;
    }

    public GeometryBuffer Write(ReadOnlySpan<byte> bytes, int alignment, out int offset, out int count)
    {
        if (bytes.Length > GeometryBufferPool.BufferSize)
        {
            var buffer = pool.GetLargeBuffer(bytes.Length);
            buffer.Write(bytes, alignment, out offset, out count);
            return buffer;
        }

        if (currentBuffer is null || !currentBuffer.TryWrite(bytes, alignment, out offset, out count))
        {
            currentBuffer = pool.Rent();

            if (currentBuffer.TryWrite(bytes, alignment, out offset, out count))
            {
                return currentBuffer;
            }
            else
            {
                throw new Exception("huh");
            }
        }

        return currentBuffer;
    }

    public void Reset()
    {
        currentBuffer = null;
    }
}
