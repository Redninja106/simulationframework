using Silk.NET.Input;
using Silk.NET.Input.Extensions;
using SimulationFramework.Input;
using SimulationFramework.Messaging;
using System.Numerics;

namespace SimulationFramework.Desktop;
internal class DesktopGamepadProvider : IGamepadProvider
{
    private readonly IGamepad gamepad;

    public Vector2 LeftJoystick => new(gamepad.Thumbsticks[0].X, gamepad.Thumbsticks[0].X);
    public Vector2 RightJoystick => new(gamepad.Thumbsticks[1].X, gamepad.Thumbsticks[1].X);
    public float LeftTrigger => gamepad.Triggers[0].Position;
    public float RightTrigger => gamepad.Triggers[1].Position;
    public IEnumerable<GamepadButton> HeldButtons => heldButtons;
    public IEnumerable<GamepadButton> PressedButtons => pressedButtons;
    public IEnumerable<GamepadButton> ReleasedButtons => releasedButtons;
    public float VibrationStrength 
    { 
        get => vibrationStrength;
        set
        {
            vibrationStrength = value;
            foreach (var motor in gamepad.VibrationMotors)
            {
                motor.Speed = vibrationStrength;
            }
        }
    }

    public event GamepadButtonEvent ButtonPressed;
    public event GamepadButtonEvent ButtonReleased;

    private readonly List<GamepadButton> heldButtons = new();
    private readonly List<GamepadButton> pressedButtons = new();
    private readonly List<GamepadButton> releasedButtons = new();
    private float vibrationStrength;

    public DesktopGamepadProvider(IGamepad gamepad)
    {
        this.gamepad = gamepad;

        gamepad.ButtonDown += Gamepad_ButtonDown;
        gamepad.ButtonUp += Gamepad_ButtonUp;
    }

    private void Gamepad_ButtonUp(IGamepad arg1, Button arg2)
    {
        var button = ConvertButton(arg2.Name);

        heldButtons.Remove(button);
        releasedButtons.Add(button);

        ButtonReleased(button);
    }

    private void Gamepad_ButtonDown(IGamepad arg1, Button arg2)
    {
        var button = ConvertButton(arg2.Name);

        if (!heldButtons.Contains(button))
            heldButtons.Add(button);
        
        pressedButtons.Add(button);

        ButtonPressed(button);
    }

    public void Initialize(MessageDispatcher dispatcher)
    {
        dispatcher.Subscribe<BeforeEventsMessage>(m =>
        {
            pressedButtons.Clear();
            releasedButtons.Clear();
        });
    }

    public void Dispose()
    {
    }

    private GamepadButton ConvertButton(ButtonName button) => button switch
    {
        ButtonName.A => GamepadButton.A,
        ButtonName.B => GamepadButton.B,
        ButtonName.X => GamepadButton.X,
        ButtonName.Y => GamepadButton.Y,
        ButtonName.LeftBumper => GamepadButton.LeftBumper,
        ButtonName.RightBumper => GamepadButton.RightBumper,
        ButtonName.Back => GamepadButton.Back,
        ButtonName.Start => GamepadButton.Start,
        ButtonName.Home => GamepadButton.Home,
        ButtonName.LeftStick => GamepadButton.LeftStick,
        ButtonName.RightStick => GamepadButton.RightStick,
        ButtonName.DPadUp => GamepadButton.Up,
        ButtonName.DPadRight => GamepadButton.Right,
        ButtonName.DPadDown => GamepadButton.Down,
        ButtonName.DPadLeft => GamepadButton.Left,
        _ => throw new Exception("Unrecognized gamepad button"),
    };
}