using System.Collections.Generic;

namespace SimulationFramework.Components;

/// <summary>
/// Provides a <see cref="SimulationHost"/> with components to run a <see cref="Simulation"/>.
/// </summary>
public interface ISimulationPlatform : ISimulationComponent
{
    /// <summary>
    /// Gets all of the system's currently active displays.
    /// </summary>
    IEnumerable<IDisplay> GetDisplays();

    /// <summary>
    /// The system's primary display.
    /// </summary>
    IDisplay PrimaryDisplay { get; }
}