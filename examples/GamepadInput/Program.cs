using Silk.NET.Input;
using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.Input;
using System.Numerics;

Start<Program>();

partial class Program : Simulation
{
    Color backgroundColor = Color.FromHSV(0, 0, .05f);
    Color foregroundColor = Color.FromHSV(0, 0, .75f);

    public override void OnInitialize()
    {
    }

    public override void OnRender(ICanvas canvas)
    {
        canvas.Clear(backgroundColor);
        canvas.Translate(canvas.Width / 2f, canvas.Height / 2f);

        canvas.PushState();
        canvas.Translate(100, 0);
        DrawJoystick(canvas, Gamepad.RightJoystick, Gamepad.IsButtonDown(GamepadButton.RightStick));
        canvas.PopState();

        canvas.PushState();
        canvas.Translate(-100, 0);
        DrawJoystick(canvas, Gamepad.LeftJoystick, Gamepad.IsButtonDown(GamepadButton.LeftStick));
        canvas.PopState();

        canvas.PushState();
        canvas.Translate(100, -200);
        DrawTrigger(canvas, Gamepad.RightTrigger);
        DrawButton(canvas, new(0, 30), Gamepad.IsButtonDown(GamepadButton.RightBumper));
        canvas.PopState();

        canvas.PushState();
        canvas.Translate(-100, -200);
        DrawTrigger(canvas, Gamepad.LeftTrigger);
        DrawButton(canvas, new(0, 30), Gamepad.IsButtonDown(GamepadButton.LeftBumper));
        canvas.PopState();

        canvas.PushState();
        canvas.Translate(300, 0);
        DrawButton(canvas, new(0, 40), Gamepad.IsButtonDown(GamepadButton.A));
        DrawButton(canvas, new(40, 0), Gamepad.IsButtonDown(GamepadButton.B));
        DrawButton(canvas, new(-40, 0), Gamepad.IsButtonDown(GamepadButton.X));
        DrawButton(canvas, new(0, -40), Gamepad.IsButtonDown(GamepadButton.Y));
        canvas.PopState();

        canvas.PushState();
        canvas.Translate(-300, 0);
        DrawButton(canvas, new(0, 40), Gamepad.IsButtonDown(GamepadButton.Down));
        DrawButton(canvas, new(40, 0), Gamepad.IsButtonDown(GamepadButton.Right));
        DrawButton(canvas, new(-40, 0), Gamepad.IsButtonDown(GamepadButton.Left));
        DrawButton(canvas, new(0, -40), Gamepad.IsButtonDown(GamepadButton.Up));
        canvas.PopState();

        canvas.PushState();
        canvas.Translate(0, 200);
        DrawButton(canvas, new(60, 0), Gamepad.IsButtonDown(GamepadButton.Back));
        DrawButton(canvas, new(0, 0), Gamepad.IsButtonDown(GamepadButton.Home));
        DrawButton(canvas, new(-60, 0), Gamepad.IsButtonDown(GamepadButton.Start));
        canvas.PopState();

    }

    private void DrawJoystick(ICanvas canvas, Vector2 joystickPosition, bool down)
    {
        canvas.Fill(foregroundColor);
        canvas.DrawCircle(0, 0, 75);

        canvas.Fill(Color.Red);
        canvas.DrawLine(Vector2.Zero, joystickPosition * 75);

        if (down)
        {
            canvas.DrawCircle(0, 0, 70);
        }
    }

    private void DrawTrigger(ICanvas canvas, float position)
    {
        canvas.Fill(foregroundColor);
        canvas.DrawRect(0, 0, 20, 100, Alignment.BottomCenter);
        canvas.Fill(Color.Red);
        canvas.DrawRect(0, 0, 20, 100 * position, Alignment.BottomCenter);
    }

    private void DrawButton(ICanvas canvas, Vector2 offset, bool down)
    {
        canvas.PushState();
        canvas.Translate(offset);
        canvas.Fill(foregroundColor);
        canvas.DrawCircle(0, 0, 20);
        if (down)
        {
            canvas.Fill(Color.Red);
            canvas.DrawCircle(0, 0, 16);
        }
        canvas.PopState();
    }
}
