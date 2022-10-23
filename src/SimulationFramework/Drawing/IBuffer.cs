using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing;

public interface IBuffer<T> : IResource where T : unmanaged
{
    /// <summary>
    /// The number of <typeparamref name="T"/> elements in this buffer.
    /// </summary>
    int Length { get; }

    ResourceOptions Options { get; }

    /// <summary>
    /// Sets the content of the buffer.
    /// </summary>
    /// <param name="data">A span of <typeparamref name="T"/> elements, containing the new data of the buffer. Must have the same length as the buffer.</param>
    void SetData(Span<T> data);
    sealed unsafe void SetData(IntPtr data, int length) => SetData(new Span<T>(data.ToPointer(), length));
    sealed unsafe void SetData(T data) => SetData(new Span<T>(&data, 1));
    sealed unsafe void SetData(T[] data) => SetData(data.AsSpan());

    sealed ref T this[int index] => ref Data[index];

    Span<T> Data { get; }

    void ApplyChanges();
}