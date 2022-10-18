using SimulationFramework.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

/// <summary>
/// Handles the simulation's event loop.
/// </summary>
public interface IApplicationController : IApplicationComponent
{
    /// <summary>
    /// Starts a simulation event loop.
    /// </summary>
    /// <param name="dispatcher">The <see cref="MessageDispatcher"/> to provide with events.</param>
    void Start(MessageDispatcher dispatcher);

    /// <summary>
    /// Applies an AppConfig to the simulation.
    /// </summary>
    /// <param name="config"></param>
    /// <returns></returns>
    bool ApplyConfig(AppConfig config);

    /// <summary>
    /// Initalizes an appConfig with simulation's current state.
    /// </summary>
    /// <param name="config"></param>
    void InitializeConfig(AppConfig config);
}