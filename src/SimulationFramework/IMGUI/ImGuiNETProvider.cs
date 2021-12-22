using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;
using static ImGuiNET.ImGuiNative;

namespace SimulationFramework.IMGUI;

/// <summary>
/// Basic ImGui provider which user ImGui.NET's ImGuiNative api.
/// </summary>
public unsafe sealed class ImGuiNETProvider : IImGuiProvider
{
    private readonly IImGuiNETBackend backend;

    public ImGuiNETProvider(IImGuiNETBackend backend)
    {
        this.backend = backend;

        unsafe
        {
            igCreateContext(null);
        }
    }

    public void Apply(Simulation simulation)
    {
        simulation.BeforeRender += BeforeRender;
        simulation.AfterRender += AfterRender;
    }

    private static ImGuiDataType GetDataType<T>() where T : unmanaged
    {
        if (typeof(T) == typeof(sbyte)) return ImGuiDataType.S8;
        if (typeof(T) == typeof(short)) return ImGuiDataType.S16;
        if (typeof(T) == typeof(int)) return ImGuiDataType.S32;
        if (typeof(T) == typeof(long)) return ImGuiDataType.S64;
        if (typeof(T) == typeof(byte)) return ImGuiDataType.U8;
        if (typeof(T) == typeof(ushort)) return ImGuiDataType.U16;
        if (typeof(T) == typeof(uint)) return ImGuiDataType.U32;
        if (typeof(T) == typeof(ulong)) return ImGuiDataType.U64;
        if (typeof(T) == typeof(float)) return ImGuiDataType.Float;
        if (typeof(T) == typeof(double)) return ImGuiDataType.Double;
        throw new NotSupportedException("This type is not supported!");
    }

    private static float AsFloat<T>(ImGuiDataType dataType, T value) where T : unmanaged
    {
        if (value is sbyte) return *(sbyte*)&value;
        if (value is short) return *(short*)&value;
        if (value is int) return *(int*)&value;
        if (value is long) return *(long*)&value;
        if (value is byte) return *(byte*)&value;
        if (value is ushort) return *(ushort*)&value;
        if (value is uint) return *(uint*)&value;
        if (value is ulong) return *(ulong*)&value;
        if (value is float) return *(float*)&value;
        if (value is double) return (float)*(double*)&value;
        throw new NotSupportedException("This type is not supported!");
    }

    private void AfterRender()
    {
        backend.Render();
    }

    private void BeforeRender()
    {
        backend.NewFrame();
    }

    public void Dispose()
    {
        backend.Dispose();
    }

    private unsafe byte* MarshalText(string text, Span<byte> buffer)
    {
        Encoding.UTF8.GetBytes(text.AsSpan(), buffer);
        fixed (byte* ptr = buffer)
        {
            return ptr;
        }
    }

    public unsafe void TextUnformatted(string text)
    {
        igText(MarshalText(text, stackalloc byte[text.Length + 1]));
    }

