using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Direct3D11;
internal class ResourceCache<TKey, TResource> : IDisposable
    where TResource : IDisposable
{
    private readonly Dictionary<TKey, TResource> resources = new();



    public void Dispose()
    {
        foreach (var (_, resource) in resources)
        {
            resource.Dispose();
        }
    }
}
