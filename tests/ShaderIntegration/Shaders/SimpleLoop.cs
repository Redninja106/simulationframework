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
internal class SimpleLoop : CanvasShader
{
    int x;

    public override ColorF GetPixelColor(Vector2 position)
    {
        float y = x;

        for (int i = 0; i < x; i++)
        {
            y *= 1.5f;
        }

        return new(y, y, y);
    }
}
