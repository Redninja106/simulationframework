using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing;

public interface IBuffer<T> : IDisposable where T : unmanaged
{
    /// <summary>
    /// The number of <see cref="T"/> elements in this buffer.
    /// </summary>
    int Size { get; }

    void SetData(Span<T> data);

    unsafe void SetData(T data) => SetData(new Span<T>(&data, 1));
    void SetData(T[] data) => SetData(data.AsSpan());
    
}
