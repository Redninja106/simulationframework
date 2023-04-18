using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using SimulationFramework.Components;

namespace SimulationFramework;

/// <summary>
/// Provides gamepad input to the application.
/// </summary>
public static class Gamepad
{
    private static InputContext Context => Application.GetComponent<InputContext>();
    
    /// <summary>
    /// The left joystick value of the gamepad.
    /// </summary>
    public static Vector2 LeftJoystick => Context.leftJoystick;

    /// <summary>
    /// The right joystick value of the gamepad.
    /// </summary>
    public static Vector2 RightJoystick => Context.rightJoystick;

    /// <summary>
    /// The left trigger value of the gamepad.
    /// </summary>
    public static float LeftTrigger => Context.leftTrigger;

    /// <summary>
    /// The right trigger value of the gamepad.
    /// </summary>
    public static float RightTrigger => Context.rightTrigger;

    /// <summary>
    /// Returns <see langword="true"/> if the provided button is down, otherwise <see langword="false"/>.
    /// </summary>
    public static bool IsButtonDown(GamepadButton button) => Context.gamepadButtons.Contains(button);

    /// <summary>
    /// Returns <see langword="true"/> if the provided button was released this frame, otherwise <see langword="false"/>.
    /// </summary>
    public static bool IsButtonReleased(GamepadButton button) => !Context.gamepadButtons.Contains(button) && Context.lastGamepadButtons.Contains(button);

    /// <summary>
    /// Returns <see langword="true"/> if the provided button was pressed this frame, otherwise <see langword="false"/>.
    /// </summary>
    public static bool IsButtonPressed(GamepadButton button) => Context.gamepadButtons.Contains(button) && !Context.lastGamepadButtons.Contains(button);
}