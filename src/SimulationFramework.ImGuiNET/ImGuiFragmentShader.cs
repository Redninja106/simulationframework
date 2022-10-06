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
    public ITexture texture;

    [ShaderIn]
    [ShaderOut(OutSemantic.Color)]
    ColorF color;

    [ShaderIn]
    Vector2 uv;

    public void Main()
    {
        // color = color.ToVector4() * texture.Sample(uv).ToVector4());
    }
}
