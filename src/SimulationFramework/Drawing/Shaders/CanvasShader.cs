using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders;
public abstract class CanvasShader : Shader
{
    public Matrix3x2 TransformMatrix { get; set; } = Matrix3x2.Identity;

    public abstract ColorF GetPixelColor(Vector2 position);
}
