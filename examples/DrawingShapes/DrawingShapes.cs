using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.Drawing.Shaders;
using SimulationFramework.Input;
using SkiaSharp;
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

        //ImGuiNET.ImGui.Text("hello, world!");

        canvas.Fill(Color.Orange);
        f += Mouse.ScrollWheelDelta;
        // canvas.DrawPolygon(new[]
        // {
        //     new Vector2(500, f),
        //     new Vector2(10, 10),
        //     new Vector2(10, 500),
        //     Mouse.Position,
        //     new Vector2(500, 10),
        // });

        //canvas.Fill(Color.Red);
        //canvas.DrawRect(100, 100, 100, 100);

        //canvas.Fill(new LinearGradient(300, 100, 400, 200, Color.Blue, Color.Purple, Color.Red));
        //canvas.DrawRect(300, 100, 100, 100);

        //canvas.Stroke(Color.Yellow);
        //canvas.DrawRect(500, 100, 100, 100);

        //canvas.Stroke(Color.Green);

        //canvas.StrokeWidth(10);
        //canvas.DrawLine(500, 500, 1000, 1000);

        //canvas.DrawLine(100, 100, 500, 500);

        // var shader = new MyShader(1f);
        // canvas.Fill(shader);
        // shader.brightness = MathF.Sin(f / 10f) * .5f + .5f;
        // canvas.DrawRect(Mouse.Position, new(500, 500), Alignment.Center);

        {
            canvas.PushState();
            canvas.Translate(Mouse.Position);
            canvas.DrawTexture(logo);
            canvas.PopState();
        }
        
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
    public float brightness;

    public MyShader(float brightness)
    {
        this.brightness = brightness;
    }

    public override ColorF GetPixelColor(Vector2 position)
    {
        return brightness * new ColorF(position.X / 500f, position.Y / 500f, 0, 1);
    }
}