using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

/// <summary>
/// Provides Immediate mode GUI Functionality to the application.
/// </summary>
public static class ImGui
{
    internal static IImGuiProvider Provider => Simulation.Current.GetComponent<IImGuiProvider>();

    /// <summary>
    /// A text widget with the provided text.
    /// </summary>
    /// <param name="text">The text of the widget.</param>
    public static void Text(string text) => Provider.TextUnformatted(text);

    /// <summary>
    /// A 1-dimensional floating-point slider widget. CTRL+Click to turn it into an input box (unless <see cref="SliderFlags.AlwaysClamp"/> is specified).
    /// </summary>
    /// <param name="label">The slider's label.</param>
    /// <param name="value">A reference to the value which the slider should display.</param>
    /// <param name="speed">The amount which the slider should change for each pixel the mouse moves while dragging.</param>
    /// <param name="min">The minimum value the slider will allow. This can be overridden using CTRL+Click unless the <see cref="SliderFlags.AlwaysClamp"/> flag is specified.</param>
    /// <param name="max">The maximum value the slider will allow. This can be overridden using CTRL+Click unless the <see cref="SliderFlags.AlwaysClamp"/> flag is specified.</param>
    /// <param name="format">The C-Style format string to use to display the value of the slider.</param>
    /// <param name="flags">A <see cref="SliderFlags"/> value which effects the behavior of the slider.</param>
    /// <returns><see langword="true"/> if the slider's <paramref name="value"/> was modified, otherwise <see langword="false"/>.</returns>
    public static bool DragFloat(string label, ref float value, float speed = 1.0f, float min = 0.0f, float max = 0.0f, string format = "%.3f", SliderFlags flags = SliderFlags.None)
    {
        return Provider.DragFloat(label, ref value, speed, min, max, format, flags);
    }

    /// <summary>
    /// A 1-dimensional floating-point slider widget. CTRL+Click to turn it into an input box (unless <see cref="SliderFlags.AlwaysClamp"/> is specified).
    /// </summary>
    /// <param name="label">The label of the slider.</param>
    /// <param name="value">The value which the slider should display.</param>
    /// <param name="speed">The change in value of the slider for every pixel.</param>
    /// <param name="min">The minimum value the slider will allow. This can be overridden using CTRL+Click unless the <see cref="SliderFlags.AlwaysClamp"/> flag is specified.</param>
    /// <param name="max">The maximum value the slider will allow. This can be overridden using CTRL+Click unless the <see cref="SliderFlags.AlwaysClamp"/> flag is specified.</param>
    /// <param name="format">The C-Style format string to use to display the value of the slider.</param>
    /// <param name="flags">A <see cref="SliderFlags"/> value which effects the behavior of the slider.</param>
    /// <returns><see langword="true"/> if the slider's <paramref name="value"/> was modified, otherwise <see langword="false"/>.</returns>
    public static float DragFloat(string label, float value, float speed = 1.0f, float min = 0.0f, float max = 0.0f, string format = "%.3f", SliderFlags flags = SliderFlags.None)
    {
        Provider.DragFloat(label, ref value, speed, min, max, format, flags);
        return value;
    }

    /// <summary>
    /// A 2-dimensional floating-point slider widget. CTRL+Click to turn it into an input box (unless <see cref="SliderFlags.AlwaysClamp"/> is specified).
    /// </summary>
    /// <param name="label">The label of the slider.</param>
    /// <param name="value">A reference to the value which the slider should display.</param>
    /// <param name="speed">The change in value of the slider for every pixel.</param>
    /// <param name="min">The minimum value the slider will allow for each component. This can be overridden using CTRL+Click unless the <see cref="SliderFlags.AlwaysClamp"/> flag is specified.</param>
    /// <param name="max">The maximum value the slider will allow for each component. This can be overridden using CTRL+Click unless the <see cref="SliderFlags.AlwaysClamp"/> flag is specified.</param>
    /// <param name="format">The C-Style format string to use to display each value of the slider.</param>
    /// <param name="flags">A <see cref="SliderFlags"/> value which effects the behavior of the slider.</param>
    /// <returns>The new value of the slider. This may be the same as <paramref name="value"/> if the slider was not modified. To see if the slider was modified, use <see cref="IsItemEdited"/>.</returns>
    public static bool DragFloat(string label, ref Vector2 value, float speed = 1.0f, float min = 0.0f, float max = 0.0f, string format = "%.3f", SliderFlags flags = SliderFlags.None)
    {
        return Provider.DragFloat(label, ref value, speed, min, max, format, flags);
    }

