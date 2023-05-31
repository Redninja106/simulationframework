using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

/// <summary>
/// Provides mouse input to the application.
/// </summary>
public static class Mouse
{
    internal static InputContext Context => Application.Current?.GetComponent<InputContext>() ?? throw Exceptions.CoreComponentNotFound();

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
    /// Returns <see langword="true"/> if the provided button is held down, otherwise <see langword="false"/>.
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

    /// <summary>
    /// Returns true if any button is held down.
    /// </summary>
    public static bool IsAnyButtonDown() => Context.pressedMouseButtons.Any();

    /// <summary>
    /// Returns true if any of the given buttons are held down.
    /// </summary>
    /// <param name="mouseButtons"></param>
    /// <returns></returns>
    public static bool IsAnyButtonDown(params MouseButton[] mouseButtons) => IsAnyButtonDown(mouseButtons.AsSpan());

    /// <summary>
    /// Returns true if any of the given buttons are held down.
    /// </summary>
    public static bool IsAnyButtonDown(ReadOnlySpan<MouseButton> buttons)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (IsButtonDown(buttons[i]))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Returns true if any of the given buttons were just released this frame.
    /// </summary>
    public static bool IsAnyButtonReleased() => Context.isAnyButtonReleased;


    /// <summary>
    /// Returns true if any of the given buttons were just released this frame.
    /// </summary>
    public static bool IsAnyButtonReleased(params MouseButton[] mouseButtons) => IsAnyButtonReleased(mouseButtons.AsSpan());


    /// <summary>
    /// Returns true if any of the given buttons were just released this frame.
    /// </summary>
    public static bool IsAnyButtonReleased(ReadOnlySpan<MouseButton> buttons)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (IsButtonReleased(buttons[i]))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Returns true if any of the given buttons were just pressed this frame.
    /// </summary>
    public static bool IsAnyButtonPressed() => Context.isAnyButtonPressed;


    /// <summary>
    /// Returns true if any of the given buttons were just pressed this frame.
    /// </summary>
    public static bool IsAnyButtonPressed(params MouseButton[] mouseButtons) => IsAnyButtonPressed(mouseButtons.AsSpan());


    /// <summary>
    /// Returns true if any of the given buttons were just pressed this frame.
    /// </summary>
    public static bool IsAnyButtonPressed(ReadOnlySpan<MouseButton> buttons)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (IsButtonPressed(buttons[i]))
                return true;
        }

        return false;
    }
}