using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

public sealed class RealtimeProvider : ITimeProvider
{
    public float MaxDeltaTime { get; set; } = 1 / 30f;
    public float TimeScale { get; set; } = 1.0f;

    private readonly Stopwatch stopwatch;

    private float deltaTime;
    private float totalTime;
    private bool isRunningSlowly;
    private float framerate;

    public RealtimeProvider()
    {
        stopwatch = Stopwatch.StartNew();
    }

    public void Apply(Simulation simulation)
    {
        simulation.BeforeRender += Tick;
    }

    public float GetFramerate()
    {
        return framerate;
    }

    private void Tick()
    {
        deltaTime = TimeScale * (stopwatch.ElapsedTicks / (float)Stopwatch.Frequency);
        stopwatch.Restart();
        framerate = 1f / deltaTime;

        if (deltaTime > MaxDeltaTime)
        {
            isRunningSlowly = true;
            deltaTime = MaxDeltaTime;
        }
        else
        {
            isRunningSlowly = false;
        }

        totalTime += deltaTime;
    }

    public float GetTotalTime()
    {
        return totalTime;
    }

    public float GetDeltaTime()
    {
        return deltaTime;
    }

    public bool IsRunningSlowly()
    {
        return isRunningSlowly;
    }

    public void SetMaxDeltaTime(float maxDeltaTime)
    {
        this.MaxDeltaTime = maxDeltaTime;
    }

    public void Dispose()
    {
    }
}