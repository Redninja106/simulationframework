using Silk.NET.GLFW;
using Silk.NET.Input;
using SimulationFramework.Input;
using SimulationFramework.Messaging;
using System.Buffers;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace SimulationFramework.Desktop;
internal class DesktopMouseProvider : IMouseProvider
{
    private readonly IMouse mouse;

    public Vector2 Position { get => mouse.Position; set => mouse.Position = value; }
    public Vector2 DeltaPosition => mousePosition - lastMousePosition;
    public float ScrollWheelDelta => scrollWheel;
    public IEnumerable<MouseButton> HeldButtons => heldButtons;
    public IEnumerable<MouseButton> PressedButtons => pressedButtons;
    public IEnumerable<MouseButton> ReleasedButtons => releasedButtons;

    public bool Visible
    {
        get => mouse.Cursor.CursorMode == CursorMode.Normal;
        set => mouse.Cursor.CursorMode = value ? CursorMode.Normal : CursorMode.Hidden;
    }

    public event MouseButtonEvent ButtonPressed;
    public event MouseButtonEvent ButtonReleased;

    private readonly List<MouseButton> heldButtons = new();
    private readonly List<MouseButton> pressedButtons = new();
    private readonly List<MouseButton> releasedButtons = new();

    private Vector2 mousePosition;
    private Vector2 lastMousePosition;

    private float scrollWheel;
    internal bool capturedByImgui;

    public DesktopMouseProvider(IMouse mouse)
    {
        this.mouse = mouse;

        mouse.MouseDown += Mouse_MouseDown;
        mouse.MouseUp += Mouse_MouseUp;
    }

    private void Mouse_MouseUp(IMouse arg1, SilkButton arg2)
    {
        if (capturedByImgui)
            return;

        var button = ConvertButton(arg2);

        _ = heldButtons.Remove(button);
        releasedButtons.Add(button);

        ButtonReleased?.Invoke(button);
    }

    private void Mouse_MouseDown(IMouse arg1, SilkButton arg2)
    {
        if (capturedByImgui)
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
        dispatcher.Subscribe<AfterRenderMessage>(m => AfterRender());
    }

    void BeforeEvents()
    {
        pressedButtons.Clear();
        releasedButtons.Clear();
    }

    void AfterEvents()
    {
        lastMousePosition = mousePosition;
        mousePosition = mouse.Position;

        scrollWheel = capturedByImgui ? 0 : mouse.ScrollWheels.FirstOrDefault().Y;
    }

    void AfterRender()
    {
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

    public unsafe void SetCursor(int width, int height, ReadOnlySpan<Color> colors, int centerX, int centerY)
    {
        fixed (Color* colorsPtr = &colors[0])
        {
            var memoryManager = new UnsafePinnedMemoryManager<byte>((byte*)colorsPtr, sizeof(Color) * width * height);
            this.mouse.Cursor.Image = new(width, height, memoryManager.Memory);
        }

        this.mouse.Cursor.HotspotX = centerX;
        this.mouse.Cursor.HotspotY = centerY;

        this.mouse.Cursor.Type = CursorType.Custom;
    }

    public void SetCursor(SystemCursor cursor)
    {
        this.mouse.Cursor.StandardCursor = cursor switch
        {
            SystemCursor.Default => StandardCursor.Default,
            SystemCursor.Arrow => StandardCursor.Arrow,
            SystemCursor.IBeam => StandardCursor.IBeam,
            SystemCursor.Crosshair => StandardCursor.Crosshair,
            SystemCursor.Hand => StandardCursor.Hand,
            SystemCursor.HorizontalResize => StandardCursor.HResize,
            SystemCursor.VerticalResize => StandardCursor.VResize,
            _ => throw new ArgumentException(null, nameof(cursor))
        };

        this.mouse.Cursor.Type = CursorType.Standard;
    }

    private unsafe class UnsafePinnedMemoryManager<T> : MemoryManager<T> where T : unmanaged
    {
        private readonly T* pointer;
        private readonly int length;

        public UnsafePinnedMemoryManager(T* pointer, int length)
        {
            this.pointer = pointer;
            this.length = length;
        }

        public override Span<T> GetSpan()
        {
            return new(pointer, length);
        }

        public override MemoryHandle Pin(int elementIndex = 0)
        {
            return new MemoryHandle(pointer);
        }

        public override void Unpin()
        {
        }

        protected override void Dispose(bool disposing)
        {
        }
    }
}