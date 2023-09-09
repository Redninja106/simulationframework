using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.SkiaSharp;

Start<Program>();

partial class Program : Simulation
{
    ITexture texture;

    public override void OnInitialize()
    {
        texture = Graphics.CreateTexture(100, 100);
        var canvas = texture.GetCanvas();
        canvas.Clear(Color.Red);
        canvas.Flush();
        
    }

    public override void OnRender(ICanvas canvas)
    {
        ImGuiNET.ImGui.Image((nint)SkiaInterop.GetGLTextureID(texture), new(100, 100));
        canvas.Clear(Color.Black);
    }
}