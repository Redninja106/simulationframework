using SimulationFramework;
using SimulationFramework.Drawing.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ShaderIntegration.Shaders;

[Test]
internal class ContinueStatement : CanvasShader
{
    public override ColorF GetPixelColor(Vector2 position)
    {
        float y = 2;
        while (y < 10)
        {
            y /= 1.5f;

            if (y == 6)
                continue;

            y *= 2;
        }
        return new(y, y, y);
    }
}