    public bool IsItemEdited() => 1 == igIsItemEdited();
    public bool Button(string label, Vector2 size) => 1 == igButton(MarshalText(label, stackalloc byte[label.Length + 1]), size);
    public void Image(ISurface surface, Vector2 size, Vector2 uv0, Vector2 uv1, Color tintColor, Color borderColor)
    {
        var id = backend.GetTextureID(surface);
        igImage(id, size, uv0, uv1, tintColor.ToVector4(), borderColor.ToVector4());
    }
    public bool BeginWindow(string title, ref bool open, WindowFlags flags) => 1 == igBegin(MarshalText(title, stackalloc byte[title.Length + 1]), (byte*)Unsafe.AsPointer(ref open), (ImGuiWindowFlags)flags);
    public void EndWindow() => igEnd();
    public bool CheckBox(string label, ref bool value) => 1 == igCheckbox(MarshalText(label, stackalloc byte[label.Length + 1]), (byte*)Unsafe.AsPointer(ref value));
    public bool CheckBoxFlags(string label, ref uint value, uint flag) => 1 == igCheckboxFlags(MarshalText(label, stackalloc byte[label.Length + 1]), (uint*)Unsafe.AsPointer(ref value), flag);
    public bool RadioButton(string label, bool active) => 1 == igRadioButtonBool(MarshalText(label, stackalloc byte[label.Length + 1]), (byte)(active ? 1 : 0));
    public bool RadioButtonFlags(string label, ref uint value, uint flag) => 1 == igRadioButtonIntPtr(MarshalText(label, stackalloc byte[label.Length + 1]), (int*)Unsafe.AsPointer(ref value), *(int*)&flag);
    public void ProgressBar(float fraction, Vector2 size, string overlay) => igProgressBar(fraction, size, MarshalText(overlay, stackalloc byte[overlay.Length + 1]));
    public void Bullet() => igBullet();
    public bool DragScalar<T>(string label, Span<T> values, T speed, T min, T max, string format, SliderFlags flags) where T : unmanaged
    {
        fixed (T* pValues = values)
        {
            return 0 != igDragScalarN(MarshalText(label, stackalloc byte[label.Length + 1]), GetDataType<T>(), pValues, values.Length, AsFloat(GetDataType<T>(), speed), &min, &max, MarshalText(format, stackalloc byte[format.Length + 1]), (ImGuiSliderFlags)flags);
        }
    }
    public bool SliderScalar<T>(string label, Span<T> values, T min, T max, string format, SliderFlags flags) where T : unmanaged
    {
        fixed (T* pValues = values)
        {
            return 0 != igSliderScalarN(MarshalText(label, stackalloc byte[label.Length + 1]), GetDataType<T>(), pValues, values.Length, &min, &max, MarshalText(format, stackalloc byte[format.Length + 1]), (ImGuiSliderFlags)flags);
        }
    }
    public bool InputScalar<T>(string label, Span<T> values, T step, T stepFast, string format, InputTextFlags flags) where T : unmanaged
    {
        fixed (T* pValues = values)
        {
            return 0 != igInputScalarN(MarshalText(label, stackalloc byte[label.Length + 1]), GetDataType<T>(), pValues, values.Length, &step, &stepFast, MarshalText(format, stackalloc byte[format.Length + 1]), (ImGuiInputTextFlags)flags);
        }
    }
    public bool InputText(string label, ref string value, int maxSize, InputTextFlags flags)
    {
        byte* buf = MarshalText(value, stackalloc byte[maxSize]);
        bool result = 1 == igInputText(MarshalText(label, stackalloc byte[label.Length + 1]), buf, (uint)maxSize, (ImGuiInputTextFlags)flags, null, null);
        value = Encoding.UTF8.GetString(buf, maxSize);
        return result;
    }
    public bool ColorEdit(string label, ref Color color, ColorEditFlags flags)
    {
        var col4 = color.ToVector4();
        var result = 1 == igColorEdit4(MarshalText(label, stackalloc byte[label.Length + 1]), (System.Numerics.Vector4*)&col4, (ImGuiColorEditFlags)flags);
        color = new Color(col4);
        return result;
    }
    public bool ColorPicker(string label, ref Color color, ColorEditFlags flags)
    {
        var col4 = color.ToVector4();
        var result = 1 == igColorPicker4(MarshalText(label, stackalloc byte[label.Length + 1]), (System.Numerics.Vector4*)&col4, (ImGuiColorEditFlags)flags, null);
        color = new Color(col4);
        return result;
    }
    public bool TreeNode(string label, TreeNodeFlags flags) => 1 == igTreeNodeExStr(MarshalText(label, stackalloc byte[label.Length + 1]), (ImGuiTreeNodeFlags)flags);
    public void TreePush(string id) => igTreePushStr(MarshalText(id, stackalloc byte[id.Length + 1]));
    public void TreePop() => igTreePop();
    public bool BeginMenuBar() => 1 == igBeginMenuBar();
    public void EndMenuBar() => igEndMenuBar();
    public bool BeginMainMenuBar() => 1 == igBeginMainMenuBar();
    public void EndMainMenuBar() => igEndMainMenuBar();
    public bool BeginMenu(string label, bool enabled) => 1 == igBeginMenu(MarshalText(label, stackalloc byte[label.Length + 1]), enabled ? (byte)1: (byte)0);
    public void EndMenu() => igEndMenu();
    public bool MenuItem(string label, string shortcut, bool selected, bool enabled) 
        => 1 == igMenuItemBool(
            MarshalText(label, stackalloc byte[label.Length + 1]),
            MarshalText(shortcut, stackalloc byte[shortcut.Length + 1]),
            selected ? (byte)1 : (byte)0, 
            enabled ? (byte)1 : (byte)0
            );
    public void BeginTooltip() => igBeginTooltip();
    public void EndToolTip() => igEndTooltip();
    public void Separator() => igSeparator();
    public void SameLine(float offsetFromStartX, float spacing) => igSameLine(offsetFromStartX, spacing);
    public void NewLine() => igNewLine();
    public void Spacing() => igSpacing(); 
    public void Dummy(Vector2 size) => igDummy(size);
    public void Indent(float width) => igIndent(width);
    public void Unindent(float width) => igUnindent(width);
    public void BeginGroup() => igBeginGroup();
    public void EndGroup() => igEndGroup();
    public bool IsItemHovered(HoveredFlags flags) => 1 == igIsItemHovered((ImGuiHoveredFlags)flags);
    public bool IsItemClicked(MouseButton button) => 1 == igIsItemClicked((ImGuiMouseButton)button);
}