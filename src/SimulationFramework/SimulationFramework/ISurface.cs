using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

public interface ISurface : IDisposable
{
    // surface properties 
    int Width { get; }
    int Height { get; }

    // drawing to the surface
    ICanvas OpenCanvas();
}