using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.Input;
using System.Numerics;

namespace BasicInput;

internal class BasicInputSimulation : Simulation
{
    public override void OnInitialize()
    {
    }

    int c;
    public override void OnRender(ICanvas canvas)
    {
        canvas.Clear(Color.Gray);

        if (Mouse.IsButtonPressed(MouseButton.Left))
        {
            c++;
        }

        if (Keyboard.IsKeyReleased(Key.Space))
        {
            c++;
        }

        canvas.DrawRect(Mouse.Position, Vector2.One * 100, Alignment.Center);
        canvas.DrawText(c.ToString(), Vector2.One * 20);
        Window.ShowSystemMenu = false;
    }
}