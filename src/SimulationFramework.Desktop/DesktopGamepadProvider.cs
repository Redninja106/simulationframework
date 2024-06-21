using Silk.NET.Input;
using SimulationFramework.Input;
using SimulationFramework.Messaging;
using System.Numerics;

namespace SimulationFramework.Desktop;
internal class DesktopGamepadProvider : IGamepadProvider
{
    private IGamepad? gamepad;

    public Vector2 LeftJoystick => gamepad is null ? Vector2.Zero : new(gamepad.Thumbsticks[0].X, gamepad.Thumbsticks[0].Y);
    public Vector2 RightJoystick => gamepad is null ? Vector2.Zero : new(gamepad.Thumbsticks[1].X, gamepad.Thumbsticks[1].Y);
    public float LeftTrigger => gamepad is null ? 0 : gamepad.Triggers[0].Position * .5f + .5f;
    public float RightTrigger => gamepad is null ? 0 : gamepad.Triggers[1].Position * .5f + .5f;
    public IEnumerable<GamepadButton> HeldButtons => heldButtons;
    public IEnumerable<GamepadButton> PressedButtons => pressedButtons;
    public IEnumerable<GamepadButton> ReleasedButtons => releasedButtons;
    public float VibrationStrength 
    { 
        get => vibrationStrength;
        set
        {
            if (gamepad is not null)
            {
                vibrationStrength = value;
                foreach (var motor in gamepad.VibrationMotors)
                {
                    motor.Speed = vibrationStrength;
                }
            }
        }
    }

    public event GamepadButtonEvent? ButtonPressed;
    public event GamepadButtonEvent? ButtonReleased;

    private readonly List<GamepadButton> heldButtons = new();
    private readonly List<GamepadButton> pressedButtons = new();
    private readonly List<GamepadButton> releasedButtons = new();
    private float vibrationStrength;
    private IInputContext context;

    public DesktopGamepadProvider(IInputContext context)
    {
        this.context = context;
        context.ConnectionChanged += Context_ConnectionChanged;

        var gp = context.Gamepads.FirstOrDefault();
        if (gp != null)
        {
            OnGamepadAdded(gp);
        }
    }

    private void Context_ConnectionChanged(IInputDevice device, bool added)
    {
        if (device is IGamepad gamepad)
        {
            if (added)
            {
                OnGamepadAdded(gamepad);
            }
            else
            {
                OnGamepadRemoved(gamepad);
            }
        }
    }

    private void OnGamepadAdded(IGamepad gamepad)
    {
        if (this.gamepad is null)
        {
            this.gamepad = gamepad;
            this.gamepad.ButtonDown += Gamepad_ButtonDown;
            this.gamepad.ButtonUp += Gamepad_ButtonUp;
        }
    }

    private void OnGamepadRemoved(IGamepad gamepad)
    {
        if (gamepad == this.gamepad)
        {
            this.gamepad = null;
            pressedButtons.Clear();
            heldButtons.Clear();
            releasedButtons.Clear();
        }

        if (context.Gamepads.Count() > 0)
        {
            OnGamepadAdded(context.Gamepads.First());
        }
    }

    private void Gamepad_ButtonUp(IGamepad arg1, Button arg2)
    {
        var button = ConvertButton(arg2.Name);

        heldButtons.Remove(button);
        releasedButtons.Add(button);

        ButtonReleased?.Invoke(button);
    }

    private void Gamepad_ButtonDown(IGamepad arg1, Button arg2)
    {
        var button = ConvertButton(arg2.Name);

        if (!heldButtons.Contains(button))
            heldButtons.Add(button);
        
        pressedButtons.Add(button);

        ButtonPressed?.Invoke(button);
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

    private static GamepadButton ConvertButton(ButtonName button) => button switch
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