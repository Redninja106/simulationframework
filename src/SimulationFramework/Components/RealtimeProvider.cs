using SimulationFramework.Messaging;
using System.Diagnostics;

namespace SimulationFramework.Components;

/// <summary>
/// Feeds real time values into a simulation.
/// </summary>
public sealed class RealTimeProvider : ITimeProvider
{
    /// <summary>
    /// </summary>
    public float MaxDeltaTime { get; set; } = float.PositiveInfinity;

    /// <summary>
    /// </summary>
    public float Scale { get; set; } = 1.0f;

    private readonly Stopwatch stopwatch;

    private float deltaTime;
    private float rawDeltaTime;
    private float totalTime;
    private bool isRunningSlowly;

    /// <summary>
    /// </summary>
    public RealTimeProvider()
    {
        stopwatch = Stopwatch.StartNew();
    }

    private void Tick()
    {
        rawDeltaTime = stopwatch.ElapsedTicks / (float)Stopwatch.Frequency;
        deltaTime = Scale * rawDeltaTime;
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
    public float GetRawDeltaTime()
    {
        return rawDeltaTime;
    }

    /// <inheritdoc/>
    public bool IsRunningSlowly()
    {
        return isRunningSlowly;
    }

    /// <inheritdoc/>
    public void SetMaxDeltaTime(float maxDeltaTime)
    {
        MaxDeltaTime = maxDeltaTime;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
    }

    /// <inheritdoc/>
    public void Initialize(MessageDispatcher dispatcher)
    {
        dispatcher.Subscribe<BeforeRenderMessage>(m =>
        {
            Tick();
        });
    }
}