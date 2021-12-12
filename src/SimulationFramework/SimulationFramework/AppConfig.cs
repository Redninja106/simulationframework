using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

public sealed class AppConfig
{
    internal AppConfig(Simulation simulation)
    {
        simulation.SetEnvironment(new WindowEnvironment());
    }

    public void EnableImGui() { }
    public void EnableSimulationPane(int width, int height, bool scaling = true) { }
    public void OpenWindow(int width, int height, string title) 
    { 
    
    }
}