using SimulationFramework;
using SimulationFramework.Drawing;

namespace BlankSimulation;

internal class BlankSimulation : Simulation
{
    ITexture tex;
    int zoom;
    public override void OnInitialize(AppConfig config)
    {
        tex = Graphics.LoadTexture("./texture.png");
    }

    public override void OnRender(ICanvas canvas)
    {
        canvas.Clear(Color.Gray);
        zoom += Mouse.ScrollWheelDelta;
        canvas.Scale(MathF.Pow(1.1f, zoom));
        canvas.DrawTexture(tex);
    }
}