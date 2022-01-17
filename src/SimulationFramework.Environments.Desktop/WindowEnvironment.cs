using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Silk.NET.Windowing;
using Silk.NET.Input;
using SimulationFramework.SkiaSharp;
using SkiaSharp;
using SilkKey = Silk.NET.Input.Key;
using SilkButton = Silk.NET.Input.MouseButton;
using Silk.NET.OpenGL.Extensions.ImGui;
using ImGuiNET;
using Silk.NET.OpenGL;
using SimulationFramework.IMGUI;

namespace SimulationFramework;

/// <summary>
/// Implements a simulation environment which runs the simulation in a window.
/// </summary>
public sealed class WindowEnvironment : ISimulationEnvironment
{
    private readonly IWindow window;

    public WindowEnvironment(string title, int width, int height, bool resizable)
    {
        window = Window.Create(WindowOptions.Default with
        {
            Size = new(width, height),
            Title = title,
            WindowBorder = resizable ? WindowBorder.Resizable : WindowBorder.Fixed,
        });
        window.Initialize();
        MakeContextCurrent();
    }

    public void MakeContextCurrent()
    {
        window.MakeCurrent();
    }

    public void Dispose()
    {
    }

    public IEnumerable<ISimulationComponent> CreateSupportedComponents()
    {
        yield return new RealtimeProvider();

        var frameProvider = new WindowFrameProvider(window.Size.X, window.Size.Y);

        window.FramebufferResize += size =>
        {
            frameProvider.Resize(size.X, size.Y);
        };

        yield return new SkiaGraphicsProvider(frameProvider, name =>
        {
            window.GLContext.TryGetProcAddress(name, out nint addr);
            return addr;
        });

        yield return new WindowInputProvider(window);
        yield return new ImGuiNETProvider(new WindowImGuiBackend(window));
    }

    public void ProcessEvents()
    {
        window.DoEvents();
    }

    public bool ShouldExit()
    {
        return window.IsClosing;
    }

    public void EndFrame()
    {
        window.GLContext.SwapBuffers();
    }

    public (int, int) GetOutputSize()
    {
        return (window.Size.X, window.Size.Y);
    }

    private class WindowFrameProvider : ISkiaFrameProvider
    {
        private GRContext context;
        private int width;
        private int height;

        public WindowFrameProvider(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        private GRBackendRenderTarget backendRenderTarget;
        private SKSurface frameSurface;

        public void Resize(int width, int height)
        {
            backendRenderTarget?.Dispose();
            frameSurface?.Dispose();

            const SKColorType format = SKColorType.Rgba8888;
            backendRenderTarget = new GRBackendRenderTarget(width, height, 1, 32, new GRGlFramebufferInfo { Format = format.ToGlSizedFormat(), FramebufferObjectId = 0 });
            frameSurface = SKSurface.Create(context, backendRenderTarget, format);

            this.width = width;
            this.height = height;
        }

        SKCanvas ISkiaFrameProvider.GetCurrentFrame()
        {
            return frameSurface.Canvas;
        }

        public void SetContext(GRContext context)
        {
            this.context = context;
            Resize(width, height);
        }
    }

    private class WindowInputProvider : IInputProvider
    {
        private readonly IInputContext inputContext;

        private bool mouseCaptured;
        private bool keyboardCaptured;
        private bool textCaptured;

        private Vector2 mouseDelta;
        private Vector2 mousePosition;
        private int scrollWheel;
        private List<SilkKey> lastPressedKeys = new();
        private List<SilkKey> pressedKeys = new();
        private List<SilkButton> lastPressedButtons = new();
        private List<SilkButton> pressedButtons = new();

        public WindowInputProvider(IWindow window)
        {
            this.inputContext = window.CreateInput();

            var keyboard = inputContext.Keyboards.FirstOrDefault();
            var mouse = inputContext.Mice.FirstOrDefault();

            keyboard.KeyDown += KeyDown;
            keyboard.KeyUp += KeyUp;
            mouse.MouseDown += ButtonDown;
            mouse.MouseUp += ButtonUp;
            mouse.Scroll += Scrolled;
        }

