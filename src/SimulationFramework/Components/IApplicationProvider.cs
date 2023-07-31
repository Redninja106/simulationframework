using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Components;

/// <summary>
/// Provides the simulation with access to certain system functionalities.
/// </summary>
public interface IApplicationProvider : ISimulationComponent
{
    /// <summary>
    /// The system's primary display.
    /// </summary>
    IDisplay PrimaryDisplay { get; }

    /// <summary>
    /// Gets all of the system's currently active displays.
    /// </summary>
    IEnumerable<IDisplay> GetDisplays();

    /// <summary>
    /// Requests that the application exits.
    /// </summary>
    void Exit(bool cancellable);

    /// <summary>
    /// Cancels a request to exit the application.
    /// </summary>
    void CancelExit();
}
