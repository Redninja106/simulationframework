using ImGuiNET;
using SimulationFramework;
using SimulationFramework.Desktop;
using SimulationFramework.Drawing;
using SimulationFramework.Drawing.Shaders;
using SimulationFramework.Input;
using System.Diagnostics;
using System.Numerics;
using System.Reflection;

Start<Program>(new DesktopPlatform());

partial class Program : Simulation
{
    public override unsafe void OnInitialize()
    {
        SetFixedResolution(320, 180, Color.Gray, subpixelInput: true);
    }
    float scale = 32;
    string s = "";
    public override void OnRender(ICanvas canvas)
    {
        canvas.Clear(Color.FromHSV(0, 0, .15f));
        scale += Mouse.ScrollWheelDelta;

        ImGui.SliderFloat("scale1", ref scale, 8, 64);

        if (Keyboard.IsKeyPressed(Key.Backspace) && s.Length > 0)
        {
            s = s[..^1];
        }
        
        foreach (var typedKey in Keyboard.TypedKeys)
        {
            s += typedKey;
        }

        canvas.FontSize(scale);
        canvas.DrawText(s, Mouse.Position);
        canvas.Stroke(Color.Red);
        canvas.Translate(Mouse.Position);
        canvas.DrawRect(canvas.State.font.MeasureText(s, canvas.State.FontSize, canvas.State.FontStyle));
        //canvas.DrawRect(Mouse.Position, Vector2.One * 100);
    }
}

class MyShader : CanvasShader
{
    public Vector4[] circles;
    public void Update()
    {
        for (int i = 0; i < circles.Length; i++)
        {
            circles[i].Z += Time.DeltaTime * 100;
        }

        if (Mouse.IsButtonPressed(MouseButton.Left))
        {
            circles = [.. circles, new(Mouse.Position.X, Mouse.Position.Y, 0, 0)];
        }
    }

    public override ColorF GetPixelColor(Vector2 position)
    {
        float minDist = 100000;
        for (int i = 0; i < circles.Length; i++)
        {
            Circle c = default;
            c.Position = new(circles[i].X, circles[i].Y);
            c.Radius = circles[i].Z;
            float d = MathF.Abs(c.SignedDistance(position));
            if (d < minDist)
            {
                minDist = d;
            }
        }
        return new(1 - (minDist / 10f)*(minDist / 10f), 0, 0);
    }
}