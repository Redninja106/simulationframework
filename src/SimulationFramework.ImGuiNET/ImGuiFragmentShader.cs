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
    public ITexture<Color> texture;

    [ShaderInput(InputSemantic.Position)]
    Vector4 position;

    [ShaderInput]
    Vector2 uv;

    [ShaderInput]
    Vector4 color;

    [ShaderOutput(OutputSemantic.Color)]
    Vector4 outColor;

    public void Main()
    {
        outColor = color;
        // color = color.ToVector4() * texture.Sample(uv).ToVector4());
    }
}
