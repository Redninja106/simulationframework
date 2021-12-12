using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

public static class Graphics
{
    internal static IGraphicsProvider Provider => Simulation.Current.GraphicsProvider; 
}