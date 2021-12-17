using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

/// <summary>
/// Provider time measurements to an environment.
/// </summary>
public interface ITimeProvider : ISimulationComponent
{
    float MaxDeltaTime { get; set; }
    float TimeScale { get; set; }

    float GetFramerate();
    float GetTotalTime();
    float GetDeltaTime();
    bool IsRunningSlowly();
    void SetMaxDeltaTime(float maxDeltaTime);
}