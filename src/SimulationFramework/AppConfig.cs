using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

/// <summary>
/// Provides ways to enable and disable certain functionality of the simulation.
/// </summary>
public sealed class AppConfig
{
    private readonly Simulation simulation;

    internal AppConfig(Simulation simulation)
    {
        this.simulation = simulation;
    }

    public void EnableSimulationPane(int width, int height, bool scaling = true) { }

    public void SetAngleMode(AngleMode angleMode) 
    {
        simulation.AngleMode = angleMode; 
    }
}