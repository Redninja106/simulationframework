using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

/// <summary>
/// Provider time measurements to an environment.
/// </summary>
public interface ITimeProvider : IApplicationComponent
{
    /// <summary>
    /// The largest value deltaTime before it's clamped.
    /// </summary>
    float MaxDeltaTime { get; set; }

    /// <summary>
    /// Gets the amount time that has passed since the simulation has started, in seconds.
    /// </summary>
    float GetTotalTime();

    /// <summary>
    /// Gets the amount time that has passed since the last frame of the simulation, in seconds.
    /// </summary>
    float GetDeltaTime();

    /// <summary>
    /// <see langword="true"/> if deltaTime had to be clamped to MaxDeltaTime this frame.
    /// </summary>
    bool IsRunningSlowly();
}