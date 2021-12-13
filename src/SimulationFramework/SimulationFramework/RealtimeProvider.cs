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

    public RealtimeProvider()
    {
        stopwatch = Stopwatch.StartNew();
    }

    public void Apply(Simulation simulation)
    {
        simulation.BeforeRender += Tick;
    }

    private void Tick()
    {
        deltaTime = TimeScale * (stopwatch.ElapsedTicks / (float)Stopwatch.Frequency);

        if (deltaTime > MaxDeltaTime)
        {
            isRunningSlowly = true;
            deltaTime = MaxDeltaTime;
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