using System.ComponentModel;
using System.Runtime.InteropServices;
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
    Vector2 p;
    Queue<float> fps = new(1921);
    float scale = 100f;

    public override void OnRender(ICanvas canvas)
    {
        canvas.Clear(Color.CornflowerBlue);

        fps.Enqueue(Debug.Framerate);

        while (fps.Sum(x => 1f / x) * scale > Width)
        {
            fps.Dequeue();
        }

        var lastval = fps.Peek();
        foreach (var val in fps)
        {
            var lastEntryWidth = (1 / lastval) * scale;
            var entryWidth = (1 / val) * scale;
            var lineWidth = (lastEntryWidth + entryWidth) * .5f;

            canvas.DrawLine(0, Height - lastval, lineWidth, Height - val, Color.Yellow);
            canvas.Translate(lineWidth, 0);
            lastval = val;
        }

        ImGui.DragFloat("scale", ref scale);
    }

    public override void OnUnitialize()
    {
    }
}