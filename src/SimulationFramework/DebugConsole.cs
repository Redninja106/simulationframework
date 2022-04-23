using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using SimulationFramework.IMGUI;

namespace SimulationFramework;

public sealed class DebugConsole : DebugWindow
{
    static List<string> lines = new();
    static string input;
    static bool justSentInput;
    static Dictionary<string, object> commands = new();
    static bool helpWindowOpen;
    static bool windowJustOpened;

    internal DebugConsole() : base("Debug Console", WindowFlags.NoScrollbar | WindowFlags.NoScrollWithMouse)
    {
        RegisterCommand("help", () => helpWindowOpen = !helpWindowOpen);
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


        if (ImGui.BeginListBox("", new Vector2(-5, -25)))
        {
            foreach (var l in lines)
            {
                ImGui.Text(l);
            }

            ImGui.EndListBox();
        }

        if (justSentInput)
        {
            ImGuiNET.ImGui.SetKeyboardFocusHere();
            justSentInput = false;
        }

        if (windowJustOpened)
        {
            ImGuiNET.ImGui.SetKeyboardFocusHere();
            windowJustOpened = false;
        }

        if (ImGui.InputText("", ref input, 256, InputTextFlags.EnterReturnsTrue))
        {
            lines.Add(">"+input);
            var name = input.Contains(' ') ? input.Substring(0, input.IndexOf(' ')) : input;
            if (commands.ContainsKey(name.Trim('\0').ToUpper()))
            {
                var command = commands[name.Trim('\0').ToUpper()];

                if (command is Action noParamCmd)
                {
                    noParamCmd();
                }
                if (command is Action<string[]> paramCmd)
                {
                    paramCmd(input.Substring(input.IndexOf(' ')).Split(' '));
                }
            }

            input = "";
            justSentInput = true;
        }

        if (helpWindowOpen && ImGui.BeginWindow("Debug Console Help", ref helpWindowOpen))
        {
            if (ImGui.TreeNode("Available Commands:")) 
            {
                if (ImGui.BeginListBox("", new Vector2(-5, 300)))
                {
                    foreach (var cmd in commands.Keys)
                    {
                        ImGui.Text(cmd.ToLower());
                    }

                    ImGui.EndListBox();
                }
                ImGui.TreePop();
            }
        }
        ImGui.EndWindow();
    }

    public void RegisterCommand(string name, Action command)
    {
        commands.Add(name.ToUpper(), command);
    }

    public void RegisterCommand(string name, Action<string[]> command)
    {
        commands.Add(name.ToUpper(), command);
    }
}
