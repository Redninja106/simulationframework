using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Input;

/// <summary>
/// Provides gamepad input to the application.
/// </summary>
public static class Gamepad
{
    private static IGamepadProvider Provider => Application.GetComponent<IGamepadProvider>();

    /// <summary>
    /// Invoked when a button is pressed.
    /// </summary>
    public static event GamepadButtonEvent ButtonPressed
    {
        add => Provider.ButtonPressed += value;
        remove => Provider.ButtonPressed -= value;
    }
    /// <summary>
    /// Invoked when a button is released.
    /// </summary>
    public static event GamepadButtonEvent ButtonReleased
    {
        add => Provider.ButtonReleased += value;
        remove => Provider.ButtonReleased -= value;
    }

    /// <summary>
    /// The left joystick value of the gamepad.
    /// </summary>
    public static Vector2 LeftJoystick => Provider.LeftJoystick;

    /// <summary>
    /// The right joystick value of the gamepad.
    /// </summary>
    public static Vector2 RightJoystick => Provider.RightJoystick;

    /// <summary>
    /// The left trigger value of the gamepad.
    /// </summary>
    public static float LeftTrigger => Provider.LeftTrigger;

    /// <summary>
    /// The right trigger value of the gamepad.
    /// </summary>
    public static float RightTrigger => Provider.RightTrigger;

    /// <summary>
    /// A collection of all buttons which are pressed this frame. A button is only considered pressed on the first frame that it is held.
    /// </summary>
    public static IEnumerable<GamepadButton> HeldButtons => Provider.HeldButtons;

    /// <summary>
    /// A collection of all buttons which are pressed this frame. A button is only considered pressed on the first frame that it is held.
    /// </summary>
    public static IEnumerable<GamepadButton> PressedButtons => Provider.PressedButtons;

    /// <summary>
    /// A collection of all buttons which are pressed this frame. A button is only considered released on the first frame that it is not held.
    /// </summary>
    public static IEnumerable<GamepadButton> ReleasedButtons => Provider.ReleasedButtons;

    /// <summary>
    /// Returns <see langword="true"/> if the provided button is down, otherwise <see langword="false"/>.
    /// </summary>
    public static bool IsButtonDown(GamepadButton button) => Provider.HeldButtons.Contains(button);

    /// <summary>
    /// Returns <see langword="true"/> if the provided button was released this frame, otherwise <see langword="false"/>.
    /// </summary>
    public static bool IsButtonReleased(GamepadButton button) => Provider.ReleasedButtons.Contains(button);

    /// <summary>
    /// Returns <see langword="true"/> if the provided button was pressed this frame, otherwise <see langword="false"/>.
    /// </summary>
    public static bool IsButtonPressed(GamepadButton button) => Provider.PressedButtons.Contains(button);
}