using System.ComponentModel;
using System.Runtime.InteropServices;
using SimulationFramework;
using SimulationFramework.IMGUI;
using SimulationFramework.Desktop;

using var sim = new MySimulation();
sim.RunWindowed("Framerate graph!", 1920, 1080);

class MySimulation : Simulation
{
    public override void OnInitialize(AppConfig config)
    {
    }

    Queue<float> fps = new(1921);
    float scale = 100f;

    public override void OnRender(ICanvas canvas)
    {
        canvas.Clear(Color.CornflowerBlue);

        fps.Enqueue(Performance.Framerate);

        while (fps.Sum(x => 1f / x) * scale > canvas.Width)
        {
            fps.Dequeue();
        }

        canvas.Fill(Color.Yellow);
        var lastval = fps.Peek();
        foreach (var val in fps)
        {
            var lastEntryWidth = (1 / lastval) * scale;
            var entryWidth = (1 / val) * scale;
            var lineWidth = (lastEntryWidth + entryWidth) * .5f;
            
            canvas.DrawLine(0, canvas.Width - lastval, lineWidth, canvas.Height - val);
            canvas.Translate(lineWidth, 0);
            lastval = val;
        }

        ImGui.DragFloat("scale", ref scale);
    }

    public override void OnUnitialize()
    {
    }
}