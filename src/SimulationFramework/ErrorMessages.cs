using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;
internal static class Exceptions
{
    public static Exception CoreComponentNotFound() => new SimulationFrameworkException("A simulation is not initialized or a required component is missing.");
}
