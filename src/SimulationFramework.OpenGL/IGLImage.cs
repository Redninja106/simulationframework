using SimulationFramework.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.OpenGL;
public interface IGLImage
{
    public IMask? Resident { get; set; }
    public int Width { get; }
    public int Height { get; }
}
