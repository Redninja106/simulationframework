using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.RenderPipeline;

public interface IBuffer<T> : IDisposable where T : unmanaged
{
    /// <summary>
    /// The number of <typeparamref name="T"/> elements in this buffer.
    /// </summary>
    int Size { get; }

    // ResourceOptions Options { get; }

    void SetData(Span<T> data);
    unsafe void SetData(T data) => SetData(new Span<T>(&data, 1));
    void SetData(T[] data) => SetData(data.AsSpan());

    // ref T this[int index] { get; }
}