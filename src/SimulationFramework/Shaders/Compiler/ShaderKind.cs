using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using SimulationFramework.Drawing;

namespace SimulationFramework.Shaders.Compiler;

[Flags]
public enum ShaderKind
{
    Vertex = 1 << 0,
    Fragment = 2 << 0,
    Geometry = 3 << 0,
    Compute = 4 << 0,
}