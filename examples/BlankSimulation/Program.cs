using SimulationFramework;
using SimulationFramework.IMGUI;

using var sim = new MySimulation();
Simulation.RunWindowed(sim, "Simulation!", 1920, 1080); 

class MySimulation : Simulation
{
    public override void OnInitialize(AppConfig config)
    {
        this.Resized += (x, y) => Console.WriteLine($"resized to {x},{y}");
    }

    float d;
    Vector2 p;

    public override void OnRender(ICanvas canvas)
    {
        canvas.Clear(Color.CornflowerBlue);
        canvas.DrawRect((d * 100) % 1920,100,100,100, Color.Black, Alignment.BottomRight);
        canvas.DrawRect((d * 200) % 1920,300,100,100, Color.Black, Alignment.BottomRight);
        
        using (canvas.Push())
        {
            canvas.Translate(p.X, p.Y);
            canvas.DrawRect((0, 0), (100, 100), Color.Purple, Alignment.Center);
        }
        
        canvas.DrawEllipse(Mouse.Position, (250, 250), Mouse.IsButtonDown(MouseButton.Left) ? (255,0,0,123) : Color.OrangeRed);
        
        d += Time.DeltaTime;
        if (ImGui.BeginWindow("ImGui testing"))
        {
            ImGui.Text("Hello there!");
            ImGui.Text("fps: " + (int)Debug.Framerate + (Time.IsRunningSlowly ? "Running slowly!" : ""));
            ImGui.DragFloat("TIME", ref d);
            ImGui.DragFloat("POS", ref p);
        }

        canvas.SetFont("arial", TextStyles.Default, 120);
        canvas.SetDrawMode(DrawMode.Border);
        canvas.DrawRect(Mouse.Position, canvas.MeasureText("HELLO WORLD"), Color.Orange, Alignment.BottomRight);
        canvas.SetDrawMode(DrawMode.Fill);
        canvas.DrawText("HELLO WORLD", Mouse.Position, Color.Orange, Alignment.BottomRight);
    }

    public override void OnUnitialize()
    {
    }
}