using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.OpenGL;
internal class OpenGLBuffer<T> : IBuffer<T> where T : unmanaged
{
    private readonly Dictionary<BufferUsageARB, BufferHandle> bufferHandles = new();

    public int Length { get; }
    public ResourceOptions Options { get; }
    public Span<T> Data => data;

    public T this[uint index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public T this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    private T[] data;

    public OpenGLBuffer(int length, ResourceOptions options)
    {
        this.Length = length;
        this.Options = options;
        this.data = new T[length];
    }

    public void ApplyChanges()
    {
        foreach (var (usage, handle) in bufferHandles)
        {
            GL.BindBuffer(BufferTargetARB.ArrayBuffer, handle);
            GL.BufferData(BufferTargetARB.ArrayBuffer, this.data, usage);
            GL.BindBuffer(BufferTargetARB.ArrayBuffer, BufferHandle.Zero);
        }
    }

    public void Dispose()
    {
        foreach (var (_, handle) in bufferHandles)
        {
            GL.DeleteBuffer(handle);
        }
    }

    public void SetData(Span<T> data)
    {
        data.CopyTo(this.data);
    }

    private BufferHandle GetBufferForUsage(BufferUsageARB bufferUsage)
    {
        if (!bufferHandles.TryGetValue(bufferUsage, out var value))
        {
            bufferHandles[bufferUsage] = value = GL.GenBuffer();

            int oldBuffer = 0;
            GL.GetInteger(GetPName.ArrayBufferBinding, ref oldBuffer);
            
            GL.BindBuffer(BufferTargetARB.ArrayBuffer, value);
            GL.BufferData(BufferTargetARB.ArrayBuffer, this.data, bufferUsage);
            GL.BindBuffer(BufferTargetARB.ArrayBuffer, new(oldBuffer));
        }

        return value;
    }

    public T[] GetData()
    {
        throw new NotImplementedException();
    }

    public void Update(nint data, nint length)
    {
        throw new NotImplementedException();
    }

    public void Update(Span<T> data, int offset = 0)
    {
        throw new NotImplementedException();
    }

    public void Update(T[] data, int offset = 0)
    {
        throw new NotImplementedException();
    }

    public ReadOnlySpan<T> GetSpan()
    {
        throw new NotImplementedException();
    }

    public void Update(nint data, nint length, IGraphicsQueue? queue = null)
    {
        throw new NotImplementedException();
    }

    public void Update(ReadOnlySpan<T> data, int offset = 0, IGraphicsQueue? queue = null)
    {
        throw new NotImplementedException();
    }

    public void Update(T[] data, int offset = 0, IGraphicsQueue? queue = null)
    {
        throw new NotImplementedException();
    }
}
