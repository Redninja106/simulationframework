using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.IMGUI;

/// <summary>
/// Provides Immediate mode GUI Functionality to the application.
/// </summary>
public static class ImGui
{
    internal static IImGuiProvider Provider => Simulation.Current.GetComponent<IImGuiProvider>();

    /// <summary>
    /// Pushes a window to the stack and starts appending widgets to it. 
    /// Call <see cref="EndWindow"/> to stop appending to the window.
    /// </summary>
    /// <param name="title">The title of the window.</param>
    /// <param name="flags">A set of <see cref="WindowFlags"/> value which effect the behavior of the window.</param>
    /// <returns><see langword="false"/> if the window is collapsed or occluded, meaning that none of its content will be visible. Otherwise, <see langword="true"/>.</returns>
    public static bool BeginWindow(string title, WindowFlags flags = 0)
    {
        return BeginWindow(title, ref Unsafe.NullRef<bool>(), flags);
    }

    /// <summary>
    /// Pushes a window to the stack and starts appending widgets to it. 
    /// Call <see cref="EndWindow"/> to stop appending to the window.
    /// </summary>
    /// <param name="title">The title of the window.</param>
    /// <param name="open">The window's opened/closed state.</param>
    /// <param name="flags">A set of <see cref="WindowFlags"/> value which effect the behavior of the window.</param>
    /// <returns><see langword="false"/> if the window is collapsed or occluded, meaning that none of its content will be visible. Otherwise, <see langword="true"/>.</returns>
    public static bool BeginWindow(string title, ref bool open, WindowFlags flags = 0)
    {
        return Provider.BeginWindow(title, ref open, flags);
    }

    /// <summary>
    /// Pops the latest window from the stack.
    /// </summary>
    public static void EndWindow() => Provider.EndWindow();

    /// <summary>
    /// A text widget with the provided text.
    /// </summary>
    /// <param name="text">The text of the widget.</param>
    public static void Text(string text) => Provider.TextUnformatted(text);
    public static void Text(object obj) => Provider.TextUnformatted(obj.ToString());

    public static bool Button(string label) => Button(label, default);
    public static bool Button(string label, Vector2 size) => Provider.Button(label, size);
    public static bool SmallButton(string label) => Provider.SmallButton(label);
    public static void Image(ISurface surface, Vector2 size) => Image(surface, size, (0, 0), (1, 1));
    public static void Image(ISurface surface, Vector2 size, Vector2 uv0, Vector2 uv1) => Image(surface, size, uv0, uv1, Color.White, Color.Black);
    public static void Image(ISurface surface, Vector2 size, Color tintColor, Color borderColor) => Image(surface, size, (0, 0), (1, 1), tintColor, borderColor);
    public static void Image(ISurface surface, Vector2 size, Vector2 uv0, Vector2 uv1, Color tintColor, Color borderColor) => Provider.Image(surface, size, uv0, uv1, tintColor, borderColor);
    public static bool CheckBox(string label, ref bool value) => Provider.CheckBox(label, ref value);
    public static bool RadioButton(string label, bool active) => Provider.RadioButton(label, active);
    public static void ProgressBar(float completion, Vector2 size, string overlay) => Provider.ProgressBar(completion, size, overlay);
    public static void Bullet() => Provider.Bullet();


