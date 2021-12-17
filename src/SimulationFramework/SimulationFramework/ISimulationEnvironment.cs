using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

public interface ISimulationEnvironment : IDisposable
{
    /// <summary>
    /// Makes the enviroment's context current on the calling thread.
    /// </summary>
    void MakeContextCurrent();

    bool ShouldExit();

    void ProcessEvents();

    void EndFrame();

    (int, int) GetOutputSize();

    IEnumerable<ISimulationComponent> CreateSupportedComponents();
}