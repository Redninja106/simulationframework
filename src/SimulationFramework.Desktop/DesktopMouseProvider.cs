using Silk.NET.GLFW;
using Silk.NET.Input;
using SimulationFramework.Input;
using SimulationFramework.Messaging;
using System.Numerics;

namespace SimulationFramework.Desktop;
internal unsafe class DesktopMouseProvider : IMouseProvider
{
    private readonly IMouse mouse;

    public Vector2 Position { get => mouse.Position; set => mouse.Position = value; }
    public Vector2 DeltaPosition => mousePosition - lastMousePosition!.Value;
    public float ScrollWheelDelta => scrollWheel;
    public IEnumerable<MouseButton> HeldButtons => heldButtons;
    public IEnumerable<MouseButton> PressedButtons => pressedButtons;
    public IEnumerable<MouseButton> ReleasedButtons => releasedButtons;

    public bool Visible
    {
        get => mouse.Cursor.CursorMode == CursorMode.Normal;
        set => mouse.Cursor.CursorMode = value ? CursorMode.Normal : CursorMode.Hidden;
    }

    public event MouseButtonEvent? ButtonPressed;
    public event MouseButtonEvent? ButtonReleased;

    private readonly List<MouseButton> heldButtons = new();
    private readonly List<MouseButton> pressedButtons = new();
    private readonly List<MouseButton> releasedButtons = new();

    private Vector2 mousePosition;
    private Vector2? lastMousePosition;

    private Glfw glfw = Glfw.GetApi();
    private unsafe Cursor* glfwCursor;
    private unsafe WindowHandle* glfwWindow;

    private float scrollWheel;
    internal bool CapturedByImgui;

    public DesktopMouseProvider(IMouse mouse, WindowHandle* window)
    {
        this.glfwWindow = window;
        this.mouse = mouse;

        mouse.MouseDown += Mouse_MouseDown;
        mouse.MouseUp += Mouse_MouseUp;
    }

    private void Mouse_MouseUp(IMouse arg1, SilkButton arg2)
    {
        if (CapturedByImgui)
            return;

        var button = ConvertButton(arg2);

        _ = heldButtons.Remove(button);
        releasedButtons.Add(button);

        ButtonReleased?.Invoke(button);
    }

    private void Mouse_MouseDown(IMouse arg1, SilkButton arg2)
    {
        if (CapturedByImgui)
            return;

        var button = ConvertButton(arg2);

        if (!heldButtons.Contains(button))
            heldButtons.Add(button);

        pressedButtons.Add(button);

        ButtonPressed?.Invoke(button);
    }

    public void Initialize(MessageDispatcher dispatcher)
    {
        dispatcher.Subscribe<BeforeEventsMessage>(m => BeforeEvents());
        dispatcher.Subscribe<AfterEventsMessage>(m => AfterEvents());
    }

    void BeforeEvents()
    {
        pressedButtons.Clear();
        releasedButtons.Clear();
    }

    void AfterEvents()
    {
        if (lastMousePosition is not null)
        {
            lastMousePosition = mousePosition;
        }
        else
        {
            lastMousePosition = mouse.Position;
        }

        mousePosition = mouse.Position;

        scrollWheel = GetScrollWheelValue();
    }

    private float GetScrollWheelValue()
    {
        if (CapturedByImgui)
            return 0;

        if (mouse.ScrollWheels.Count > 0)
            return mouse.ScrollWheels[0].Y;

        return 0;
    }

    public void Dispose()
    {
    }

    private static MouseButton ConvertButton(SilkButton button)
    {
        return button switch
        {
            SilkButton.Left => MouseButton.Left,
            SilkButton.Right => MouseButton.Right,
            SilkButton.Middle => MouseButton.Middle,
            SilkButton.Button4 => MouseButton.X1,
            SilkButton.Button5 => MouseButton.X2,
            _ => throw new Exception(),
        };
    }

    public unsafe void SetCursor(ReadOnlySpan<Color> colors, int width, int height, int centerX, int centerY)
    {
        Cursor* oldCursor = glfwCursor;

        fixed (Color* colorsPtr = colors)
        {
            Image image = new()
            {
                Width = width,
                Height = height,
                Pixels = (byte*)colorsPtr,
            };

            UpdateCursor(glfw.CreateCursor(&image, centerX, centerY));
        }
    }

    public void SetCursor(SystemCursor cursor)
    {
        CursorShape shape = cursor switch
        {
            SystemCursor.Default => CursorShape.Arrow,
            SystemCursor.Arrow => CursorShape.Arrow,
            SystemCursor.IBeam => CursorShape.IBeam,
            SystemCursor.Crosshair => CursorShape.Crosshair,
            SystemCursor.Hand => CursorShape.Hand,
            SystemCursor.HorizontalResize => CursorShape.HResize,
            SystemCursor.VerticalResize => CursorShape.VResize,
            _ => throw new ArgumentException(null, nameof(cursor))
        };

        UpdateCursor(glfw.CreateStandardCursor(shape));
    }

    private void UpdateCursor(Cursor* cursor)
    {
        var oldCursor = glfwCursor;
        glfwCursor = cursor;
        glfw.SetCursor(glfwWindow, cursor);
        if (oldCursor != null)
        {
            glfw.DestroyCursor(oldCursor);
        }
    }
}