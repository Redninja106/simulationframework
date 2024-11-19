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
internal class SimpleConditional : CanvasShader
{
    float x;

    public override ColorF GetPixelColor(Vector2 position)
    {
        float y;
        if (x < 100)
        {
            y = 10;
        }
        else
        {
            y = 20;
        }
        return new(y, y, y);
    }
}

[ShaderTest]
internal class ShortCircuitConditional : CanvasShader
{
    float x;

    public override ColorF GetPixelColor(Vector2 position)
    {
        float y = 0;
        if (x == 1 || x == 2 || x == 3 || x == 4)
        {
            y = 12;
        }
        if (x == 1 && x >= 2 && x < 3 && x > 4)
        {
            y = 12;
        }
        return new(y, y, y);
    }
}

