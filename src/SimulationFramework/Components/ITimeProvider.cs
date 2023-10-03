namespace SimulationFramework.Components;

/// <summary>
/// Provider time measurements to an environment.
/// </summary>
public interface ITimeProvider : ISimulationComponent
{
    /// <summary>
    /// The largest value deltaTime before it's clamped.
    /// </summary>
    float MaxDeltaTime { get; set; }

    /// <summary>
    /// </summary>
    float Scale { get; set; }

    /// <summary>
    /// Gets the amount time that has passed since the simulation has started, in seconds.
    /// </summary>
    float GetTotalTime();

    /// <summary>
    /// Gets the amount time that has passed since the last frame of the simulation, in seconds.
    /// </summary>
    float GetDeltaTime();

    /// <summary>
    /// Gets the unscaled, unclamped amount of time that has passes since the last frame of the simulation, in seconds.
    /// </summary>
    /// <returns></returns>
    float GetRawDeltaTime();

    /// <summary>
    /// <see langword="true"/> if deltaTime had to be clamped to MaxDeltaTime this frame.
    /// </summary>
    bool IsRunningSlowly();
}