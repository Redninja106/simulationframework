using Silk.NET.Input;
using SimulationFramework.Input;
using SimulationFramework.Messaging;

namespace SimulationFramework.Desktop;
internal class DesktopKeyboardProvider : IKeyboardProvider
{
    public IEnumerable<char> TypedKeys => typedChars;
    public IEnumerable<Key> HeldKeys => heldKeys;
    public IEnumerable<Key> PressedKeys => pressedKeys;
    public IEnumerable<Key> ReleasedKeys => releasedKeys;

    public event KeyEvent? KeyPressed;
    public event KeyEvent? KeyReleased;
    public event KeyTypedEvent? KeyTyped;

    private readonly List<char> typedChars = new();
    private readonly List<Key> heldKeys = new();
    private readonly List<Key> pressedKeys = new();
    private readonly List<Key> releasedKeys = new();
    internal bool capturedByImgui;

    public DesktopKeyboardProvider(IKeyboard keyboard)
    {
        keyboard.KeyDown += Keyboard_KeyDown;
        keyboard.KeyUp += Keyboard_KeyUp;
        keyboard.KeyChar += Keyboard_KeyChar;
    }

    private void Keyboard_KeyChar(IKeyboard arg1, char arg2)
    {
        typedChars.Add(arg2);
        KeyTyped?.Invoke(arg2);
    }

    private void Keyboard_KeyUp(IKeyboard arg1, SilkKey arg2, int arg3)
    {
        var key = ConvertKey(arg2);

        heldKeys.Remove(key);

        releasedKeys.Add(key);
        KeyReleased?.Invoke(key);
    }

    private void Keyboard_KeyDown(IKeyboard arg1, SilkKey arg2, int arg3)
    {
        var key = ConvertKey(arg2);

        if (!heldKeys.Contains(key))
            heldKeys.Add(key);

        pressedKeys.Add(key);
        KeyPressed?.Invoke(key);
    }

    public void Initialize(MessageDispatcher dispatcher)
    {
        dispatcher.Subscribe<BeforeEventsMessage>(m => BeforeEvents());
    }

    private void BeforeEvents()
    {
        typedChars.Clear();
        pressedKeys.Clear();
        releasedKeys.Clear();
    }

    public void Dispose()
    {
    }

    private static Key ConvertKey(SilkKey key)
    {
        if (key >= SilkKey.A && key <= SilkKey.Z)
            return (Key)(key - 64);

        return key switch
        {
            SilkKey.Number0 => Key.Key0,
            SilkKey.Number1 => Key.Key1,
            SilkKey.Number2 => Key.Key2,
            SilkKey.Number3 => Key.Key3,
            SilkKey.Number4 => Key.Key4,
            SilkKey.Number5 => Key.Key5,
            SilkKey.Number6 => Key.Key6,
            SilkKey.Number7 => Key.Key7,
            SilkKey.Number8 => Key.Key8,
            SilkKey.Number9 => Key.Key9,
            SilkKey.Keypad0 => Key.Numpad0,
            SilkKey.Keypad1 => Key.Numpad1,
            SilkKey.Keypad2 => Key.Numpad2,
            SilkKey.Keypad3 => Key.Numpad3,
            SilkKey.Keypad4 => Key.Numpad4,
            SilkKey.Keypad5 => Key.Numpad5,
            SilkKey.Keypad6 => Key.Numpad6,
            SilkKey.Keypad7 => Key.Numpad7,
            SilkKey.Keypad8 => Key.Numpad8,
            SilkKey.Keypad9 => Key.Numpad9,
            SilkKey.F1 => Key.F1,
            SilkKey.F2 => Key.F2,
            SilkKey.F3 => Key.F3,
            SilkKey.F4 => Key.F4,
            SilkKey.F5 => Key.F5,
            SilkKey.F6 => Key.F6,
            SilkKey.F7 => Key.F7,
            SilkKey.F8 => Key.F8,
            SilkKey.F9 => Key.F9,
            SilkKey.F10 => Key.F10,
            SilkKey.F11 => Key.F11,
            SilkKey.F12 => Key.F12,
            SilkKey.Space => Key.Space,
            SilkKey.AltLeft => Key.LeftAlt,
            SilkKey.ControlLeft => Key.LeftControl,
            SilkKey.ShiftLeft => Key.LeftShift,
            SilkKey.CapsLock => Key.CapsLock,
            SilkKey.Tab => Key.Tab,
            SilkKey.GraveAccent => Key.Tilde,
            SilkKey.Escape => Key.Esc,
            SilkKey.AltRight => Key.RightAlt,
            SilkKey.Menu => Key.Menu,
            SilkKey.ControlRight => Key.RightControl,
            SilkKey.ShiftRight => Key.RightShift,
            SilkKey.Enter => Key.Enter,
            SilkKey.Backspace => Key.Backspace,
            SilkKey.Comma => Key.Comma,
            SilkKey.Period => Key.Period,
            SilkKey.Slash => Key.Slash,
            SilkKey.Semicolon => Key.Semicolon,
            SilkKey.Apostrophe => Key.Apostrophe,
            SilkKey.LeftBracket => Key.OpenBracket,
            SilkKey.RightBracket => Key.CloseBracket,
            SilkKey.BackSlash => Key.BackSlash,
            SilkKey.Minus => Key.Minus,
            SilkKey.Equal => Key.Plus,
            SilkKey.Up => Key.UpArrow,
            SilkKey.Left => Key.LeftArrow,
            SilkKey.Down => Key.DownArrow,
            SilkKey.Right => Key.RightArrow,
            SilkKey.Insert => Key.Insert,
            SilkKey.Home => Key.Home,
            SilkKey.PageUp => Key.PageUp,
            SilkKey.Delete => Key.Delete,
            SilkKey.End => Key.End,
            SilkKey.PageDown => Key.PageDown,
            _ => Key.Unknown,
        };
    }
}
