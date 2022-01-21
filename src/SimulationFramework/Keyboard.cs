using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

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

    /// <summary>
    /// Modifies the provided text based off the typed chars this frame.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="charFilter"></param>
    public static void InputText(ref string text, int cursorPosition, Func<char, bool> charFilter = null)
    {
        foreach (var c in GetChars())
        {
            if (charFilter is null || charFilter(c))
            {
                text = text.Insert(cursorPosition, c.ToString());
            }
        }
    }
}