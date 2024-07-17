using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders;
public static class TextureExtensions
{
    public static ColorF SampleUV(this ITexture texture, Vector2 uv)
    {
        return ShaderIntrinsics.TextureSample(texture, uv);
    }

    public static ColorF Sample(this ITexture texture, Vector2 position)
    {
        return ShaderIntrinsics.TextureSample(texture, position);
    }

    public static ColorF Load(this ITexture texture, int x, int y)
    {
        return ShaderIntrinsics.TextureLoad(texture, x, y);
    }

    public static void Store(this ITexture texture, int x, int y, ColorF color)
    {
        ShaderIntrinsics.TextureStore(texture, x, y, color);
    }
}
