using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimulationFramework.Desktop;

namespace SimulationFramework.Desktop;

public static class SimulationExtensions
{
    public static void RunDesktop(this Simulation simulation)
    {
        using var platform = DesktopAppPlatform.CreateForCurrentPlatform();
        simulation.Run(platform);
    }
}
