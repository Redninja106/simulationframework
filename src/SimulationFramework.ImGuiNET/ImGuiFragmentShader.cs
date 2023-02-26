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
    [Uniform]
    public ITexture<Color> texture;

    [Uniform]
    public TextureSampler sampler;

    [Input(InputSemantic.Position)]
    Vector4 outPosition;

    [Input(LinkageName = "outUV")]
    Vector2 uv;

    [Input(LinkageName = "outColor")]
    ColorF color;

    [Output(OutputSemantic.Color)]
    ColorF fragColor;

    public void Main()
    {
        fragColor = color * texture.Sample(uv, sampler);
    }
}