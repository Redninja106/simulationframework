using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Components;
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

    void Exit(bool cancellable);
    void CancelExit();
}
