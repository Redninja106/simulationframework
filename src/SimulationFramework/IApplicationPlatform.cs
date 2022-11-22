using SimulationFramework.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

/// <summary>
/// Provides an environment for a simulation to run in.
/// </summary>
public interface IApplicationPlatform : IApplicationComponent
{
    /// <summary>
    /// Provides the application with an IApplicationController.
    /// </summary>
    /// <returns></returns>
    IApplicationController CreateController();
    IGraphicsProvider CreateGraphicsProvider();
    ITimeProvider CreateTimeProvider();
    IEnumerable<IApplicationComponent> CreateAdditionalComponents();
}