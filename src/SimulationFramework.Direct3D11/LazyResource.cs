using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Direct3D11;

internal class LazyResource<T> : D3D11Object, IDisposable where T : class, IDisposable
{
    private Func<T> valueFactory;
    private T? value;

    /// <summary>
    /// This should not be called outside of the DeviceResources class.
    /// </summary>
    public LazyResource(DeviceResources resources, Func<T> valueFactory) : base(resources)
    {
        this.valueFactory = valueFactory;
    }

    public T GetValue()
    {
        return value ??= valueFactory();
    }

    public override void Dispose()
    {
        DisposeValue();
        base.Dispose();
    }

    public void DisposeValue()
    {
        value?.Dispose();
        value = null;
    }
}