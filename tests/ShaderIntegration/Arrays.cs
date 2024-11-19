using SimulationFramework;
using SimulationFramework.Drawing.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ShaderIntegration;

[ShaderTest]
internal class Array2DShader : CanvasShader
{
    float[,] testArray = new float[10, 10];

    public override ColorF GetPixelColor(Vector2 position)
    {
        return ColorF.White * testArray[(int)position.X, (int)position.Y];
    }
}

[ShaderTest]
internal class Array3DShader : CanvasShader
{
    float[,,,] testArray = new float[10, 10, 10, 1];

    public override ColorF GetPixelColor(Vector2 position)
    {
        return ColorF.White * testArray[(int)position.X, (int)position.Y, (int)position.Y, 1];
    }
}