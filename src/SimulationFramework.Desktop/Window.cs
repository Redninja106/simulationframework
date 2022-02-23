using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Desktop;

internal static class Window
{
    internal static bool graphicsEnabled = true;

    public static void SetGraphicsEnabled(bool enabled)
    {
        graphicsEnabled = enabled;
    }
}
