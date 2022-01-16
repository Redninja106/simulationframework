using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimulationFramework.IMGUI;

namespace SimulationFramework;

public sealed class DebugConsole : DebugWindow
{
    static List<string> lines = new();

    internal DebugConsole() : base("Debug Console")
    {
    }

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
        lines.Add(message);
    }

    protected override (Key, Key) GetDefaultKeybind()
    {
        return (Key.F3, Key.Unknown);
    }

    protected override void OnLayout()
    {
        if (ImGui.BeginListBox("", new Vector2(-5,-5)))
        {
            foreach (var l in lines)
            {
                ImGui.Text(l);
            }

            ImGui.EndListBox();
        }
    }
}
