using SimulationFramework.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.OpenGL;
public static class GLInterop
{
    public static uint GetTextureID(ITexture texture)
    {
        return ((GLTexture)texture).GetID();
    }
}
