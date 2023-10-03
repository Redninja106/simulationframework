using SimulationFramework.Drawing;
using SimulationFramework.SkiaSharp;

namespace SimulationFramework.Desktop;
public static class TextureExtensions
{
    public static nint GetImGuiID(this ITexture texture)
    {
        return (nint)SkiaInterop.GetGLTextureID(texture);
    }
}
