using System.Collections.Generic;

namespace SimulationFramework.Components;

/// <summary>
/// Provides a <see cref="SimulationHost"/> with components to run a <see cref="Simulation"/>.
/// </summary>
public interface ISimulationPlatform : ISimulationComponent
{
    IEnumerable<IDisplay> GetDisplays();
}