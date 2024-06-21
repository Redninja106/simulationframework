using Silk.NET.OpenGL;
using SimulationFramework;
using SimulationFramework.Desktop;
using SimulationFramework.Drawing;
using SimulationFramework.Drawing.Shaders;
using SimulationFramework.SkiaSharp;
using SkiaSharp;
using System.Numerics;

Start<Program>(new DesktopPlatform());

partial class Program : Simulation
{
    public override unsafe void OnInitialize()
    {
    }

    MyShader shader = new MyShader()
    {
        data = new(1, 2, 4, 5),
    };

    public override void OnRender(ICanvas canvas)
    {
        canvas.Clear(Color.Black);
        canvas.Fill(shader);
        canvas.Translate(canvas.Width / 2f, canvas.Height / 2f);
        canvas.DrawRect(0, 0, 100, 100, Alignment.Center);
    }
}

class MyShader : CanvasShader
{
    public Vector4 data;

    public override ColorF GetPixelColor(Vector2 position)
    {
        return ColorF.Red;
    }
}