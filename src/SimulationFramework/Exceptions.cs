namespace SimulationFramework;
internal static class Exceptions
{
    public static Exception ComponentNotFound(Type? componentType = null) => new($"Missing component of type {componentType}.");
    public static Exception ParseFailed(string? paramName = null) => new ArgumentOutOfRangeException(paramName, "Input string was not in the correct format");
    public static Exception NoPlatform() => new("No Simulation Platforms found. Make sure you have a platform package referenced. If using NativeAOT, make sure you call Register() on the platform you want to use (ex. DesktopPlatform.Register().");
    public static Exception DuplicateMessageListener() => new("Duplicate Message Listener");
    public static Exception SimulationRunning() => new InvalidOperationException("A simulation is already running.");
    public static Exception HostAlreadyInitialized() => new("Host is already initialized!");
    public static Exception NoActiveHost() => new("No Simulation Host is Currently Active!");
}