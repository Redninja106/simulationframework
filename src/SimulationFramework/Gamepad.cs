using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

public static class Gamepad
{
    private static InputContext Context => Application.Current.GetComponent<InputContext>();
    
    public static Vector2 LeftJoystick => Context.leftJoystick;
    public static Vector2 RightJoystick => Context.rightJoystick;

    public static float LeftTrigger => Context.leftTrigger;
    public static float RightTrigger => Context.rightTrigger;

    public static bool IsButtonDown(GamepadButton button) => Context.gamepadButtons.Contains(button);
    public static bool IsButtonReleased(GamepadButton button) => !Context.gamepadButtons.Contains(button) && Context.lastGamepadButtons.Contains(button);
    public static bool IsButtonPressed(GamepadButton button) => Context.gamepadButtons.Contains(button) && !Context.lastGamepadButtons.Contains(button);
}