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

    public void EnableImGui() { }
    public void EnableSimulationPane(int width, int height, bool scaling = true) { }
    
    /// <summary>
    /// Runs the simulation in a interactable window environment.
    /// </summary>
    /// <param name="width">The stating width of the window.</param>
    /// <param name="height">The stating height of the window.</param>
    /// <param name="title">The title of the window.</param>
    public void OpenWindow(int width, int height, string title) 
    { 
        simulation.SetEnvironment(new WindowEnvironment(width, height, title));
    }
}