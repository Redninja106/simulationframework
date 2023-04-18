using SimulationFramework.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

/// <summary>
/// Handles the simulation's event loop.
/// </summary>
public interface ISimulationController : ISimulationComponent
{
    void Start(Action runFrame);
}