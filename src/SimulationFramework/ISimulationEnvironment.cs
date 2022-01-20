using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

/// <summary>
/// Provides an environment for a simulation to run in.
/// </summary>
public interface ISimulationEnvironment : IDisposable
{
    /// <summary>
    /// Makes the enviroment's context current on the calling thread.
    /// </summary>
    void MakeContextCurrent();

    /// <summary>
    /// Returns true if the simulation should exit, or false if its should keep running.
    /// </summary>
    bool ShouldExit();

    /// <summary>
    /// Called by the simulation at the start of a frame, for the simulation to process system events.
    /// </summary>
    void ProcessEvents();

    /// <summary>
    /// Called by the simulation when it is done rendering a frame.
    /// </summary>
    void EndFrame();

    /// <summary>
    /// Gets the size of the video output.
    /// </summary>
    (int, int) GetOutputSize();

    /// <summary>
    /// Creates all of the supported simulation components in this environment.
    /// </summary>
    IEnumerable<ISimulationComponent> CreateSupportedComponents();
}