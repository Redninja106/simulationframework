using SimulationFramework;
using SimulationFramework.Desktop;
using SimulationFramework.Drawing;
using SimulationFramework.Drawing.Shaders;
using SimulationFramework.Input;
using SimulationFramework.OpenGL;
using System.Numerics;

namespace DrawingShapes;

class DrawingShapesSimulation : Simulation
{
    ITexture logo;

    public override void OnInitialize()
    {
        logo = Graphics.LoadTexture("./logo-512x512.png");
        SetFixedResolution(640, 480, Color.Black);
    }
    float f;
    string s = "Hello!";
    public override void OnRender(ICanvas canvas)
    {
        Window.Title = "Simulation - " + (int)Performance.Framerate + " FPS";
        canvas.Clear(Color.Gray);
        //canvas.Scale(MathF.Pow(1.1f, f));
        f += Mouse.ScrollWheelDelta;
        foreach (var typedChar in Keyboard.TypedKeys)
        {
            s += typedChar;
        }

        if (Keyboard.IsKeyPressed(Key.Backspace))
        {
            if (s.Length > 0)
                s = s[..(s.Length-1)];
        }

        canvas.Antialias(true);

        canvas.DrawAlignedText(s, 32f, Mouse.Position + Vector2.One * 10, Alignment.Center, Keyboard.IsKeyDown(Key.Space) ? TextStyle.Bold : 0);
        canvas.DrawCircle(Mouse.Position, MathF.Pow(1.1f, f));

        // ImGuiNET.ImGui.Text("hello, world!");
        // ImGuiNET.ImGui.Image(logo.GetImGuiID(), new(100, 100));
        // 
        // canvas.PushState();
        // canvas.Translate(Mouse.Position);
        // canvas.DrawTexture(logo);
        // canvas.PopState();
        // f += Mouse.ScrollWheelDelta;
        // canvas.DrawPolygon(new[]
        // {
        //      new Vector2(500, f),
        //      new Vector2(10, 10),
        //      new Vector2(10, 500),
        //      Mouse.Position,
        //      new Vector2(500, 10),
        //  });
        // 
        // canvas.Fill(Color.Red);
        // canvas.DrawRect(100, 100, 100, 100);
        // 
        // //canvas.Fill(new LinearGradient(300, 100, 400, 200, Color.Blue, Color.Purple, Color.Red));
        // //canvas.DrawRect(300, 100, 100, 100);
        // 
        // canvas.Stroke(Color.Yellow);
        // canvas.DrawRect(500, 100, 100, 100);
        // 
        // canvas.Stroke(Color.Green);
        // canvas.StrokeWidth(10);
        // canvas.DrawLine(500, 500, 1000, 1000);
        // canvas.DrawLine(100, 100, 500, 500);

        // canvas.Font("Verdana");
        // canvas.Fill(Color.Purple);
        //
        // float textX = canvas.Width / 2, textY = canvas.Height / 2;
        //
        // canvas.FontSize(72);
        //
        // canvas.FontStyle(FontStyle.Normal);
        // canvas.DrawText("Hello, World!", textX, textY, Alignment.TopRight);
        // 
        // canvas.FontStyle(FontStyle.Bold | FontStyle.Italic);
        // canvas.DrawText("Hello, World!", textX, textY, Alignment.TopLeft);
        // 
        // canvas.FontStyle(FontStyle.Bold | FontStyle.Strikethrough | FontStyle.Italic | FontStyle.Underline);
        // canvas.DrawText("Hello, World!", textX, textY, Alignment.BottomRight);
        // 
        // canvas.FontStyle(FontStyle.Strikethrough | FontStyle.Underline);
        // canvas.DrawText("Hello, World!", textX, textY, Alignment.BottomLeft);
    }
}
