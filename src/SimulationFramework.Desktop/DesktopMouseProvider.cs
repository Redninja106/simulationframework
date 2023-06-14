using Silk.NET.Input;
using SimulationFramework.Input;
using SimulationFramework.Messaging;
using System.Numerics;

namespace SimulationFramework.Desktop;
internal class DesktopMouseProvider : IMouseProvider
{
    private readonly IMouse mouse;

    public Vector2 Position { get => mouse.Position; set => mouse.Position = value; }
    public Vector2 DeltaPosition => Mouse.Position - lastMousePosition;
    public int ScrollWheelDelta => (int)(mouse.ScrollWheels.FirstOrDefault().Y - lastScrollWheel);
    public IEnumerable<MouseButton> HeldButtons => heldButtons;
    public IEnumerable<MouseButton> PressedButtons => pressedButtons;
    public IEnumerable<MouseButton> ReleasedButtons => releasedButtons;

    public event MouseButtonEvent ButtonPressed;
    public event MouseButtonEvent ButtonReleased;

    private readonly List<MouseButton> heldButtons = new();
    private readonly List<MouseButton> pressedButtons = new();
    private readonly List<MouseButton> releasedButtons = new();

    private Vector2 lastMousePosition;
    private float lastScrollWheel;

    public DesktopMouseProvider(IMouse mouse)
    {
        this.mouse = mouse;

        mouse.MouseDown += Mouse_MouseDown;
        mouse.MouseUp += Mouse_MouseUp;
    }

    private void Mouse_MouseUp(IMouse arg1, SilkButton arg2)
    {
        var button = ConvertButton(arg2);

        _ = heldButtons.Remove(button);
        releasedButtons.Add(button);

        ButtonReleased?.Invoke(button);
    }

    private void Mouse_MouseDown(IMouse arg1, SilkButton arg2)
    {
        var button = ConvertButton(arg2);

        if (!heldButtons.Contains(button))
            heldButtons.Add(button);

        pressedButtons.Add(button);

        ButtonPressed?.Invoke(button);
    }

    public void Initialize(MessageDispatcher dispatcher)
    {
        dispatcher.Subscribe<BeforeEventsMessage>(m => BeforeEvents());
        dispatcher.Subscribe<AfterRenderMessage>(m => AfterRender());
    }

    void BeforeEvents()
    {
        pressedButtons.Clear();
        releasedButtons.Clear();
    }

    void AfterRender()
    {
        lastScrollWheel = mouse.ScrollWheels.FirstOrDefault().Y;
        lastMousePosition = mouse.Position;
    }

    public void Dispose()
    {
        throw new NotImplementedException();
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
}