using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimulationFramework.Components;

namespace SimulationFramework;

/// <summary>
/// A delegate for key up/down events.
/// </summary>
public delegate void KeyEvent(Key key);

/// <summary>
/// Provides keyboard input to the simulation.
/// </summary>
public static class Keyboard
{
    internal static InputContext Context => Application.GetComponent<InputContext>();

    /// <summary>
    /// Returns true if the provided key is pressed.
    /// </summary>
    public static bool IsKeyDown(Key key) => Context.pressedKeys.Contains(key);
    
    /// <summary>
    /// Returns true if the provided key is pressed this frame and was not pressed the frame before.
    /// </summary>
    public static bool IsKeyPressed(Key key) => Context.pressedKeys.Contains(key) && !Context.lastPressedKeys.Contains(key);
    /// <summary>
    /// Returns true if the provided key is not pressed this frame and was pressed the frame before.
    /// </summary>
    public static bool IsKeyReleased(Key key) => !Context.pressedKeys.Contains(key) && Context.lastPressedKeys.Contains(key);

    /// <summary>
    /// Gets the keys typed this frame.
    /// </summary>
    public static IEnumerable<char> GetChars() => Context.typedKeys;

    /// <summary>
    /// Invoked when a key is pressed on the keyboard.
    /// </summary>
    public static event KeyEvent KeyPressed
    {
        add => Context.KeyDown += value;
        remove => Context.KeyUp -= value;
    }

    /// <summary>
    /// Invoked when a key is released on the keyboard.
    /// </summary>
    public static event KeyEvent KeyReleased
    {
        add => Context.KeyDown += value;
        remove => Context.KeyUp -= value;
    }

    public static bool IsAnyKeyDown()
    {
        return Context.pressedKeys.Any();
    }
    
    public static bool IsAnyKeyDown(params Key[] keys)
    {
        return IsAnyKeyDown(keys.AsSpan());
    }

    public static bool IsAnyKeyDown(ReadOnlySpan<Key> keys)
    {
        for (int i = 0; i < keys.Length; i++)
        {
            if (IsKeyDown(keys[i]))
                return true;
        }

        return false;
    }

    public static bool IsAnyKeyPressed()
    {
        return Context.isAnyKeyPressed;
    }

    public static bool IsAnyKeyPressed(params Key[] keys)
    {
        return IsAnyKeyPressed(keys.AsSpan());
    }

    public static bool IsAnyKeyPressed(ReadOnlySpan<Key> keys)
    {
        for (int i = 0; i < keys.Length; i++)
        {
            if (IsKeyPressed(keys[i]))
                return true;
        }

        return false;
    }

    public static bool IsAnyKeyReleased()
    {
        return Context.isAnyKeyReleased;
    }

    public static bool IsAnyKeyReleased(params Key[] keys)
    {
        return IsAnyKeyReleased(keys.AsSpan());
    }

    public static bool IsAnyKeyReleased(ReadOnlySpan<Key> keys)
    {
        for (int i = 0; i < keys.Length; i++)
        {
            if (IsKeyReleased(keys[i]))
                return true;
        }

        return false;
    }
}