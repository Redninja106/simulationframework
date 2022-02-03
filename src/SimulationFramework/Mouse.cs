using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

public static class Mouse
{
    internal static InputContext Context => Simulation.Current.InputContext;

    public static Vector2 Position => Context.mousePosition;
    public static Vector2 DeltaPosition => Context.lastMousePosition - Context.mousePosition;
    public static int ScrollWheelDelta => Context.scrollDelta;
    public static bool IsButtonDown(MouseButton button) => Context.pressedMouseButtons.Contains(button);
    public static bool IsButtonUp(MouseButton button) => !Context.pressedMouseButtons.Contains(button);
    public static bool IsButtonPressed(MouseButton button) => Context.pressedMouseButtons.Contains(button) && !Context.lastPressedMouseButtons.Contains(button);
    public static bool IsButtonReleased(MouseButton button) => !Context.pressedMouseButtons.Contains(button) && Context.lastPressedMouseButtons.Contains(button);
}