using SimulationFramework.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.OpenGL.Geometry;
internal abstract class GeometryEffect
{
    public abstract void Apply(GLCanvas canvas, Matrix4x4 projection);
    public abstract bool CheckStateCompatibility(ref readonly CanvasState state);

    public ShaderProgram Program { get; }

    public GeometryEffect(ShaderProgram program)
    {
        Program = program;
    }
}