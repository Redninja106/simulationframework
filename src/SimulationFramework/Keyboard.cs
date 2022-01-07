using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

public static class Keyboard
{
    internal static IInputProvider Provider => Simulation.Current.GetComponent<IInputProvider>();

    public static bool IsKeyDown(Key key) => Provider.IsKeyDown(key);
    public static bool IsKeyUp(Key key) => !Provider.IsKeyDown(key);
    public static bool IsKeyPressed(Key key) => Provider.IsKeyPressed(key);
    public static bool IsKeyReleased(Key key) => Provider.IsKeyReleased(key);
}