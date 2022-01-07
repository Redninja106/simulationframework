using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

public static class Mouse
{
    internal static IInputProvider Provider => Simulation.Current.GetComponent<IInputProvider>();

    public static Vector2 Position => Provider.GetMousePosition();
    public static Vector2 Delta => Provider.GetMousePosition();
    public static int ScrollWheelDelta => Provider.GetScrollWheelDelta();
    public static bool IsButtonDown(MouseButton button) => Provider.IsMouseButtonDown(button);
    public static bool IsButtonUp(MouseButton button) => !Provider.IsMouseButtonDown(button);
    public static bool IsButtonPressed(MouseButton button) => Provider.IsMouseButtonPressed(button);
    public static bool IsButtonReleased(MouseButton button) => Provider.IsMouseButtonReleased(button);
}