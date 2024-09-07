using SimulationFramework.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.OpenGL;
public static class GLInterop
{
    public static uint GetMaskID(IDepthMask mask)
    {
        var glTexture = (GLMask)mask;
        glTexture.PrepareForRender();
        return glTexture.GetID();
    }

    public static uint GetTextureID(ITexture texture)
    {
        var glTexture = (GLTexture)texture;
        glTexture.PrepareForRender();
        return glTexture.GetID();
    }
}
