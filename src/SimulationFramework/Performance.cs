using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

/// <summary>
/// Provides performance-related information.
/// </summary>
public static class Performance
{
    /// <summary>
    /// The simulations current framerate.
    /// </summary>
    public static float Framerate => 1f / (Application.Current.GetComponent<ITimeProvider>() ?? throw Exceptions.CoreComponentNotFound()).GetDeltaTime();
}     