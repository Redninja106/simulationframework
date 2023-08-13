using SimulationFramework;
using SimulationFramework.Drawing;

Start<Program>();

partial class Program : Simulation
{
    IFont font = Graphics.LoadFont("Borel-Regular.ttf");

    public override void OnInitialize()
    {
    }

    public override void OnRender(ICanvas canvas)
    {
        canvas.FontSize(32);
        canvas.Font(font);
        canvas.DrawText("This text is drawn using a custom font!", 0, 0);
    }
}