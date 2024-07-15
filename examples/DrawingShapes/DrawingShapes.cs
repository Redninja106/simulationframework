using SimulationFramework;
using SimulationFramework.Desktop;
using SimulationFramework.Drawing;
using SimulationFramework.Drawing.Shaders;
using SimulationFramework.Input;
using System.Numerics;

namespace DrawingShapes;

class DrawingShapesSimulation : Simulation
{
    ITexture logo;


    public override void OnInitialize()
    {
        logo = Graphics.LoadTexture("./logo-512x512.png");
    }
    float f;
    public override void OnRender(ICanvas canvas)
    {
        canvas.Clear(Color.Gray);

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

        var shader = new MyShader();
        shader.myTexture = logo;
        canvas.Fill(shader);
        canvas.Translate(Mouse.Position);
        canvas.DrawRect(0, 0, 512, 512);

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

class MyShader : CanvasShader
{
    public ITexture myTexture;

    public override ColorF GetPixelColor(Vector2 position)
    {
        return myTexture.TextureSample(position * (1f / 512f));
    }
}