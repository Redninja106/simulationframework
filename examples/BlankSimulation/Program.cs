using ImGuiNET;
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
        circles = [],
    };

    public override void OnRender(ICanvas canvas)
    {
        canvas.Clear(Color.FromHSV(0, 0, .15f));

        // shader.color = ColorF.FromHSV(MathF.Sin(Time.TotalTime) * .5f + .5f, 1, 1);

        if (ImGui.Button("add"))
        {
            shader.circles = [..shader.circles, default];
        }

        for (int i = 0; i < shader.circles.Length; i++)
        {
            ImGui.Separator();
            ImGui.PushID(i);
            ImGui.DragFloat("x", ref shader.circles[i].X);
            ImGui.DragFloat("y", ref shader.circles[i].Y);
            ImGui.DragFloat("radius", ref shader.circles[i].Z);
            if (ImGui.Button("remove"))
            {
                shader.circles = [..shader.circles[..i], ..shader.circles[(i + 1)..]];
            }
            ImGui.PopID();
        }

        for (int i = 0; i < shader.circles.Length; i++)
        {
            shader.circles[i].Z += Time.DeltaTime * 100;
        }

        if (Mouse.IsButtonPressed(MouseButton.Left))
        {
            shader.circles = [.. shader.circles, new(Mouse.Position.X, Mouse.Position.Y, 0, 0)];
        }

        canvas.Fill(shader);
        canvas.DrawRect(0, 0, canvas.Width, canvas.Height, Alignment.TopLeft);
    }
}

class MyShader : CanvasShader
{
    public Vector4[] circles;

    public override ColorF GetPixelColor(Vector2 position)
    {
        ColorF color = ColorF.Black;
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
        return color = new(1 - (minDist / 10f)*(minDist / 10f), 0, 0);
    }
}