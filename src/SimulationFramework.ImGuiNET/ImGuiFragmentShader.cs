using SimulationFramework.Drawing;
using SimulationFramework.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.ImGuiNET;
internal struct ImGuiFragmentShader : IShader
{
    // public ITexture<Color> texture;

    [ShaderInput(InputSemantic.Position)]
    Vector4 outPosition;

    [ShaderInput]
    Vector2 outUV;

    [ShaderInput]
    ColorF outColor;

    [ShaderOutput(OutputSemantic.Color)]
    ColorF fragColor;

    public void Main()
    {
        fragColor = outColor;
        // color = color.ToVector4() * texture.Sample(uv).ToVector4());
    }
}
