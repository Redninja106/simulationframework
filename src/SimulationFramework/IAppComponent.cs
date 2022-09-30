using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

/// <summary>
/// The base interface for all application components.
/// </summary>
public interface IAppComponent : IDisposable
{
    /// <summary>
    /// Initializes the component.
    /// This method is only called once on each component.
    /// </summary>
    /// <param name="application">The application to which this <see cref="IAppComponent"/> belongs to.</param>
    void Initialize(Application application);
}