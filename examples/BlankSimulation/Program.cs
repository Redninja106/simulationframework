using Silk.NET.OpenGL;
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
    ITexture texture;

    public override unsafe void OnInitialize()
    {
        
    }

    MyShader shader = new MyShader()
    {
        color = ColorF.Red,
    };

    public override void OnRender(ICanvas canvas)
    {
        canvas.Clear(Color.FromHSV(0, 0, .15f));

        shader.color = ColorF.FromHSV(MathF.Sin(Time.TotalTime) * .5f + .5f, 1, 1);

        canvas.Fill(shader);
        canvas.Translate(canvas.Width / 2f, canvas.Height / 2f);
        canvas.DrawRect(0, 0, 250, 250, Alignment.Center);
    }
}

class MyShader : CanvasShader
{
    public ColorF color;

    public override ColorF GetPixelColor(Vector2 position)
    {
        Circle c = default;
        return color;
    }
}