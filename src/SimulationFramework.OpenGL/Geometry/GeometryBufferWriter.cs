using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SimulationFramework.OpenGL.Geometry;

class GeometryBufferWriter
{
    GeometryBuffer? currentBuffer;
    GeometryBufferPool pool;

    private byte[] bufferData;
    private int bufferPosition;

    public GeometryBufferWriter(GeometryBufferPool pool)
    {
        this.pool = pool;
        bufferData = new byte[GeometryBufferPool.BufferSize];
    }

    public GeometryBuffer Write(ReadOnlySpan<byte> bytes, int alignment, out int offsetInBytes, out int countInBytes)
    {
        if (bytes.Length > GeometryBufferPool.BufferSize)
        {
            var buffer = pool.GetLargeBuffer(bytes.Length);
            buffer.Upload(bytes);
            offsetInBytes = 0;
            countInBytes = bytes.Length;
            return buffer;
        }

        if (!TryWriteBuffer(bytes, alignment, out offsetInBytes, out countInBytes))
        {
            if (currentBuffer != null)
            {
                UploadCurrentBuffer();
            }

            currentBuffer = pool.Rent();
             
            if (TryWriteBuffer(bytes, alignment, out offsetInBytes, out countInBytes))
            {
                return currentBuffer;
            }
            else
            {
                throw new Exception("Could not write to buffer");
            }
        }

        return currentBuffer;
    }

    public void UploadCurrentBuffer()
    {
        currentBuffer?.Upload<byte>(bufferData);
        currentBuffer = null;
        bufferPosition = 0;
    }

    [MemberNotNullWhen(true, nameof(currentBuffer))]
    private bool TryWriteBuffer(ReadOnlySpan<byte> bytes, int alignment, out int offset, out int count)
    {
        if (currentBuffer is null)
        {
            offset = count = default;
            return false;
        }

        int alignedPosition = bufferPosition;
        if (alignedPosition % alignment != 0)
        {
            alignedPosition += alignment - alignedPosition % alignment;
        }

        if (alignedPosition + bytes.Length > currentBuffer.size)
        {
            offset = count = 0;
            return false;
        }

        bytes.CopyTo(bufferData.AsSpan(alignedPosition, bytes.Length));
        bufferPosition = alignedPosition + bytes.Length;

        offset = alignedPosition;
        count = bytes.Length;
        return true;
    }

    public void Reset()
    {
        currentBuffer = null;
        bufferPosition = 0;
    }
}
