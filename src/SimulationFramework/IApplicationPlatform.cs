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
    /// Creates an <see cref="IApplicationController "/> for the simulation.
    /// </summary>
    IApplicationController CreateController();

    /// <summary>
    /// Creates an <see cref="IGraphicsProvider"/> for the simulation.
    /// </summary>
    IGraphicsProvider CreateGraphicsProvider();

    /// <summary>
    /// Creates an <see cref="ITimeProvider"/> for the simulation.
    /// </summary>
    ITimeProvider CreateTimeProvider();

    /// <summary>
    /// Creates any additional <see cref="IApplicationComponent"/>s for the simulation.
    /// </summary>
    IEnumerable<IApplicationComponent> CreateAdditionalComponents();
}