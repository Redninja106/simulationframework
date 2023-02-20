using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing;
public abstract class GraphicsCapabilities
{
    // compute shader
    public abstract int MaxThreadGroupWidth { get; }
    public abstract int MaxThreadGroupHeight { get; }
    public abstract int MaxThreadGroupDepth { get; }
    public abstract int MaxThreadGroupSize { get; }
    public abstract int MaxThreadGroupCountX { get; }
    public abstract int MaxThreadGroupCountY { get; }
    public abstract int MaxThreadGroupCountZ { get; }
}
