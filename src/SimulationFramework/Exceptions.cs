using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;
internal static class Exceptions
{
    public static Exception ComponentNotFound(Type? componentType = null) => new($"Missing component of type {componentType}.");
    public static Exception ParseFailed(string? paramName = null) => new ArgumentOutOfRangeException(paramName, "Input string was not in the correct format");
    public static Exception NoPlatform() => new("No platform was provided and none could be found.");
    public static Exception DuplicateMessageListener() => new("Duplicate Message Listener");
}