        private void Scrolled(IMouse mouse, ScrollWheel scrollWheel)
        {
            this.scrollWheel += (int)scrollWheel.Y;
        }

        private void ButtonUp(IMouse mouse, SilkButton button)
        {
            if (pressedButtons.Contains(button))
                pressedButtons.Remove(button);
        }

        private void ButtonDown(IMouse mouse, SilkButton button)
        {
            if (!pressedButtons.Contains(button))
                pressedButtons.Add(button);
        }

        private void KeyUp(IKeyboard keyboard, SilkKey key, int arg3)
        {
            if (pressedKeys.Contains(key))
                pressedKeys.Remove(key);
        }

        private void KeyDown(IKeyboard keyboard, SilkKey key, int arg3)
        {
            if (!pressedKeys.Contains(key))
                pressedKeys.Add(key);
        }

        public void Apply(Simulation simulation)
        {
            simulation.BeforeRender += BeforeRender;
            simulation.AfterRender += AfterRender;
        }

        private void BeforeRender()
        {
            RestoreCaptureState();

            var mouse = inputContext.Mice.FirstOrDefault();

            if (mouse != null)
            {
                mouseDelta = ((Vector2)mouse.Position) - mousePosition;
                mousePosition = mouse.Position;
            }
        }

        private void AfterRender()
        {
            scrollWheel = 0;
            lastPressedButtons = new(pressedButtons);
            lastPressedKeys = new(pressedKeys);
        }

        public void Dispose()
        {
        }

        public Vector2 GetMouseDelta()
        {
            return mouseDelta;
        }

        public Vector2 GetMousePosition()
        {
            return mousePosition;
        }

        public int GetScrollWheelDelta()
        {
            if (mouseCaptured)
                return 0;

            return scrollWheel;
        }

        public bool IsKeyDown(Key key)
        {
            if (keyboardCaptured)
                return false;

            return pressedKeys.Contains(ConvertKey(key));
        }

        public bool IsMouseButtonDown(MouseButton key)
        {
            if (mouseCaptured)
                return false;

            return this.pressedButtons.Contains(ConvertButton(key));
        }

