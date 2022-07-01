using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Pipeline;

public interface IShader : IDisposable
{
    ShaderKind Kind { get; }

    void Apply(IRenderer renderer);

    void SetBuffer<T>(string name, IBuffer<T> buffer) where T : unmanaged;
    void SetTexture(string name, ITexture texture, TileMode tileMode);
    void SetVariable<T>(string name, T value) where T : unmanaged;

    T GetVariable<T>(string name) where T : unmanaged;
    ITexture GetTexture(string name);
    IBuffer<T> GetBuffer<T>(string name) where T : unmanaged;
}
