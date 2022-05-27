using System;
using System.Diagnostics;
using SimulationFramework;
using SimulationFramework.Desktop;

class Program : Simulation
{
    static void Main()
    {
        using var sim = new Program();
        sim.RunWindowed("Canvas State Test", 1920, 1080, true);
    }

    public override void OnInitialize(AppConfig config)
    {
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