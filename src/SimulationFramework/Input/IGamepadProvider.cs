using SimulationFramework.Components;
using System.Numerics;

namespace SimulationFramework.Input;

/// <summary>
/// Provides gamepad input to the simulation.
/// </summary>
public interface IGamepadProvider : ISimulationComponent
{
    /// <summary>
    /// Invoked when a button is pressed.
    /// </summary>
    event GamepadButtonEvent ButtonPressed;

    /// <summary>
    /// Invoked when a button is released.
    /// </summary>
    event GamepadButtonEvent ButtonReleased;

    /// <summary>
    /// The value of the gamepad's left joystick.
    /// </summary>
    Vector2 LeftJoystick { get; }

    /// <summary>
    /// The value of the gamepad's right joystick.
    /// </summary>
    Vector2 RightJoystick { get; }

    /// <summary>
    /// Gets the value of the gamepad's left trigger, between 0 (not hold) and 1 (fully held).
    /// </summary>
    float LeftTrigger { get; }

    /// <summary>
    /// Gets the value of the gamepad's right trigger, between 0 (not held) and 1 (fully held).
    /// </summary>
    float RightTrigger { get; }

    /// <summary>
    /// Controls the strength of the gamepads's haptic feedback, between 0 (no vibration) and 1 (max vibration).
    /// </summary>
    float VibrationStrength { get; set; }

    /// <summary>
    /// A collection of all buttons which are pressed this frame. A button is only considered pressed on the first frame that it is held.
    /// </summary>
    IEnumerable<GamepadButton> HeldButtons { get; }

    /// <summary>
    /// A collection of all buttons which are pressed this frame. A button is only considered pressed on the first frame that it is held.
    /// </summary>
    IEnumerable<GamepadButton> PressedButtons { get; }

    /// <summary>
    /// A collection of all buttons which are pressed this frame. A button is only considered released on the first frame that it is not held.
    /// </summary>
    IEnumerable<GamepadButton> ReleasedButtons { get; }
}
