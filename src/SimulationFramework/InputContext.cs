using SimulationFramework.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

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
        }
        else
        {
            pressedKeys.Remove(key);
            KeyUp?.Invoke(key);
        }
    }

    public void UpdateMouseButton(MouseButton button, bool isDown)
    {
        if (isDown == pressedMouseButtons.Contains(button))
            return;

        if (isDown)
            pressedMouseButtons.Add(button);
        else
            pressedMouseButtons.Remove(button);
    }

    public void UpdateMousePosition(Vector2 position)
    {
        this.mousePosition = position;
    }

    public void UpdateMouseScroll(int scrollDelta)
    {
        this.scrollDelta = scrollDelta;
    }

    public void UpdateGamepadTriggers(float rightTrigger, float leftTrigger)
    {
        this.rightTrigger = rightTrigger;
        this.leftTrigger = leftTrigger;
    }

    public void UpdateGamepadJoysticks(Vector2 rightJoystick, Vector2 leftJoystick)
    {
        this.rightJoystick = rightJoystick;
        this.leftJoystick = leftJoystick;
    }

    public void UpdateGamepadButton(GamepadButton button, bool isDown)
    {
        if (isDown == gamepadButtons.Contains(button))
            return;

        if (isDown)
            gamepadButtons.Add(button);
        else
            gamepadButtons.Remove(button);
    }

    public void SendChar(char keycode)
    {
        typedKeys.Add(keycode);
    }

    internal void NewFrame()
    {
        lastPressedKeys.Clear();
        lastPressedKeys.AddRange(this.pressedKeys);

        lastPressedMouseButtons.Clear();
        lastPressedMouseButtons.AddRange(this.pressedMouseButtons);

        lastGamepadButtons.Clear();
        lastGamepadButtons.AddRange(gamepadButtons);

        lastMousePosition = mousePosition;
        scrollDelta = 0;
    }

    public void Dispose()
    {
    }
    
    public void Initialize(MessageDispatcher dispatcher)
    {
        dispatcher.Subscribe<FrameBeginMessage>(m =>
        {
            NewFrame();
        }, ListenerPriority.High);
    }
}