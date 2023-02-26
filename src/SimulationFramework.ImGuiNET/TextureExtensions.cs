using SimulationFramework.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.ImGuiNET;
public static class TextureExtensions
{
    public static nint GetImGuiTextureID(this ITexture<Color> texture)
    {
        var imguiComponent = Application.Current?.GetComponent<ImGuiComponent>();

        if (imguiComponent is null)
            throw new InvalidOperationException("No simulation is active or no ImGui component is present.");

        return imguiComponent.GetOrRegisterTextureID(texture);
    }
}