    /// <summary>
    /// A 1-dimensional floating-point slider widget. CTRL+Click to turn it into an input box (unless <see cref="SliderFlags.AlwaysClamp"/> is specified).
    /// </summary>
    /// <param name="label">The slider's label.</param>
    /// <param name="value">A reference to the value which the slider should display.</param>
    /// <param name="speed">The amount which the slider should change for each pixel the mouse moves while dragging.</param>
    /// <param name="min">The minimum value the slider will allow. This can be overridden using CTRL+Click unless the <see cref="SliderFlags.AlwaysClamp"/> flag is specified.</param>
    /// <param name="max">The maximum value the slider will allow. This can be overridden using CTRL+Click unless the <see cref="SliderFlags.AlwaysClamp"/> flag is specified.</param>
    /// <param name="format">The C-Style format string to use to display the value of the slider.</param>
    /// <param name="flags">A set of <see cref="SliderFlags"/> value which effect the behavior of the slider.</param>
    /// <returns><see langword="true"/> if the slider's <paramref name="value"/> was modified, otherwise <see langword="false"/>.</returns>
    public unsafe static bool DragFloat(string label, ref float value, float speed = 1.0f, float min = 0.0f, float max = 0.0f, string format = "%.3f", SliderFlags flags = SliderFlags.None)
    {
        return Provider.DragScalar(label, new Span<float>((float*)Unsafe.AsPointer(ref value), 1), speed, min, max, format, flags);
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
        DragFloat(label, ref value, speed, min, max, format, flags);
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
    public unsafe static bool DragFloat(string label, ref Vector2 value, float speed = 1.0f, float min = 0.0f, float max = 0.0f, string format = "%.3f", SliderFlags flags = SliderFlags.None)
    {
        return Provider.DragScalar(label, new Span<float>((float*)Unsafe.AsPointer(ref value), 2), speed, min, max, format, flags);
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
        DragFloat(label, ref value, speed, min, max, format, flags);
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
    public unsafe static bool DragFloat(string label, ref Vector3 value, float speed = 1.0f, float min = 0.0f, float max = 0.0f, string format = "%.3f", SliderFlags flags = SliderFlags.None)
    {
        return Provider.DragScalar(label, new Span<float>((float*)Unsafe.AsPointer(ref value), 3), speed, min, max, format, flags);
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
        DragFloat(label, ref value, speed, min, max, format, flags);
        return value;
    }
    /// <summary>
    /// A 4-dimensional floating-point slider widget. CTRL+Click to turn it into an input box (unless <see cref="SliderFlags.AlwaysClamp"/> is specified).
    /// </summary>
    /// <param name="label">The label of the slider.</param>
    /// <param name="value">A reference to the value which the slider should display.</param>
    /// <param name="speed">The change in value of the slider for every pixel.</param>
    /// <param name="min">The minimum value the slider will allow for each component. This can be overridden using CTRL+Click unless the <see cref="SliderFlags.AlwaysClamp"/> flag is specified.</param>
    /// <param name="max">The maximum value the slider will allow for each component. This can be overridden using CTRL+Click unless the <see cref="SliderFlags.AlwaysClamp"/> flag is specified.</param>
    /// <param name="format">The C-Style format string to use to display each value of the slider.</param>
    /// <param name="flags">A <see cref="SliderFlags"/> value which effects the behavior of the slider.</param>
    /// <returns>The new value of the slider. This may be the same as <paramref name="value"/> if the slider was not modified. To see if the slider was modified, use <see cref="IsItemEdited"/>.</returns>
    public unsafe static bool DragFloat(string label, ref Vector4 value, float speed = 1.0f, float min = 0.0f, float max = 0.0f, string format = "%.3f", SliderFlags flags = SliderFlags.None)
    {
        return Provider.DragScalar(label, new Span<float>((float*)Unsafe.AsPointer(ref value), 4), speed, min, max, format, flags);
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
        DragFloat(label, ref value, speed, min, max, format, flags);
        return value;
    }
    public unsafe static bool DragInt(string label, int value, int speed = 1, int min = 0, int max = 0, string format = "%d", SliderFlags flags = SliderFlags.None) => DragScalar(label, ref value, speed, min, max, format, flags);
    public unsafe static bool DragScalar<T>(string label, ref T value, T speed = default, T min = default, T max = default, string format = null, SliderFlags flags = SliderFlags.None) where T : unmanaged
    {
        return DragScalar(label, new Span<T>(Unsafe.AsPointer(ref value), 1), speed, min, max, format, flags);
    }
    public unsafe static bool DragScalar<T>(string label, Span<T> values, T speed = default, T min = default, T max = default, string format = null, SliderFlags flags = SliderFlags.None) where T : unmanaged
    {
        return Provider.DragScalar(label, values, speed, min, max, format, flags);
    }


    /// <summary>
    /// A 1-dimensional floating-point slider widget. CTRL+Click to turn it into an input box (unless <see cref="SliderFlags.AlwaysClamp"/> is specified).
    /// </summary>
    /// <param name="label">The slider's label.</param>
    /// <param name="value">A reference to the value which the slider should display.</param>
    /// <param name="speed">The amount which the slider should change for each pixel the mouse moves while dragging.</param>
    /// <param name="min">The minimum value the slider will allow. This can be overridden using CTRL+Click unless the <see cref="SliderFlags.AlwaysClamp"/> flag is specified.</param>
    /// <param name="max">The maximum value the slider will allow. This can be overridden using CTRL+Click unless the <see cref="SliderFlags.AlwaysClamp"/> flag is specified.</param>
    /// <param name="format">The C-Style format string to use to display the value of the slider.</param>
    /// <param name="flags">A set of <see cref="SliderFlags"/> value which effect the behavior of the slider.</param>
    /// <returns><see langword="true"/> if the slider's <paramref name="value"/> was modified, otherwise <see langword="false"/>.</returns>
    public unsafe static bool SliderFloat(string label, ref float value, float min = 0.0f, float max = 0.0f, string format = "%.3f", SliderFlags flags = SliderFlags.None) => Provider.SliderScalar(label, new Span<float>((float*)Unsafe.AsPointer(ref value), 1), min, max, format, flags);
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
    public static float SliderFloat(string label, float value, float min = 0.0f, float max = 0.0f, string format = "%.3f", SliderFlags flags = SliderFlags.None)
    {
        SliderFloat(label, ref value, min, max, format, flags);
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
    public unsafe static bool SliderFloat(string label, ref Vector2 value, float min = 0.0f, float max = 0.0f, string format = "%.3f", SliderFlags flags = SliderFlags.None) => Provider.SliderScalar(label, new Span<float>((float*)Unsafe.AsPointer(ref value), 2), min, max, format, flags);
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
    public static Vector2 SliderFloat(string label, Vector2 value, float min = 0.0f, float max = 0.0f, string format = "%.3f", SliderFlags flags = SliderFlags.None)
    {
        SliderFloat(label, ref value, min, max, format, flags);
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
    public unsafe static bool SliderFloat(string label, ref Vector3 value, float min = 0.0f, float max = 0.0f, string format = "%.3f", SliderFlags flags = SliderFlags.None) => Provider.SliderScalar(label, new Span<float>((float*)Unsafe.AsPointer(ref value), 3), min, max, format, flags);
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
    public static Vector3 SliderFloat(string label, Vector3 value, float min = 0.0f, float max = 0.0f, string format = "%.3f", SliderFlags flags = SliderFlags.None)
    {
        SliderFloat(label, ref value, min, max, format, flags);
        return value;
    }
    /// <summary>
    /// A 4-dimensional floating-point slider widget. CTRL+Click to turn it into an input box (unless <see cref="SliderFlags.AlwaysClamp"/> is specified).
    /// </summary>
    /// <param name="label">The label of the slider.</param>
    /// <param name="value">A reference to the value which the slider should display.</param>
    /// <param name="speed">The change in value of the slider for every pixel.</param>
    /// <param name="min">The minimum value the slider will allow for each component. This can be overridden using CTRL+Click unless the <see cref="SliderFlags.AlwaysClamp"/> flag is specified.</param>
    /// <param name="max">The maximum value the slider will allow for each component. This can be overridden using CTRL+Click unless the <see cref="SliderFlags.AlwaysClamp"/> flag is specified.</param>
    /// <param name="format">The C-Style format string to use to display each value of the slider.</param>
    /// <param name="flags">A <see cref="SliderFlags"/> value which effects the behavior of the slider.</param>
    /// <returns>The new value of the slider. This may be the same as <paramref name="value"/> if the slider was not modified. To see if the slider was modified, use <see cref="IsItemEdited"/>.</returns>
    public unsafe static bool SliderFloat(string label, ref Vector4 value, float min = 0.0f, float max = 0.0f, string format = "%.3f", SliderFlags flags = SliderFlags.None) => Provider.SliderScalar(label, new Span<float>((float*)Unsafe.AsPointer(ref value), 4), min, max, format, flags);
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
    public static Vector4 SliderFloat(string label, Vector4 value, float min = 0.0f, float max = 0.0f, string format = "%.3f", SliderFlags flags = SliderFlags.None)
    {
        SliderFloat(label, ref value, min, max, format, flags);
        return value;
    }
    public unsafe static bool SliderInt(string label, int value, int min = 0, int max = 0, string format = "%d", SliderFlags flags = SliderFlags.None) => SliderScalar(label, ref value, min, max, format, flags);
    public unsafe static bool SliderScalar<T>(string label, ref T value, T min = default, T max = default, string format = null, SliderFlags flags = SliderFlags.None) where T : unmanaged
    {
        return SliderScalar(label, new Span<T>(Unsafe.AsPointer(ref value), 1), min, max, format, flags);
    }
    public unsafe static bool SliderScalar<T>(string label, Span<T> values, T min = default, T max = default, string format = null, SliderFlags flags = SliderFlags.None) where T : unmanaged
    {
        return Provider.SliderScalar(label, values, min, max, format, flags);
    }


    public static bool BeginMenuBar() => Provider.BeginMenuBar();
    public static void EndMenuBar() => Provider.EndMenuBar();
    public static bool BeginMainMenuBar() => Provider.BeginMainMenuBar();
    public static void EndMainMenuBar() => Provider.EndMainMenuBar();
    public static bool BeginMenu(string label, bool enabled) => Provider.BeginMenu(label, enabled);
    public static void EndMenu() => Provider.EndMenu();
    public static bool MenuItem(string label, string shortcut = "", bool selected = false, bool enabled = true) => Provider.MenuItem(label, shortcut, selected, enabled);

    public static bool TreeNode(string label, TreeNodeFlags flags = 0) => Provider.TreeNode(label, flags);
    public static void TreePush(string id) => Provider.TreePush(id);
    public static void TreePop() => Provider.TreePop();

    public static void Separator() => Provider.Separator();
    public static void SameLine(float offsetFromStartX = 0, float spacing = -1) => Provider.SameLine(offsetFromStartX, spacing);
    public static void NewLine() => Provider.NewLine();
    public static void Spacing() => Provider.Spacing();
    public static void Dummy(Vector2 size = default) => Provider.Dummy(size);
    public static void Indent(float width = 0) => Provider.Indent(width);
    public static void Unindent(float width = 0) => Provider.Unindent(width);
    public static void BeginGroup() => Provider.BeginGroup();
    public static void EndGroup() => Provider.EndGroup();
    public static bool IsItemHovered(HoveredFlags flags = HoveredFlags.None) => Provider.IsItemHovered(flags);
    public static bool IsItemClicked(MouseButton button = MouseButton.Left) => Provider.IsItemClicked(button);
    public static bool IsItemEdited() => Provider.IsItemEdited();
    public static bool BeginListBox(string label, Vector2 size) => Provider.BeginListBox(label, size);
    public static void EndListBox() => Provider.EndListBox();
    public static bool ListBox(string label, ref int currentItem, Span<string> items, int heightInItems = -1) => Provider.ListBox(label, ref currentItem, items, heightInItems);
    public static bool ListBox<T>(string label, ref int currentItem, Func<int, string> itemsGetter, int itemCount, int heightInItems = -1) => Provider.ListBox<T>(label, ref currentItem, itemsGetter, itemCount, heightInItems);
    public static bool Selectable(string label, bool selected = false, SelectableFlags flags = 0, Vector2 size = default) => Provider.Selectable(label, selected, flags, size);
    public static bool Selectable(string label, ref bool selected, SelectableFlags flags = 0, Vector2 size = default) => Provider.Selectable(label, ref selected, flags, size);
    public static bool BeginChild(string id, Vector2 size = default, bool border = false, WindowFlags flags = 0) => Provider.BeginChild(id, size, border, flags);
    public static void EndChild() => Provider.EndChild();

    public static bool BeginPopup(string id, WindowFlags flags = 0) => Provider.BeginPopup(id, flags);
    public static bool BeginPopupModal(string name, WindowFlags flags = 0) => Provider.BeginPopupModal(name, flags);
    public static bool BeginPopupModal(string name, ref bool open, WindowFlags flags = 0) => Provider.BeginPopupModal(name, ref open, flags);
    public static void EndPopup() => Provider.EndPopup();

    public static bool BeginCombo(string label, string previewValue, ComboFlags flags = 0) => Provider.BeginCombo(label, previewValue, flags);
    public static void EndCombo() => Provider.EndCombo();

    public static void OpenPopup(string id, PopupFlags flags = PopupFlags.None) => Provider.OpenPopup(id, flags);
    public static void CloseCurrentPopup() => Provider.CloseCurrentPopup();
}