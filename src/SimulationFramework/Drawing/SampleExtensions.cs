using SimulationFramework.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing;
public static class SampleExtensions
{
    [ShaderIntrinsic]
    public static ColorF Sample(this ITexture<Color> texture, Vector2 uv, TextureSampler sampler)
    {
        throw new NotImplementedException();
    }
}
