using SimulationFramework.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Components;

/// <summary>
/// Provides a <see cref="SimulationHost"/> with components to run a <see cref="Simulation"/>.
/// </summary>
public interface ISimulationPlatform : ISimulationComponent
{
    IEnumerable<IDisplay> GetDisplays();
}