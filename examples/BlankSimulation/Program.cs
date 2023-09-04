using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.Input;

Start<Program>();

partial class Program : Simulation
{
    ITexture tex;
    public override void OnInitialize()
    {
        tex = Graphics.CreateTexture(1024, 1024);
        tex.GetCanvas().Clear(Color.Red);
        Window.SetIcon(tex);
    }

    public override void OnRender(ICanvas canvas)
    {
        canvas.Clear(Color.Black);
        canvas.DrawTexture(tex);
    }
}