using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Desktop;

public static class SimulationExtensions
{
    public static void RunWindowed(this Simulation simulation, string title, int width, int height, bool resizable = true)
    {
        simulation.Run(new WindowEnvironment(title, width, height, resizable));
    }
}
