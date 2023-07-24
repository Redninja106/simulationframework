using System;
using System.Collections.Generic;
using System.Linq;

namespace SimulationFramework.Input;

/// <summary>
/// Provides keyboard input to the simulation.
/// </summary>
public static class Keyboard
{
    private static IKeyboardProvider Provider => Application.GetComponent<IKeyboardProvider>();

    /// <summary>
    /// Invoked when a key is pressed on the keyboard.
    /// </summary>
    public static event KeyEvent KeyPressed
    {
        add => Provider.KeyPressed += value;
        remove => Provider.KeyPressed -= value;
    }

    /// <summary>
    /// Invoked when a key is released on the keyboard.
    /// </summary>
    public static event KeyEvent KeyReleased
    {
        add => Provider.KeyReleased += value;
        remove => Provider.KeyReleased -= value;
    }

    /// <summary>
    /// Invoked when a key is typed. This is usually used for text input.
    /// </summary>
    public static event KeyTypedEvent KeyTyped
    {
        add => Provider.KeyTyped += value;
        remove => Provider.KeyTyped -= value;
    }

    /// <summary>
    /// All of the characters typed this frame. This is usually used for text input. To check for modifiers, use <see cref="IsKeyDown(Key)"/>.
    /// </summary>
    public static IEnumerable<char> TypedKeys => Provider.TypedKeys;

    /// <summary>
    /// Gets a collection of all keys which are held this frame.
    /// </summary>
    public static IEnumerable<Key> HeldKeys => Provider.HeldKeys;

    /// <summary>
    /// Gets a collection of all keys pressed this frame. A key is only considered pressed on the first frame that it is held.
    /// </summary>
    public static IEnumerable<Key> PressedKeys => Provider.PressedKeys;

    /// <summary>
    /// Gets a collection of all keys released this frame. A key is only considered released on the first frame that it is not held.
    /// </summary>
    public static IEnumerable<Key> ReleasedKeys => Provider.ReleasedKeys;

    /// <summary>
    /// Returns true if the provided key is pressed.
    /// </summary>
    public static bool IsKeyDown(Key key) => Provider.HeldKeys.Contains(key); 

    /// <summary>
    /// Returns true if the provided key is pressed this frame and was not pressed the frame before.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static bool IsKeyPressed(Key key) => Provider.PressedKeys.Contains(key);
    /// <summary>
    /// Returns true if the provided key is pressed this frame and was not pressed the frame before.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static bool IsKeyReleased(Key key) => Provider.ReleasedKeys.Contains(key);
}