    /// <summary>
    /// A 2-dimensional floating-point slider widget. CTRL+Click to turn it into an input box (unless <see cref="SliderFlags.AlwaysClamp"/> is specified).
    /// </summary>
    /// <param name="label">The label of the slider.</param>
    /// <param name="value">The value which the slider should display.</param>
    /// <param name="speed">The change in value of the slider for every pixel.</param>
    /// <param name="min">The minimum value the slider will allow for each component. This can be overridden using CTRL+Click unless the <see cref="SliderFlags.AlwaysClamp"/> flag is specified.</param>
    /// <param name="max">The maximum value the slider will allow for each component. This can be overridden using CTRL+Click unless the <see cref="SliderFlags.AlwaysClamp"/> flag is specified.</param>
    /// <param name="format">The C-Style format string to use to display each value of the slider.</param>
    /// <param name="flags">A <see cref="SliderFlags"/> value which effects the behavior of the slider.</param>
    /// <returns>The new value of the slider. This may be the same as <paramref name="value"/> if the slider was not modified. To see if the slider was modified, use <see cref="IsItemEdited"/>.</returns>
    public static Vector2 DragFloat(string label, Vector2 value, float speed = 1.0f, float min = 0.0f, float max = 0.0f, string format = "%.3f", SliderFlags flags = SliderFlags.None)
    {
        Provider.DragFloat(label, ref value, speed, min, max, format, flags);
        return value;
    }

    /// <summary>
    /// A 3-dimensional floating-point slider widget. CTRL+Click to turn it into an input box (unless <see cref="SliderFlags.AlwaysClamp"/> is specified).
    /// </summary>
    /// <param name="label">The label of the slider.</param>
    /// <param name="value">A reference to the value which the slider should display.</param>
    /// <param name="speed">The change in value of the slider for every pixel.</param>
    /// <param name="min">The minimum value the slider will allow for each component. This can be overridden using CTRL+Click unless the <see cref="SliderFlags.AlwaysClamp"/> flag is specified.</param>
    /// <param name="max">The maximum value the slider will allow for each component. This can be overridden using CTRL+Click unless the <see cref="SliderFlags.AlwaysClamp"/> flag is specified.</param>
    /// <param name="format">The C-Style format string to use to display each value of the slider.</param>
    /// <param name="flags">A <see cref="SliderFlags"/> value which effects the behavior of the slider.</param>
    /// <returns>The new value of the slider. This may be the same as <paramref name="value"/> if the slider was not modified. To see if the slider was modified, use <see cref="IsItemEdited"/>.</returns>
    public static bool DragFloat(string label, ref Vector3 value, float speed = 1.0f, float min = 0.0f, float max = 0.0f, string format = "%.3f", SliderFlags flags = SliderFlags.None)
    {
        return Provider.DragFloat(label, ref value, speed, min, max, format, flags);
    }

