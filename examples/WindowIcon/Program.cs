using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.Input;

Start<Program>();

partial class Program : Simulation
{
    readonly ITexture logo = Graphics.LoadTexture("./logo-128x128.png");

    public override void OnInitialize()
    {
        Window.SetIcon(logo);
    }

    public override void OnRender(ICanvas canvas)
    {
        canvas.Clear(Color.Black);

        Mouse.SetCursor(logo, Alignment.Center);
    }
}