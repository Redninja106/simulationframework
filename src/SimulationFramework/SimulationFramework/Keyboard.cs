using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

public static class Keyboard
{
    internal static IInputProvider Provider => Simulation.Current.GetComponent<IInputProvider>();

    public static bool IsKeyDown(Key key)
    {
        return Provider.IsKeyDown(key);
    }
}