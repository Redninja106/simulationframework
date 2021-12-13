using SimulationFramework;

using var sim = new MySimulation();
Simulation.Run(sim); 

class MySimulation : Simulation
{
    ISurface bitmap;

    public override void OnInitialize(AppConfig config)
    {
        config.OpenWindow(1920, 1080, "Simulation!");
        bitmap = Graphics.CreateSurface(200, 200);
        using var canvas = bitmap.OpenCanvas();
        canvas.Clear((0,0,0));
        canvas.DrawEllipse(0, 0, 100, 100, Color.Red, Alignment.TopLeft);
    }

    public override void OnRender(ICanvas canvas)
    {
        canvas.Clear(Color.CornflowerBlue);
        canvas.DrawRect(100,100,100,100, Color.Red, Alignment.BottomRight);
    }

    public override void OnUnitialize()
    {
    }
}
