using SimulationFramework.Drawing;
using SimulationFramework.SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Desktop;
public static class TextureExtensions
{
    public static nint GetImGuiID(this ITexture texture)
    {
        return (nint)SkiaInterop.GetGLTextureID(texture);
    }
}
