using SimulationFramework.Drawing.Shaders;
using SimulationFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ShaderIntegration.Shaders;

[ShaderTest]
internal class SimpleLoop : CanvasShader
{
    public override ColorF GetPixelColor(Vector2 position)
    {
        float y = 1;

        for (int i = 0; i < 10; i++)
        {
            y *= 1.5f;
        }

        return new(y, y, y);
    }
}

[ShaderTest]
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


[ShaderTest]
internal class BreakStatement : CanvasShader
{
    int x;

    public override ColorF GetPixelColor(Vector2 position)
    {
        float y = 1;
        for (int i = 0; i < x; i++)
        {
            y *= 1.5f;
            if (y < 23)
            {
                break;
            }
        }
        return new(y, y, y);
    }
}

[ShaderTest]
class NestedLoop : CanvasShader
{
    public override ColorF GetPixelColor(Vector2 position)
    {
        float a = 0;
        for (int i = 0; i < position.X; i++)
        {
            a *= 1.25f;
            for (int j = 0; j < position.Y; j++)
            {
                a += 1;
            }
            a *= .8f;
        }

        return new(a, a, a);
    }
}