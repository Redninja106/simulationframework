using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;
using static ImGuiNET.ImGuiNative;

namespace SimulationFramework;

/// <summary>
/// Basic ImGui provider which user ImGui.NET's ImGuiNative api.
/// </summary>
public unsafe sealed class ImGuiNETProvider : IImGuiProvider
{
    private readonly IImGuiBackend backend;

    public ImGuiNETProvider(IImGuiBackend backend)
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

    public unsafe void TextUnformatted(string text)
    {
        igText(MarshalText(text, stackalloc byte[text.Length + 1]));
    }

    private unsafe byte* MarshalText(string text, Span<byte> buffer)
    {
        Encoding.UTF8.GetBytes(text.AsSpan(), buffer);
        fixed (byte* ptr = buffer)
        {
            return ptr;
        }
    }

    public bool IsItemEdited()
    {
        return igIsItemEdited() != 0;
    }

    public bool DragFloat(string label, ref float value, float speed, float min, float max, string format, SliderFlags flags)
    {
        return 0 != igDragFloat(MarshalText(label, stackalloc byte[label.Length + 1]), (float*)Unsafe.AsPointer(ref value), speed, min, max, MarshalText(format, stackalloc byte[format.Length + 1]), (ImGuiSliderFlags)flags);
    }

    public bool DragFloat(string label, ref Vector2 value, float speed, float min, float max, string format, SliderFlags flags)
    {
        return 0 != igDragFloat2(MarshalText(label, stackalloc byte[label.Length + 1]), (System.Numerics.Vector2*)Unsafe.AsPointer(ref value), speed, min, max, MarshalText(format, stackalloc byte[format.Length + 1]), (ImGuiSliderFlags)flags);
    }

    public bool DragFloat(string label, ref Vector3 value, float speed, float min, float max, string format, SliderFlags flags)
    {
        return 0 != igDragFloat3(MarshalText(label, stackalloc byte[label.Length + 1]), (System.Numerics.Vector3*)Unsafe.AsPointer(ref value), speed, min, max, MarshalText(format, stackalloc byte[format.Length + 1]), (ImGuiSliderFlags)flags);
    }

    public bool DragFloat(string label, ref Vector4 value, float speed, float min, float max, string format, SliderFlags flags)
    {
        return 0 != igDragFloat4(MarshalText(label, stackalloc byte[label.Length + 1]), (System.Numerics.Vector4*)Unsafe.AsPointer(ref value), speed, min, max, MarshalText(format, stackalloc byte[format.Length + 1]), (ImGuiSliderFlags)flags);
    }

    public bool Button(string label, Vector2 size)
    {
        return 0 != igButton(MarshalText(label, stackalloc byte[label.Length + 1]), size);
    }

    public void Image(ISurface surface, Vector2 size, Vector2 uv0, Vector2 uv1, Color tintColor, Color borderColor)
    {
        var id = backend.GetTextureID(surface);

        igImage(id, size, uv0, uv1, tintColor.ToVector4(), borderColor.ToVector4());
    }
}