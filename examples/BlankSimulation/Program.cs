using SimulationFramework;

using var sim = new MySimulation();
Simulation.Run(sim); 

class MySimulation : Simulation
{
    public override void OnInitialize(AppConfig config)
    {
        config.OpenWindow(1920, 1080, "Simulation!");
    }

    float d;

    public override void OnRender(ICanvas canvas)
    {
        canvas.Clear(Color.CornflowerBlue);
        canvas.DrawRect(d % 1920,100,100,100, Color.Black, Alignment.BottomRight);
        canvas.DrawRect((d * 2) % 1920,300,100,100, Color.Black, Alignment.BottomRight);

        canvas.DrawEllipse(Mouse.Position, (250, 250), Mouse.IsButtonDown(MouseButton.Left) ? (255,0,0,123) : Color.OrangeRed);

        d += Time.DeltaTime * 100;
    }

    public override void OnUnitialize()
    {
    }
}