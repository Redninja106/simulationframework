using SimulationFramework.Messaging;

namespace SimulationFramework.Components;

/// <summary>
/// The base interface for all application components.
/// </summary>
public interface ISimulationComponent : IDisposable
{
    /// <summary>
    /// Initializes the component. This is called once after the component is added to a simulation.
    /// </summary>
    /// <param name="dispatcher">The simulation's <see cref="MessageDispatcher"/>. This can be used to listen for messages to be called back at a later frame.</param>
    void Initialize(MessageDispatcher dispatcher);
}