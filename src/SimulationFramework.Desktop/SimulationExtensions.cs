using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimulationFramework.Desktop;
using SimulationFramework.Drawing;

namespace SimulationFramework.Desktop;

public static class SimulationExtensions
{
    public static void RunDesktop(this Simulation simulation)
    {
        RunDesktop(simulation, null);
    }

    public static void RunDesktop(this Simulation simulation, Func<IntPtr, IGraphicsProvider> graphics)
    {
        using var platform = new DesktopPlatform(graphics);
        simulation.Run(platform);
    }
}
