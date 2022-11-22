using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;
internal static class Exceptions
{
    public static Exception CoreComponentNotFound() => new SimulationFrameworkException("A simulation is not initialized or a required component is missing.");
    public static Exception ParseFailed(string? paramName = null) => new ArgumentOutOfRangeException(paramName, "Input string was not in the correct format");
    public static Exception InvalidCullMode() => new ArgumentOutOfRangeException("Invalid CullMode value!");
}
