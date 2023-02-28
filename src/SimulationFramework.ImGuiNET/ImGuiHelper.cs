using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.ImGuiNET;
public static class ImGuiHelper
{
    public static bool ColorEdit3(string label, ref Color color)
    {
        var vector = color.ToVector3();
        var result = ImGui.ColorEdit3(label, ref vector);
        color = new(vector);
        return result;
    }

    public static bool ColorEdit4(string label, ref Color color)
    {
        var vector = color.ToVector4();
        var result = ImGui.ColorEdit4(label, ref vector);
        color = new(vector);
        return result;
    }

    public static bool ColorPicker3(string label, ref Color color)
    {
        var vector = color.ToVector3();
        var result = ImGui.ColorPicker3(label, ref vector);
        color = new(vector);
        return result;
    }

    public static bool ColorPicker4(string label, ref Color color)
    {
        var vector = color.ToVector4();
        var result = ImGui.ColorPicker4(label, ref vector);
        color = new(vector);
        return result;
    }

    public static bool ColorEdit3(string label, ref ColorF color)
    {
        var vector = color.ToVector3();
        var result = ImGui.ColorEdit3(label, ref vector);
        color = new(vector);
        return result;
    }

    public static bool ColorEdit4(string label, ref ColorF color)
    {
        var vector = color.ToVector4();
        var result = ImGui.ColorEdit4(label, ref vector);
        color = new(vector);
        return result;
    }

    public static bool ColorPicker3(string label, ref ColorF color)
    {
        var vector = color.ToVector3();
        var result = ImGui.ColorPicker3(label, ref vector);
        color = new(vector);
        return result;
    }

    public static bool ColorPicker4(string label, ref ColorF color)
    {
        var vector = color.ToVector4();
        var result = ImGui.ColorPicker4(label, ref vector);
        color = new(vector);
        return result;
    }
}
