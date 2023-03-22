using SimulationFramework.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing;

[ShaderIntrinsic]
public interface IBuffer<T> : IResource where T : unmanaged
{
    /// <summary>
    /// The number of <typeparamref name="T"/> elements in this buffer.
    /// </summary>
    int Length { get; }

    ResourceOptions Options { get; }

    [ShaderIntrinsic]
    T this[int index] { get; set; }

    [ShaderIntrinsic]
    T this[uint index] { get; set; }

    ReadOnlySpan<T> GetData();

    void Update(nint data, nint length, IGraphicsQueue? queue = null);
    void Update(ReadOnlySpan<T> data, int offset = 0, IGraphicsQueue? queue = null);
    void Update(T[] data, int offset = 0, IGraphicsQueue? queue = null);

}