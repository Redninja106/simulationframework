using SimulationFramework;
using SimulationFramework.Desktop;
using SimulationFramework.Drawing;
using SimulationFramework.Drawing.Shaders.Compiler;
using SimulationFramework.Input;
using SimulationFramework.OpenGL;
using System.Numerics;

Start<Program>(new DesktopPlatform());

partial class Program : Simulation
{
    ITexture logo;

    public override void OnInitialize()
    {
        logo = Graphics.LoadTexture("./logo-512x512.png");
    }
    public override void OnRender(ICanvas canvas)
    {
        Window.Title = $"Simulation - {(int)Performance.Framerate} FPS";

        canvas.Clear(Color.FromHSV(.1f, .1f, .1f));

        canvas.Translate(100, 100);
        canvas.DrawTexture(logo, new Rectangle(0, 0, 100, 100, Alignment.Center));

        canvas.Translate(150, 0);
        canvas.DrawPolygon(new[]
        {
             50 * Angle.ToVector(.0f * MathF.Tau),
             50 * Angle.ToVector(.2f * MathF.Tau),
             50 * Angle.ToVector(.4f * MathF.Tau),
             50 * Angle.ToVector(.6f * MathF.Tau),
             50 * Angle.ToVector(.8f * MathF.Tau),
        });

        canvas.Translate(150, 0);
        canvas.Fill(Color.Red);
        canvas.DrawRect(0, 0, 100, 100, Alignment.Center);

        canvas.Translate(150, 0);
        canvas.Stroke(Color.Green);
        canvas.StrokeWidth(10);
        canvas.DrawRect(0, 0, 90, 90, Alignment.Center);

        canvas.Fill(new LinearGradient(-50, -50, 50, 50, ColorF.Blue, ColorF.Purple, ColorF.Green, ColorF.Orange)
        {
            TileMode = TileMode.Clamp,
        });
        canvas.DrawRect(0, 0, 100, 100, Alignment.Center);

        canvas.Translate(150, 0);
        canvas.Fill(new RadialGradient(0, 0, 50, ColorF.Orange, ColorF.Green)
        {
            TileMode = TileMode.Clamp
        });
        canvas.DrawRect(0, 0, 100, 100, Alignment.Center);

        canvas.Translate(150, 0);
        canvas.Stroke(Color.Yellow);
        canvas.DrawRect(500, 100, 100, 100, Alignment.Center);

        canvas.ResetState();
        canvas.Translate(100, 300);

        canvas.Stroke(Color.Green);
        canvas.StrokeWidth(10);
        canvas.DrawLine(-50, -50, 50, 50);
        canvas.DrawLine(50, -50, -50, 50);

        canvas.Translate(150, 0);
        canvas.Fill(Color.Aquamarine);
        canvas.DrawRoundedRect(0, 0, 100, 100, 25, Alignment.Center);

        canvas.Translate(150, 0);
        canvas.Fill(Color.Beige);
        canvas.DrawArc(0, 0, 50, 50, MathF.PI / 3f, 5 * MathF.PI / 3f, true, Alignment.Center);

        canvas.Translate(150, 0);
        canvas.Fill(Color.DarkOliveGreen);
        canvas.DrawCircle(0, 0, 50);

        canvas.Translate(150, 0);
        canvas.Font("Verdana");
        canvas.Fill(Color.Purple);
        canvas.DrawText("Hello, World!", 72, 0, 0, TextStyle.Regular);
    }
}