using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing;

public interface IGeometry : IDisposable
{
    TVertex[] GetVertices<TVertex>() where TVertex : unmanaged;
    uint[]? GetIndices();
}