        private static SilkKey ConvertKey(Key key)
        {
            if (key >= Key.A && key <= Key.Z)
                return (SilkKey)(key + 64);

            return key switch
            {
                Key.Key0 => SilkKey.Number0,
                Key.Key1 => SilkKey.Number1,
                Key.Key2 => SilkKey.Number2,
                Key.Key3 => SilkKey.Number3,
                Key.Key4 => SilkKey.Number4,
                Key.Key5 => SilkKey.Number5,
                Key.Key6 => SilkKey.Number6,
                Key.Key7 => SilkKey.Number7,
                Key.Key8 => SilkKey.Number8,
                Key.Key9 => SilkKey.Number9,
                Key.Numpad0 => SilkKey.Keypad0,
                Key.Numpad1 => SilkKey.Keypad1,
                Key.Numpad2 => SilkKey.Keypad2,
                Key.Numpad3 => SilkKey.Keypad3,
                Key.Numpad4 => SilkKey.Keypad4,
                Key.Numpad5 => SilkKey.Keypad5,
                Key.Numpad6 => SilkKey.Keypad6,
                Key.Numpad7 => SilkKey.Keypad7,
                Key.Numpad8 => SilkKey.Keypad8,
                Key.Numpad9 => SilkKey.Keypad9,
                Key.F1 => SilkKey.F1,
                Key.F2 => SilkKey.F2,
                Key.F3 => SilkKey.F3,
                Key.F4 => SilkKey.F4,
                Key.F5 => SilkKey.F5,
                Key.F6 => SilkKey.F6,
                Key.F7 => SilkKey.F7,
                Key.F8 => SilkKey.F8,
                Key.F9 => SilkKey.F9,
                Key.F10 => SilkKey.F10,
                Key.F11 => SilkKey.F11,
                Key.F12 => SilkKey.F12,
                Key.Space => SilkKey.Space,
                Key.LAlt => SilkKey.AltLeft,
                Key.LCtrl => SilkKey.ControlLeft,
                Key.LShift => SilkKey.ShiftLeft,
                Key.CapsLock => SilkKey.CapsLock,
                Key.Tab => SilkKey.Tab,
                Key.Tilde => SilkKey.GraveAccent,
                Key.Esc => SilkKey.Escape,
                Key.RAlt => SilkKey.AltRight,
                Key.Menu => SilkKey.Menu,
                Key.RCtrl => SilkKey.ControlRight,
                Key.RShift => SilkKey.ShiftRight,
                Key.Enter => SilkKey.Enter,
                Key.Backspace => SilkKey.Backspace,
                Key.Comma => SilkKey.Comma,
                Key.Period => SilkKey.Period,
                Key.Slash => SilkKey.Slash,
                Key.Semicolon => SilkKey.Semicolon,
                Key.Apostrophe => SilkKey.Apostrophe,
                Key.LBracket => SilkKey.LeftBracket,
                Key.RBracket => SilkKey.RightBracket,
                Key.BackSlash => SilkKey.BackSlash,
                Key.Minus => SilkKey.Minus,
                Key.Plus => SilkKey.Equal,
                Key.UpArrow => SilkKey.Up,
                Key.LeftArrow => SilkKey.Left,
                Key.DownArrow => SilkKey.Down,
                Key.RightArrow => SilkKey.Right,
                Key.Insert => SilkKey.Insert,
                Key.Home => SilkKey.Home,
                Key.PageUp  => SilkKey.PageUp,
                Key.Delete => SilkKey.Delete,
                Key.End => SilkKey.End,
                Key.PageDown => SilkKey.PageDown,
                _ => SilkKey.Unknown,
            };
        }

        private static SilkButton ConvertButton(MouseButton button)
        {
            return button switch
            {
                MouseButton.Left => SilkButton.Left,
                MouseButton.Right => SilkButton.Right,
                MouseButton.Middle => SilkButton.Middle,
                _ => SilkButton.Unknown,
            };
        }

        public bool IsMouseButtonPressed(MouseButton button)
        {
            if (mouseCaptured)
                return false;

            return pressedButtons.Contains(ConvertButton(button)) && !lastPressedButtons.Contains(ConvertButton(button));
        }

        public bool IsMouseButtonReleased(MouseButton button)
        {
            if (mouseCaptured)
                return false;

            return !pressedButtons.Contains(ConvertButton(button)) && lastPressedButtons.Contains(ConvertButton(button));
        }

        public bool IsKeyPressed(Key key)
        {
            if (keyboardCaptured)
                return false;

            return pressedKeys.Contains(ConvertKey(key)) && !lastPressedKeys.Contains(ConvertKey(key));
        }

        public bool IsKeyReleased(Key key)
        {
            if (keyboardCaptured)
                return false;

            return !pressedKeys.Contains(ConvertKey(key)) && lastPressedKeys.Contains(ConvertKey(key));
        }

        public void OverrideCaptureState(bool captured)
        {
            this.keyboardCaptured = this.mouseCaptured = captured;
        }

        public void RestoreCaptureState()
        {
            var io = ImGuiNET.ImGui.GetIO();
            mouseCaptured = io.WantCaptureMouse;
            keyboardCaptured = io.WantCaptureKeyboard;
        }
    }

    private class WindowImGuiBackend : IImGuiNETBackend
    {
        private ImGuiController imguiController;

        public WindowImGuiBackend(IWindow window)
        {
            imguiController = new ImGuiController(window.CreateOpenGL(), window, window.CreateInput());
        }

        public void Dispose()
        {
            
            imguiController.Dispose();
        }

        public void NewFrame()
        {
            imguiController.Update(Time.DeltaTime);
        }

        public void Render()
        {
            imguiController.Render();
        }

        public IntPtr GetTextureID(ISurface surface)
        {
            return IntPtr.Zero;
        }
    }
}