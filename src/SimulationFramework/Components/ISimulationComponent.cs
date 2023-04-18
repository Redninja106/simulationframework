using SimulationFramework.Drawing;
using SimulationFramework.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Components;

/// <summary>
/// The base interface for all application components.
/// </summary>
public interface ISimulationComponent : IDisposable
{
    void Initialize(MessageDispatcher dispatcher);
}