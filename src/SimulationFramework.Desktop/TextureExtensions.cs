using SimulationFramework.Drawing;
using SimulationFramework.OpenGL;

namespace SimulationFramework.Desktop;
public static class TextureExtensions
{
    public static nint GetImGuiID(this ITexture texture)
    {
        return (nint)GLInterop.GetTextureID(texture);
    }

    public static nint GetImGuiID(this IDepthMask mask)
    {
        return (nint)GLInterop.GetMaskID(mask);
    }
}
