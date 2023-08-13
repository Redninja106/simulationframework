namespace SimulationFramework.Input;

/// <summary>
/// A delegate for key up and down events.
/// </summary>
public delegate void KeyEvent(Key key);

/// <summary>
/// A delegate for mouse button up and down events.
/// </summary>
public delegate void MouseButtonEvent(MouseButton button);

/// <summary>
/// A delegate for gamepad button up and down events.
/// </summary>
public delegate void GamepadButtonEvent(GamepadButton button);

/// <summary>
/// A delegate for text input events.
/// </summary>
public delegate void KeyTypedEvent(char character);