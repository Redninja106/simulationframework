using SimulationFramework;
using SimulationFramework.Components;
using SimulationFramework.Drawing;
using SimulationFramework.Input;
using System.Numerics;
using System.Reflection;

namespace BasicInput;

internal class BasicInputSimulation : Simulation
{
    public override void OnInitialize()
    {
    }

    int c;
    Vector2 lastDeltaPosition;
    public override void OnRender(ICanvas canvas)
    {
        if (Mouse.IsButtonPressed(MouseButton.Left))
        {
            c++;
        }

        if (Keyboard.IsKeyReleased(Key.Space))
        {
            c++;
        }

        canvas.Clear(Color.Gray);
        canvas.Fill(Color.Red);
        canvas.DrawCircle(Mouse.Position, 10, Alignment.Center);
        canvas.DrawText(c.ToString(), Vector2.One * 20);
    }
}