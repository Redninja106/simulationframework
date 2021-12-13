using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

public sealed class AppConfig
{
    private readonly Simulation simulation;

    internal AppConfig(Simulation simulation)
    {
        this.simulation = simulation;
    }

    public void EnableImGui() { }
    public void EnableSimulationPane(int width, int height, bool scaling = true) { }
    public void OpenWindow(int width, int height, string title) 
    { 
        simulation.SetEnvironment(new WindowEnvironment(width, height, title));
    }
}