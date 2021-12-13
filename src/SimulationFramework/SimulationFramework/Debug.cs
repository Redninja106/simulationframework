using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

internal static class Debug
{
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
}