    /// <summary>
    /// A 3-dimensional floating-point slider widget. CTRL+Click to turn it into an input box (unless <see cref="SliderFlags.AlwaysClamp"/> is specified).
    /// </summary>
    /// <param name="label">The label of the slider.</param>
    /// <param name="value">The value which the slider should display.</param>
    /// <param name="speed">The change in value of the slider for every pixel.</param>
    /// <param name="min">The minimum value the slider will allow for each component. This can be overridden using CTRL+Click unless the <see cref="SliderFlags.AlwaysClamp"/> flag is specified.</param>
    /// <param name="max">The maximum value the slider will allow for each component. This can be overridden using CTRL+Click unless the <see cref="SliderFlags.AlwaysClamp"/> flag is specified.</param>
    /// <param name="format">The C-Style format string to use to display each value of the slider.</param>
    /// <param name="flags">A <see cref="SliderFlags"/> value which effects the behavior of the slider.</param>
    /// <returns>The new value of the slider. This may be the same as <paramref name="value"/> if the slider was not modified. To see if the slider was modified, use <see cref="IsItemEdited"/>.</returns>
    public static Vector3 DragFloat(string label, Vector3 value, float speed = 1.0f, float min = 0.0f, float max = 0.0f, string format = "%.3f", SliderFlags flags = SliderFlags.None)
    {
        Provider.DragFloat(label, ref value, speed, min, max, format, flags);
        return value;
    }

    /// <summary>
    /// A 3-dimensional floating-point slider widget. CTRL+Click to turn it into an input box (unless <see cref="SliderFlags.AlwaysClamp"/> is specified).
    /// </summary>
    /// <param name="label">The label of the slider.</param>
    /// <param name="value">A reference to the value which the slider should display.</param>
    /// <param name="speed">The change in value of the slider for every pixel.</param>
    /// <param name="min">The minimum value the slider will allow for each component. This can be overridden using CTRL+Click unless the <see cref="SliderFlags.AlwaysClamp"/> flag is specified.</param>
    /// <param name="max">The maximum value the slider will allow for each component. This can be overridden using CTRL+Click unless the <see cref="SliderFlags.AlwaysClamp"/> flag is specified.</param>
    /// <param name="format">The C-Style format string to use to display each value of the slider.</param>
    /// <param name="flags">A <see cref="SliderFlags"/> value which effects the behavior of the slider.</param>
    /// <returns>The new value of the slider. This may be the same as <paramref name="value"/> if the slider was not modified. To see if the slider was modified, use <see cref="IsItemEdited"/>.</returns>
    public static bool DragFloat(string label, ref Vector4 value, float speed = 1.0f, float min = 0.0f, float max = 0.0f, string format = "%.3f", SliderFlags flags = SliderFlags.None)
    {
        return Provider.DragFloat(label, ref value, speed, min, max, format, flags);
    }

    /// <summary>
    /// A 4-dimensional floating-point slider widget. CTRL+Click to turn it into an input box (unless <see cref="SliderFlags.AlwaysClamp"/> is specified).
    /// </summary>
    /// <param name="label">The label of the slider.</param>
    /// <param name="value">The value which the slider should display.</param>
    /// <param name="speed">The change in value of the slider for every pixel.</param>
    /// <param name="min">The minimum value the slider will allow for each component. This can be overridden using CTRL+Click unless the <see cref="SliderFlags.AlwaysClamp"/> flag is specified.</param>
    /// <param name="max">The maximum value the slider will allow for each component. This can be overridden using CTRL+Click unless the <see cref="SliderFlags.AlwaysClamp"/> flag is specified.</param>
    /// <param name="format">The C-Style format string to use to display each value of the slider.</param>
    /// <param name="flags">A <see cref="SliderFlags"/> value which effects the behavior of the slider.</param>
    /// <returns>The new value of the slider. This may be the same as <paramref name="value"/> if the slider was not modified. To see if the slider was modified, use <see cref="IsItemEdited"/>.</returns>
    public static Vector4 DragFloat(string label, Vector4 value, float speed = 1.0f, float min = 0.0f, float max = 0.0f, string format = "%.3f", SliderFlags flags = SliderFlags.None)
    {
        Provider.DragFloat(label, ref value, speed, min, max, format, flags);
        return value;
    }
}