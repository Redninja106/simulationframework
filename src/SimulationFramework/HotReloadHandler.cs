using SimulationFramework;
using SimulationFramework.Drawing;
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
        // Graphics.
    }
}
