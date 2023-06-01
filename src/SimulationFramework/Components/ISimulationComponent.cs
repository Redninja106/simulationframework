using SimulationFramework.Messaging;
using System;

namespace SimulationFramework.Components;

/// <summary>
/// The base interface for all application components.
/// </summary>
public interface ISimulationComponent : IDisposable
{
    void Initialize(MessageDispatcher dispatcher);
}