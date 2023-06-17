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


        if (Keyboard.IsKeyPressed(Key.W))
            Window.Resize(Window.Size - new Vector2(0, 5));
        if (Keyboard.IsKeyPressed(Key.S))
            Window.Resize(Window.Size + new Vector2(0, 5));
        if (Keyboard.IsKeyPressed(Key.A))
            Window.Resize(Window.Size - new Vector2(5, 0));
        if (Keyboard.IsKeyPressed(Key.D))
            Window.Resize(Window.Size + new Vector2(5, 0));
    }
}