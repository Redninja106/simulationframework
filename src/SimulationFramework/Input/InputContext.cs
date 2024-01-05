using SimulationFramework.Components;
using SimulationFramework.Messaging;
using System.Numerics;

namespace SimulationFramework.Input;

/// <summary>
/// Controls the input sent to a simulation.
/// </summary>
public sealed class InputContext : ISimulationComponent
{
    internal event KeyEvent? KeyDown;
    internal event KeyEvent? KeyUp;

    internal readonly List<char> typedKeys = new();

    internal readonly List<Key> lastPressedKeys = new();
    internal readonly List<Key> pressedKeys = new();

    internal readonly List<MouseButton> lastPressedMouseButtons = new();
    internal readonly List<MouseButton> pressedMouseButtons = new();

    internal Vector2 mousePosition;
    internal Vector2 lastMousePosition;
    internal int scrollDelta;

    internal readonly List<GamepadButton> lastGamepadButtons = new();
    internal readonly List<GamepadButton> gamepadButtons = new();

    internal float rightTrigger, leftTrigger;
    internal Vector2 rightJoystick, leftJoystick;

    internal bool isAnyButtonPressed;
    internal bool isAnyButtonReleased;

    internal bool isAnyKeyPressed;
    internal bool isAnyKeyReleased;

    /// <summary>
    /// Updates the state of the provided key.
    /// </summary>
    public void UpdateKey(Key key, bool isDown)
    {
        if (key == Key.Unknown)
            return;

        if (isDown == pressedKeys.Contains(key))
            return;

        if (isDown)
        {
            pressedKeys.Add(key);
            KeyDown?.Invoke(key);
            isAnyKeyPressed = true;
        }
        else
        {
            pressedKeys.Remove(key);
            KeyUp?.Invoke(key);
            isAnyKeyReleased = true;
        }
    }

    /// <summary>
    /// Updates the state of the provided mouse button.
    /// </summary>
    public void UpdateMouseButton(MouseButton button, bool isDown)
    {
        if (isDown == pressedMouseButtons.Contains(button))
            return;

        if (isDown)
        {
            pressedMouseButtons.Add(button);
            isAnyButtonPressed = true;
        }
        else
        {
            pressedMouseButtons.Remove(button);
            isAnyButtonReleased = true;
        }
    }

    /// <summary>
    /// Updates the mouse position.
    /// </summary>
    public void UpdateMousePosition(Vector2 position)
    {
        this.mousePosition = position;
    }


    /// <summary>
    /// Updates the mouse scroll wheel delta.
    /// </summary>
    public void UpdateMouseScroll(int scrollDelta)
    {
        this.scrollDelta = scrollDelta;
    }

    /// <summary>
    /// Updates the state of the gamepad triggers.
    /// </summary>
    public void UpdateGamepadTriggers(float rightTrigger, float leftTrigger)
    {
        this.rightTrigger = rightTrigger;
        this.leftTrigger = leftTrigger;
    }

    /// <summary>
    /// Updates the state of the gamepad joysticks.
    /// </summary>
    public void UpdateGamepadJoysticks(Vector2 rightJoystick, Vector2 leftJoystick)
    {
        this.rightJoystick = rightJoystick;
        this.leftJoystick = leftJoystick;
    }

    /// <summary>
    /// Updates the state of a gamepad button.
    /// </summary>
    public void UpdateGamepadButton(GamepadButton button, bool isDown)
    {
        if (isDown == gamepadButtons.Contains(button))
            return;

        if (isDown)
            gamepadButtons.Add(button);
        else
            gamepadButtons.Remove(button);
    }

    /// <summary>
    /// emulates typing a character
    /// </summary>
    /// <param name="keycode"></param>
    public void SendChar(char keycode)
    {
        typedKeys.Add(keycode);
    }

    internal void NewFrame()
    {
        isAnyButtonPressed = isAnyButtonReleased = false;
        isAnyKeyPressed = isAnyKeyReleased = false;

        lastPressedKeys.Clear();
        lastPressedKeys.AddRange(this.pressedKeys);

        lastPressedMouseButtons.Clear();
        lastPressedMouseButtons.AddRange(this.pressedMouseButtons);

        lastGamepadButtons.Clear();
        lastGamepadButtons.AddRange(gamepadButtons);

        lastMousePosition = mousePosition;
        scrollDelta = 0;
    }

    ///  <inheritdoc/>
    public void Dispose()
    {
    }

    /// <inheritdoc/>
    public void Initialize(MessageDispatcher dispatcher)
    {
        dispatcher.Subscribe<AfterRenderMessage>(m =>
        {
            NewFrame();
        });
    }
}