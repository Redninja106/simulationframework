using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Direct3D11;
internal interface IResourceProvider<T> : IBindableResource
{
    void GetResource(out T resource);
    T GetResource()
    {
        GetResource(out var t);
        return t;
    }
}
