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

    public static bool Button(string label) => Button(label, default);
    public static bool Button(string label, Vector2 size) => Provider.Button(label, size);
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


}