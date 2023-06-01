using System;

namespace SimulationFramework.Components;

/// <summary>
/// Handles the simulation's event loop.
/// </summary>
public interface ISimulationController : ISimulationComponent
{
    void Start(Action runFrame);
}