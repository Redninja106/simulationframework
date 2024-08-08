using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders;
public abstract class VertexShader : Shader
{
    public abstract Vector4 GetVertexPosition();
}
