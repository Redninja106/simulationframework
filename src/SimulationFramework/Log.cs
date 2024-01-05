using System.IO;

namespace SimulationFramework;

/// <summary>
/// Provides internal logging functionality.
/// </summary>
public static class Log
{
    /// <summary>
    /// The textwriter to write messages to. Defaults to Console.Out. Set to <see langword="null"/> to ignore messages.
    /// </summary>
    public static TextWriter? Out { get; set; } = Console.Out;

    /// <summary>
    /// Logs a warning.
    /// </summary>
    public static void Warn(string warning)
    {
        Out?.WriteLine(warning);
    }

    /// <summary>
    /// Logs an error.
    /// </summary>
    public static void Error(string error)
    {
        Out?.WriteLine(error);
    }

    /// <summary>
    /// Logs a message.
    /// </summary>
    public static void Message(string message)
    {
        Out?.WriteLine(message);
    }
}
