using System;
using System.Diagnostics;
using SimulationFramework;
using SimulationFramework.Desktop;
using SimulationFramework.Drawing.Canvas;

class Program : Simulation
{
    static void Main()
    {
        var sim = new Program();
        sim.RunDesktop();
    }

    public override void OnInitialize(AppConfig config)
    {
        config.Title = "Canvas State Test";
        config.Width = 1920;
        config.Height = 1080;
        config.Resizable = true;
    }

    public override void OnRender(ICanvas canvas)
    {
        const int count = 1_000_000;

        for (int i = 0; i < count; i++)
            canvas.PushState();

        Thread.Sleep(500);

        var prevMem = Process.GetCurrentProcess().PrivateMemorySize64;
        for (int i = 0; i < count; i++)
        {
            canvas.PopState();
        }
        var postMem = Process.GetCurrentProcess().PrivateMemorySize64;
        Console.WriteLine((postMem - prevMem) / count);

        Thread.Sleep(500);
    }

    private string GetMem()
    {
        return (Environment.WorkingSet / 1_000_000_000f).ToString();
    }
}