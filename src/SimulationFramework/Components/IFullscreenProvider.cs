using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Components;

/// <summary>
/// Provides Fullscreen functionality to the application.
/// </summary>
public interface IFullscreenProvider : ISimulationComponent
{
    // Although this is conceptually a part of the window, it is not always internally handled by the platforms window system.

    bool PreferExclusive { get; set; }
    bool IsFullscreen { get; }

    void EnterFullscreen(IDisplay display);
    void ExitFullscreen();
}
