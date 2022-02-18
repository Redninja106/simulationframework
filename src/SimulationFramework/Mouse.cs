using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

/// <summary>
/// Provides mouse input to the application.
/// </summary>
public static class Mouse
{
    internal static InputContext Context => Simulation.Current.InputContext;

    /// <summary>
    /// The position of the mouse.
    /// </summary>
    public static Vector2 Position => Context.mousePosition;
    /// <summary>
    /// The distance the mouse has moved since the last frame.
    /// </summary>
    public static Vector2 DeltaPosition => Context.lastMousePosition - Context.mousePosition;
    /// <summary>
    /// The distance the scroll whell has moved since the last frame.
    /// </summary>
    public static int ScrollWheelDelta => Context.scrollDelta;

    /// <summary>
    /// Returns <see langword="true"/> if the provided button is down, otherwise <see langword="false"/>.
    /// </summary>
    public static bool IsButtonDown(MouseButton button) => Context.pressedMouseButtons.Contains(button);
    
    /// <summary>
    /// Returns <see langword="true"/> if the provided button was just pressed this frame, otherwise <see langword="false"/>.
    /// </summary>
    public static bool IsButtonPressed(MouseButton button) => Context.pressedMouseButtons.Contains(button) && !Context.lastPressedMouseButtons.Contains(button);
    
    /// <summary>
    /// Returns <see langword="true"/> if the provided button was just released this frame, otherwise <see langword="false"/>.
    /// </summary>
    public static bool IsButtonReleased(MouseButton button) => !Context.pressedMouseButtons.Contains(button) && Context.lastPressedMouseButtons.Contains(button);
}