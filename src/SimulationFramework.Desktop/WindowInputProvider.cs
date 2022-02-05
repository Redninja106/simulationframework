using Silk.NET.Windowing;
using Silk.NET.Input;
using SilkKey = Silk.NET.Input.Key;
using SilkButton = Silk.NET.Input.MouseButton;
using SimulationFramework.IMGUI;

namespace SimulationFramework.Desktop;

public sealed partial class WindowEnvironment
{
    private class WindowInputProvider : ISimulationComponent
    {
        private readonly IInputContext silkInputDevice;
        private InputContext Context;
        private float lastScrollPos;

        public WindowInputProvider(IWindow window)
        {
            silkInputDevice = window.CreateInput();

        }

        private void Scroll(IMouse arg1, ScrollWheel arg2)
        {
            Context.UpdateMouseScroll((int)(arg2.Y - lastScrollPos));
            lastScrollPos = arg2.Y;
        }

        private void MouseMove(IMouse arg1, System.Numerics.Vector2 arg2)
        {
            Context.UpdateMousePosition(arg2);
        }

        private void KeyChar(IKeyboard arg1, char arg2)
        {
            Context.SendChar(arg2);
        }

        private void KeyUp(IKeyboard arg1, SilkKey arg2, int arg3)
        {
            Context.UpdateKey(ConvertKey(arg2), false);
        }

        private void KeyDown(IKeyboard arg1, SilkKey arg2, int arg3)
        {
            Context.UpdateKey(ConvertKey(arg2), true);
        }

        private void MouseDown(IMouse arg1, SilkButton arg2)
        {
            Context.UpdateMouseButton(ConvertButton(arg2), true);
        }

        private void MouseUp(IMouse arg1, SilkButton arg2)
        {
            Context.UpdateMouseButton(ConvertButton(arg2), false);
        }

        public void Apply(Simulation simulation)
        {
            this.Context = simulation.InputContext;

            silkInputDevice.Mice[0].MouseUp += MouseUp;
            silkInputDevice.Mice[0].MouseDown += MouseDown;
            silkInputDevice.Mice[0].MouseMove += MouseMove;
            silkInputDevice.Mice[0].Scroll += Scroll;

            silkInputDevice.Keyboards[0].KeyDown += KeyDown;
            silkInputDevice.Keyboards[0].KeyUp += KeyUp;
            silkInputDevice.Keyboards[0].KeyChar += KeyChar;

            simulation.Render += Simulation_BeforeRender;
        }

        private void Simulation_BeforeRender()
        {
            if (ImGui.BeginListBox("", default))
            {
                foreach (var gamepad in silkInputDevice.Gamepads)
                {
                    if (ImGui.Button(gamepad.Name))
                    {
                        ObjectViewer.Select(new ObjectAutoViewer(gamepad), true);
                    }
                }

                ImGui.EndListBox();
            }
        }

        public void Dispose()
        {
            silkInputDevice.Dispose();
        }

        private static Key ConvertKey(SilkKey key)
        {
            if (key >= SilkKey.A && key <= SilkKey.Z)
                return (Key)(key - 64);

            return key switch
            {
                SilkKey.Number0 => Key.Key0 ,
                SilkKey.Number1 => Key.Key1 ,
                SilkKey.Number2 => Key.Key2 ,
                SilkKey.Number3 => Key.Key3 ,
                SilkKey.Number4 => Key.Key4 ,
                SilkKey.Number5 => Key.Key5 ,
                SilkKey.Number6 => Key.Key6 ,
                SilkKey.Number7 => Key.Key7 ,
                SilkKey.Number8 => Key.Key8 ,
                SilkKey.Number9 => Key.Key9 ,
                SilkKey.Keypad0 => Key.Numpad0 ,
                SilkKey.Keypad1 => Key.Numpad1 ,
                SilkKey.Keypad2 => Key.Numpad2 ,
                SilkKey.Keypad3 => Key.Numpad3 ,
                SilkKey.Keypad4 => Key.Numpad4 ,
                SilkKey.Keypad5 => Key.Numpad5 ,
                SilkKey.Keypad6 => Key.Numpad6 ,
                SilkKey.Keypad7 => Key.Numpad7 ,
                SilkKey.Keypad8 => Key.Numpad8 ,
                SilkKey.Keypad9 => Key.Numpad9 ,
                SilkKey.F1 => Key.F1 ,
                SilkKey.F2 => Key.F2 ,
                SilkKey.F3 => Key.F3 ,
                SilkKey.F4 => Key.F4 ,
                SilkKey.F5 => Key.F5 ,
                SilkKey.F6 => Key.F6 ,
                SilkKey.F7 => Key.F7 ,
                SilkKey.F8 => Key.F8 ,
                SilkKey.F9 => Key.F9 ,
                SilkKey.F10 => Key.F10 ,
                SilkKey.F11 => Key.F11 ,
                SilkKey.F12 => Key.F12 ,
                SilkKey.Space => Key.Space ,
                SilkKey.AltLeft => Key.LAlt ,
                SilkKey.ControlLeft => Key.LCtrl ,
                SilkKey.ShiftLeft => Key.LShift ,
                SilkKey.CapsLock => Key.CapsLock ,
                SilkKey.Tab => Key.Tab ,
                SilkKey.GraveAccent => Key.Tilde ,
                SilkKey.Escape => Key.Esc ,
                SilkKey.AltRight => Key.RAlt ,
                SilkKey.Menu => Key.Menu ,
                SilkKey.ControlRight => Key.RCtrl ,
                SilkKey.ShiftRight => Key.RShift ,
                SilkKey.Enter => Key.Enter ,
                SilkKey.Backspace => Key.Backspace ,
                SilkKey.Comma => Key.Comma ,
                SilkKey.Period => Key.Period ,
                SilkKey.Slash => Key.Slash ,
                SilkKey.Semicolon => Key.Semicolon ,
                SilkKey.Apostrophe => Key.Apostrophe ,
                SilkKey.LeftBracket => Key.LBracket ,
                SilkKey.RightBracket => Key.RBracket ,
                SilkKey.BackSlash => Key.BackSlash ,
                SilkKey.Minus => Key.Minus ,
                SilkKey.Equal => Key.Plus ,
                SilkKey.Up => Key.UpArrow ,
                SilkKey.Left => Key.LeftArrow ,
                SilkKey.Down => Key.DownArrow ,
                SilkKey.Right => Key.RightArrow ,
                SilkKey.Insert => Key.Insert ,
                SilkKey.Home => Key.Home ,
                SilkKey.PageUp => Key.PageUp ,
                SilkKey.Delete => Key.Delete ,
                SilkKey.End => Key.End ,
                SilkKey.PageDown => Key.PageDown,
                _ => Key.Unknown,
            };
        }

        private static MouseButton ConvertButton(SilkButton button)
        {
            return button switch
            {
                SilkButton.Left => MouseButton.Left,
                SilkButton.Right => MouseButton.Right ,
                SilkButton.Middle => MouseButton.Middle ,
                _ => throw new Exception(),
            };
        }

    }
}