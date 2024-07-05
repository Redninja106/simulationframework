using SimulationFramework.Drawing;
using SimulationFramework.OpenGL;

namespace SimulationFramework.Desktop;
public static class TextureExtensions
{
    public static nint GetImGuiID(this ITexture texture)
    {
        return (nint)GLInterop.GetTextureID(texture);
        throw new NotImplementedException();
        // return (nint)SkiaInterop.GetGLTextureID(texture);
    }
}
