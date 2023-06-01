using System;

namespace SimulationFramework.Components;

/// <summary>
/// Handles the simulation's event loop.
/// </summary>
public interface ISimulationController : ISimulationComponent
{
    /// <summary>
    /// Starts the underlying system's event loop, calling the provided callback to run each frame. This method should not return until the event loop ends.
    /// </summary>
    /// <param name="runFrame">The callback to run one frame of the simulation.</param>
    void Start(Action runFrame);
}