using SimulationFramework;
using SimulationFramework.Components;
using SimulationFramework.Desktop;
using SimulationFramework.Drawing;
using SimulationFramework.Input;
using System.Numerics;

Start<Program>(new DesktopPlatform());

partial class Program : Simulation
{
    private ITexture texture;

    public override void OnInitialize()
    {
        texture = Graphics.CreateTexture(256, 256);
        texture.GetCanvas().Clear(Color.Black);
    }

    public override void OnRender(ICanvas canvas)
    {
        canvas.Clear(Color.White);
        canvas.Fill(texture);
        canvas.DrawRoundedRect(new Vector2(canvas.Width / 2, canvas.Height / 2), Vector2.One * 256, 10, Alignment.Center);
    }
}