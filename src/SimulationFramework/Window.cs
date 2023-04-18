using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

public static class Window
{
    private static IWindowProvider Provider => Application.GetComponent<IWindowProvider>();
}
