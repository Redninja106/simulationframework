using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

public static class Debug
{
    public static float Framerate => Time.Provider.GetFramerate();

    public static bool SilenceWarnings { get; set; }

    [Conditional("DEBUG")]
    public static void Warn(string message)
    {
        if (SilenceWarnings)
            return;

        var oldCol = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Warning: " + message);
        Console.ForegroundColor = oldCol;
    }

    [Conditional("DEBUG")]
    public static void Log(string message)
    {
        Console.WriteLine(message);
    }
}
