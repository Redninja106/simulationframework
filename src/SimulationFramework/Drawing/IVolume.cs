using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing;
public interface IVolume<T> where T : unmanaged
{
    public int Width { get; }
    public int Height { get; }
    public int Depth { get; }
}
