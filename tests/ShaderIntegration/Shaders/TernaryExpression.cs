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
internal class TernaryExpression : CanvasShader
{
    public override ColorF GetPixelColor(Vector2 position)
    {
        return position.X > 50 ? ColorF.Red : ColorF.Blue;
    }
}
