using SimulationFramework;
using SimulationFramework.Drawing.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ShaderIntegration.Shaders;

[ShaderTest]
internal class LinearShader : CanvasShader
{
    public override ColorF GetPixelColor(Vector2 position)
    {
        return new(0, 0, 0);
    }
}
