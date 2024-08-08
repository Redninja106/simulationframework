using SimulationFramework;
using SimulationFramework.Desktop;
using SimulationFramework.Drawing;
using SimulationFramework.Input;
using SimulationFramework.OpenGL;
using System.Numerics;

Start<Program>(new DesktopPlatform());

partial class Program : Simulation
{
    ITexture logo;
    string typedString = "Hello!";

    public override void OnInitialize()
    {
        logo = Graphics.LoadTexture("./logo-512x512.png");
        GLGraphicsProvider.DumpShaders = true;
        // SetFixedResolution(640, 480, Color.Black);
    }
    public override void OnRender(ICanvas canvas)
    {
        Window.Title = "Simulation - " + (int)Performance.Framerate + " FPS";
        canvas.Clear(Color.FromHSV(.1f, .1f, .1f));

        // foreach (var typedChar in Keyboard.TypedKeys)
        // {
        //     typedString += typedChar;
        // }
        // 
        // if (Keyboard.IsKeyPressed(Key.Backspace))
        // {
        //     if (typedString.Length > 0)
        //         typedString = typedString[..(typedString.Length - 1)];
        // }

        // canvas.Antialias(true);
        // canvas.DrawAlignedText(typedString, 32f, Mouse.Position + Vector2.One * 10, Alignment.Center, Keyboard.IsKeyDown(Key.Space) ? TextStyle.Bold : 0);

        // ImGuiNET.ImGui.Text("hello, world!");
        // ImGuiNET.ImGui.Image(logo.GetImGuiID(), new(100, 100));

        canvas.Translate(100, 100);
        canvas.DrawTexture(logo);

        canvas.Translate(150, 0);
        canvas.DrawPolygon(new[]
        {
             50 * Angle.ToVector(.0f * MathF.Tau),
             50 * Angle.ToVector(.2f * MathF.Tau),
             50 * Angle.ToVector(.4f * MathF.Tau),
             50 * Angle.ToVector(.6f * MathF.Tau),
             50 * Angle.ToVector(.8f * MathF.Tau),
        });

        // canvas.Translate(150, 0);
        // canvas.Fill(Color.Red);
        // canvas.DrawRect(0, 0, 100, 100, Alignment.Center);
        // 
        // canvas.Translate(150, 0);
        // canvas.Stroke(Color.Green);
        // canvas.StrokeWidth(10);
        // canvas.DrawRect(0, 0, 90, 90, Alignment.Center);

        canvas.ResetState();
        canvas.Translate(Mouse.Position);
        var grad = new LinearGradient(-50, -50, 50, 50, ColorF.Blue, ColorF.Purple, ColorF.Green, ColorF.Orange);
        // grad.GetPixelColor(new Vector2(50, 50));
        canvas.Fill(grad);
        canvas.DrawRect(0, 0, 100, 100, Alignment.Center);

        canvas.Translate(150, 0);
        canvas.Fill(new RadialGradient(0, 0, 50, ColorF.Orange, ColorF.Green));
        canvas.DrawRect(0, 0, 100, 100, Alignment.Center);

        // canvas.Translate(150, 0);
        // canvas.Stroke(Color.Yellow);
        // canvas.DrawRect(500, 100, 100, 100, Alignment.Center);

        // canvas.Stroke(Color.Green);
        // canvas.StrokeWidth(10);
        // canvas.DrawLine(100, 100, 500, 500);

        canvas.Font("Verdana");
        canvas.Fill(Color.Purple);
        float textX = canvas.Width / 2, textY = canvas.Height / 2;
        canvas.DrawText("Hello, World!", 72, textX, textY, TextStyle.Regular);
    }
}