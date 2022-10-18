using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;
internal static class Exceptions
{
    public static Exception CoreComponentNotFound(Type? componentType = null) => new SimulationFrameworkException($"A simulation is not initialized or a required component is missing. (missing component of type {componentType})");
    public static Exception ParseFailed(string? paramName = null) => new ArgumentOutOfRangeException(paramName, "Input string was not in the correct format");
    public static Exception NoPlatformAvailable() => new SimulationFrameworkException("No supported application platforms were found.");
    public static Exception DuplicateComponent(Type type) => new SimulationFrameworkException($"A component of type {type.Name} already exists.");
    public static Exception Internal(Exception? innerException = null) => new SimulationFrameworkException("An internal error occurred.", innerException);
}
