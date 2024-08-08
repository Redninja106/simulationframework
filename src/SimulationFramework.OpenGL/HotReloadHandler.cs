using SimulationFramework.Components;
using SimulationFramework.Drawing;
using SimulationFramework.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;
internal static class HotReloadHandler
{
    public static void ClearCache(Type[]? updatedTypes)
    {
        // TODO: BUG: shaders arent updated if a method in a different class that is used by the shader is updated
        var graphics = Application.GetComponent<GLGraphicsProvider>();

        if (updatedTypes != null) 
        {
            foreach (var type in updatedTypes)
            {
                graphics.RemoveCachedShader(type);
            }
        }
    }

    // public static void UpdateApplication(Type[]? updateTypes)
    // {
    // }
}
