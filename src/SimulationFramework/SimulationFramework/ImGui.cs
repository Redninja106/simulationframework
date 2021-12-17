using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

/// <summary>
/// 
/// </summary>
public static class ImGui
{
    internal static IImGuiProvider Provider => Simulation.Current.GetComponent<IImGuiProvider>();

    public static void Text(string text) => Provider.TextUnformatted(text);

    public static bool DragFloat(string label, ref float value, float speed = 1.0f, float min = 0.0f, float max = 0.0f, string format = "%.3f", SliderFlags flags = SliderFlags.None)
    {
        return Provider.DragFloat(label, ref value, speed, min, max, format, flags);
    }

    public static float DragFloat(string label, float value, float speed = 1.0f, float min = 0.0f, float max = 0.0f, string format = "%.3f", SliderFlags flags = SliderFlags.None)
    {
        Provider.DragFloat(label, ref value, speed, min, max, format, flags);
        return value;
    }

    public static bool DragFloat(string label, ref Vector2 value, float speed = 1.0f, float min = 0.0f, float max = 0.0f, string format = "%.3f", SliderFlags flags = SliderFlags.None)
    {
        return Provider.DragFloat(label, ref value, speed, min, max, format, flags);
    }

    public static Vector2 DragFloat(string label, Vector2 value, float speed = 1.0f, float min = 0.0f, float max = 0.0f, string format = "%.3f", SliderFlags flags = SliderFlags.None)
    {
        Provider.DragFloat(label, ref value, speed, min, max, format, flags);
        return value;
    }

    public static bool DragFloat(string label, ref Vector3 value, float speed = 1.0f, float min = 0.0f, float max = 0.0f, string format = "%.3f", SliderFlags flags = SliderFlags.None)
    {
        return Provider.DragFloat(label, ref value, speed, min, max, format, flags);
    }

    public static Vector3 DragFloat(string label, Vector3 value, float speed = 1.0f, float min = 0.0f, float max = 0.0f, string format = "%.3f", SliderFlags flags = SliderFlags.None)
    {
        Provider.DragFloat(label, ref value, speed, min, max, format, flags);
        return value;
    }

    public static bool DragFloat(string label, ref Vector4 value, float speed = 1.0f, float min = 0.0f, float max = 0.0f, string format = "%.3f", SliderFlags flags = SliderFlags.None)
    {
        return Provider.DragFloat(label, ref value, speed, min, max, format, flags);
    }

    public static Vector4 DragFloat(string label, Vector4 value, float speed = 1.0f, float min = 0.0f, float max = 0.0f, string format = "%.3f", SliderFlags flags = SliderFlags.None)
    {
        Provider.DragFloat(label, ref value, speed, min, max, format, flags);
        return value;
    }
}