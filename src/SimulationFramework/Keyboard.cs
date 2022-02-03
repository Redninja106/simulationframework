using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

public delegate void KeyEvent(Key key);

/// <summary>
/// Provides keyboard input to the simulation.
/// </summary>
public static class Keyboard
{
    internal static IInputProvider Provider => Simulation.Current.GetComponent<IInputProvider>();

    /// <summary>
    /// Returns true if the provided key is pressed.
    /// </summary>
    public static bool IsKeyDown(Key key) => Provider.IsKeyDown(key);
    /// <summary>
    /// Returns true if the provided key is not pressed.
    /// </summary>
    public static bool IsKeyUp(Key key) => !Provider.IsKeyDown(key);

    /// <summary>
    /// Returns true if the provided key is pressed this frame and was not pressed the frame before.
    /// </summary>
    public static bool IsKeyPressed(Key key) => Provider.IsKeyPressed(key);
    /// <summary>
    /// Returns true if the provided key is not pressed this frame and was pressed the frame before.
    /// </summary>
    public static bool IsKeyReleased(Key key) => Provider.IsKeyReleased(key);

    /// <summary>
    /// Gets the keys typed this frame.
    /// </summary>
    public static char[] GetChars() => Provider.GetChars();

    public static event KeyEvent KeyPressed 
    { 
        add { Provider.KeyPressed += value; }
        remove { Provider.KeyPressed -= value; }
    }

    public static event KeyEvent KeyReleased
    {
        add { Provider.KeyReleased += value; }
        remove { Provider.KeyReleased -= value; }
    }
}