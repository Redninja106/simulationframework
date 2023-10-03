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

    float c;
    Vector2 p;

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

        c += Mouse.ScrollWheelDelta;

        canvas.Clear(Color.Gray);
        canvas.Fill(Color.Red);
        p += Mouse.DeltaPosition;
        canvas.DrawCircle(p, 10, Alignment.Center);
        canvas.DrawText(c.ToString(), Vector2.One * 20);

        if (Keyboard.IsKeyPressed(Key.Q))
        {
            Mouse.Visible = !Mouse.Visible;
        }

        if (Keyboard.IsKeyPressed(Key.W))
        {
            Mouse.SetCursor((SystemCursor)(c % 7));
        }

        if (Keyboard.IsKeyPressed(Key.E))
        {
            Color[,] colors = new Color[32, 32];

            for (int y = 0; y < colors.GetLength(1); y++)
            {
                for (int x = 0; x < colors.GetLength(0); x++)
                {
                    colors[x, y] = Color.Blue;
                }
            }

            Mouse.SetCursor(colors, Alignment.CenterLeft);
        }

        if (Keyboard.IsKeyPressed(Key.R))
        {
            if (!Window.IsMaximized)
            {
                Window.Maximize();
            }
            else
            {
                Window.Restore();
            }
        }

        if (Keyboard.IsKeyPressed(Key.T))
        {
            if (!Window.IsMinimized)
            {
                Window.Minimize();
            }
            else
            {
                Window.Restore();
            }
        }

        if (Keyboard.IsKeyPressed(Key.F))
        {
            if (Window.IsFullscreen)
            {
                Window.ExitFullscreen();
            }
            else
            {
                Window.EnterFullscreen();
            }
        }
    }
}