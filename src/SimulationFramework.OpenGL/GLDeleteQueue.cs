using Khronos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.OpenGL;

// sometimes objects, such as shader arrays (backed by buffers) are destroy via finalizer.
// since finalizers are called from a different thread (where the OpenGL context is not active),
// we need to stage the deletion of opengl objects
internal class GLDeleteQueue
{
    private List<uint> buffers = [];

    public void AddBuffer(uint buffer)
    {
        lock (buffers)
        {
            buffers.Add(buffer);
        }
    }


    public unsafe void DestroyQueuedObjects()
    {
        lock (buffers)
        {
            fixed (uint* buffersPtr = CollectionsMarshal.AsSpan(buffers))
            {
                glDeleteBuffers(buffers.Count, buffersPtr);
            }
            buffers.Clear();
        }
    }
}
