using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: System.Reflection.Metadata.MetadataUpdateHandler(typeof(HotReloadHandler))]

namespace SimulationFramework;

internal static class HotReloadHandler
{
    public static void UpdateApplication(Type[]? types)
    {
        if (types is null)
            return;

        var provider = Application.Current.GetComponent<IGraphicsProvider>();

        foreach (var type in types)
        {
            if (type.IsValueType && type.GetInterfaces().Contains(typeof(IShader)))
            {
                provider.InvalidateShader(type);
            }
        }
    }
}
