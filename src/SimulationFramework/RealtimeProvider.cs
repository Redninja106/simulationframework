using SimulationFramework.Messaging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

/// <summary>
/// Feeds real time values into a simulation.
/// </summary>
[DefaultComponent<ITimeProvider>]
public sealed class RealtimeProvider : ITimeProvider
{
    /// <summary>
    /// </summary>
    public float MaxDeltaTime { get; set; } = float.PositiveInfinity;

    /// <summary>
    /// </summary>
    public float TimeScale { get; set; } = 1.0f;

    private readonly Stopwatch stopwatch;

    private float deltaTime;
    private float totalTime;
    private bool isRunningSlowly;

    /// <summary>
    /// </summary>
    public RealtimeProvider()
    {
        stopwatch = Stopwatch.StartNew();
    }

    private void Tick()
    {
        deltaTime = TimeScale * (stopwatch.ElapsedTicks / (float)Stopwatch.Frequency);
        stopwatch.Restart();

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

    /// <inheritdoc/>
    public float GetTotalTime()
    {
        return totalTime;
    }

    /// <inheritdoc/>
    public float GetDeltaTime()
    {
        return deltaTime;
    }

    /// <inheritdoc/>
    public bool IsRunningSlowly()
    {
        return isRunningSlowly;
    }

    /// <inheritdoc/>
    public void SetMaxDeltaTime(float maxDeltaTime)
    {
        this.MaxDeltaTime = maxDeltaTime;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
    }

    /// <inheritdoc/>
    public void Initialize(Application application)
    {
        application.Dispatcher.Subscribe<RenderMessage>(m =>
        {
            this.Tick();
        }, ListenerPriority.High);
    }
}