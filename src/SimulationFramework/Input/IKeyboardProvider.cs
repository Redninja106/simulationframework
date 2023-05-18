using SimulationFramework.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Input;

/// <summary>
/// Provides keyboard input to the simulation.
/// </summary>
public interface IKeyboardProvider : ISimulationComponent
{
    /// <summary>
    /// Invoked when a key is pressed.
    /// </summary>
    event KeyEvent KeyPressed;

    /// <summary>
    /// Invoked when a key is released.
    /// </summary>
    event KeyEvent KeyReleased;

    /// <summary>
    /// Invoked when a char is typed. This is usually used for text input.
    /// </summary>
    event KeyTypedEvent KeyTyped;

    /// <summary>
    /// All of the characters typed this frame. This is usually used for text input.
    /// </summary>
    IEnumerable<char> TypedKeys { get; }

    /// <summary>
    /// A collection of all keys which are held this frame.
    /// </summary>
    IEnumerable<Key> HeldKeys { get; }

    /// <summary>
    /// A collection of all keys pressed this frame. A key is only considered pressed on the first frame that it is held.
    /// </summary>
    IEnumerable<Key> PressedKeys { get; }

    /// <summary>
    /// A collection of all keys released this frame. A key is only considered released on the first frame that it is not held.
    /// </summary>
    IEnumerable<Key> ReleasedKeys { get; }
}