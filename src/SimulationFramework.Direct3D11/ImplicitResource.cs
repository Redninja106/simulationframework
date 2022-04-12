using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Direct3D11;

internal class ImplicitResource<T> : D3D11Object, IDisposable where T : class, IDisposable
{
    private Func<T> valueFactory;
    private T value;
    
    /// <summary>
    /// This should not be called outside of the DeviceResources class.
    /// </summary>
    public ImplicitResource(DeviceResources resources, Func<T> valueFactory) : base(resources)
    {
        this.valueFactory = valueFactory;
    }

    public T Value
    {
        get
        {
            if (value is null)
                value = valueFactory();

            return value;
        }
    }

    public void Dispose()
    {
        value.Dispose();
    }
}