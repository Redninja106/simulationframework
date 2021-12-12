using SimulationFramework;

using var sim = new MySimulation();
Simulation.Run(sim); 

class MySimulation : Simulation
{
    IBitmap bitmap;

    public override void OnInitialize(AppConfig config)
    {
        config.OpenWindow(1920, 1080, "Simulation!");
        bitmap = this.GraphicsProvider.CreateBitmap(200, 200);
        using var canvas = bitmap.OpenCanvas();
        canvas.Clear((0,0,0));
        canvas.DrawEllipse(0, 0, 100, 100, Color.Red, Alignment.TopLeft);
    }

    public override void OnRender(ICanvas canvas)
    {
        canvas.Clear(Color.CornflowerBlue);
        canvas.DrawSurface(bitmap, 100,100,100,100, Alignment.BottomRight);
    }

    public override void OnUnitialize()
    {
    }
}
