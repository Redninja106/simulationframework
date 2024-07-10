using SimulationFramework.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.OpenGL;
internal abstract class GeometryStream
{
    public Matrix3x2 TransformMatrix { get; set; }
    public abstract void WriteVertex(Vector2 position);
    public abstract void Upload(GeometryBuffer buffer);
    public abstract int GetVertexCount();
    public abstract void Clear();
    public abstract void BindVertexArray();
}
