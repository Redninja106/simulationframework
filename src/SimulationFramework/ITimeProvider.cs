using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

/// <summary>
/// Provider time measurements to an environment.
/// </summary>
public interface ITimeProvider : ISimulationComponent
{
    /// <summary>
    /// The largest value deltaTime is permitted to reach.
    /// </summary>
    float MaxDeltaTime { get; set; }

    /// <summary>
    /// The rate by which time is scaled.
    /// </summary>
    float TimeScale { get; set; }

    /// <summary>
    /// Gets the current framerate of the application in frames per second.
    /// </summary>
    float GetFramerate();

    /// <summary>
    /// Gets the amount time that has passed since the simulation has started, in seconds.
    /// </summary>
    float GetTotalTime();

    /// <summary>
    /// Gets the amount time that has passed since the last frame of the simulation, in seconds.
    /// </summary>
    float GetDeltaTime();

    /// <summary>
    /// Returns <see langword="true"/> if deltaTime had to be clamped to MaxDeltaTime.
    /// </summary>
    bool IsRunningSlowly();
}