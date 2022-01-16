using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimulationFramework.IMGUI;

namespace SimulationFramework;

/// <summary>
/// Provides a main debug window (Via F1) to the application as well as functionality for other windows via ImGui.
/// </summary>
public abstract class DebugWindow
{
    private static readonly Dictionary<(Type, int), DebugWindow> windows = new();

    /// <summary>
    /// Whether the window is open or not.
    /// </summary>
    public bool IsOpen { get; protected set; }
    
    /// <summary>
    /// The title of the window.
    /// </summary>
    public string Title { get; protected set; }
    
    /// <summary>
    /// The flags to be passed to BeginWindow()
    /// </summary>
    public WindowFlags WindowFlags { get; protected set; }
    /// <summary>
    /// The window's keybind.
    /// </summary>
    public Key Keybind { get; protected set; }

    /// <summary>
    /// The window's keybind modifier.
    /// </summary>
    public Key Modifier { get; protected set; }

    /// <summary>
    /// Called when the window should layout its contents.
    /// </summary>
    protected abstract void OnLayout();

    static DebugWindow()
    {
        RegisterWindow(new ObjectViewer());
        RegisterWindow(new DebugConsole());
        RegisterWindow(new PerformanceViewer());
    }

    /// <summary>
    /// Creates a new DebugWindow.
    /// </summary>
    /// <param name="title">The title of the window.</param>
    /// <param name="windowFlags">The WindowFlags value to be pass to BeginWindow()</param>
    protected DebugWindow(string title, WindowFlags windowFlags = WindowFlags.None)
    {
        Title = title;
        WindowFlags = windowFlags;
        (Keybind, Modifier) = GetDefaultKeybind();
    }

    /// <summary>
    /// Disable keybinds for SimulationFramework-provided windows.
    /// </summary>
    public static void DisableDefaultWindowKeybinds()
    {
        var defWindows = new DebugWindow[] { GetWindow<ObjectViewer>(), GetWindow<DebugConsole>(), GetWindow<PerformanceViewer>() };

        foreach (var win in defWindows)
        {
            win.Keybind = Key.Unknown;
            win.Modifier = Key.Unknown;
        }
    }

    /// <summary>
    /// Re-enables keybinds for SimulationFramework-provided windows if the were disabled.
    /// </summary>
    public static void RestoreDefaultWindowKeybinds()
    {
        var defWindows = new DebugWindow[] { GetWindow<ObjectViewer>(), GetWindow<DebugConsole>(), GetWindow<PerformanceViewer>() };

        foreach (var win in defWindows)
        {
            (win.Keybind, win.Modifier) = win.GetDefaultKeybind();
        }
    }

    /// <para>F1 - Debug Window</para>
    /// <para>F2 - Debug Console</para>
    /// <summary>
    /// Registers a window to be automatically open, closed, and rendered.
    /// </summary>
    /// <param name="window">The window to register.</param>
    /// <param name="id">The ID of the window, every window of any given type must have a unique ID. IDs differentiate a window within other windows of the same type, meaning that the same ID can be used across mulitple types.</param>
    public static void RegisterWindow(DebugWindow window, int id = 0)
    {
        if (windows.ContainsKey((window.GetType(),id)))
        {
            throw new Exception("There is already a window of this type with that id!");
        }

        windows.Add((window.GetType(), id), window);
    }

    /// <summary>
    /// Gets a registered window of a certain type (and optionally with a certain id).
    /// </summary>
    /// <typeparam name="T">The type of window to find.</typeparam>
    /// <param name="id">The id of the window to find.</param>
    /// <returns>The window if it was found, otherwise null.</returns>
    public static T GetWindow<T>(int id = 0) where T : DebugWindow
    {
        if (!windows.ContainsKey((typeof(T), id)))
            return null;

        return (T)windows[(typeof(T), id)];
    }

    // preforms per-frame logic on each registered window and calls OnLayout() on the open ones
    internal static void Layout()
    {
        foreach (var window in windows.Values)
        {
            if (Keyboard.IsKeyPressed(window.Keybind) && (window.Modifier is Key.Unknown || Keyboard.IsKeyDown(window.Modifier)))
                window.ToggleOpen();

            if (window.IsOpen)
            {
                bool isWindowStillOpen = true;
                if (window.IsOpen && ImGui.BeginWindow(window.Title, ref isWindowStillOpen, window.WindowFlags))
                {
                    window.OnLayout();
                }
                ImGui.EndWindow();
                window.IsOpen = isWindowStillOpen;
            }
        }
    }

    /// <summary>
    /// Toggles the window between opened/closed.
    /// </summary>
    public void ToggleOpen()
    {
        this.IsOpen = !this.IsOpen;
    }

    /// <summary>
    /// Gets the windows default keybinds to revert to them in the case that they were changed.
    /// </summary>
    protected virtual (Key, Key) GetDefaultKeybind()
    {
        return (Key.Unknown, Key.Unknown);
    }
}