using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;
public interface IDisplay
{
    bool IsPrimary { get; }
    Rectangle Bounds { get; }
    string Name { get; }
    float Scaling { get; }
    float RefreshRate { get; }
}