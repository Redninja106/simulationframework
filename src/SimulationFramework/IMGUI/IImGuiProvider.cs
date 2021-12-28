using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.IMGUI;

public interface IImGuiProvider : ISimulationComponent
{
    // Windowing
    bool BeginWindow(string title, ref bool open, WindowFlags flags);
    void EndWindow();

    // widgets
    void TextUnformatted(string text);

    bool Button(string label, Vector2 size);
    void Image(ISurface surface, Vector2 size, Vector2 uv0, Vector2 uv1, Color tintColor, Color borderColor);
    bool CheckBox(string label, ref bool value);
    bool CheckBoxFlags(string label, ref uint value, uint flag);
    bool RadioButton(string label, bool active);
    bool RadioButtonFlags(string label, ref uint value, uint flag);
    void ProgressBar(float completion, Vector2 size, string overlay);
    void Bullet();

    bool DragScalar<T>(string label, Span<T> values, T speed, T min, T max, string format, SliderFlags flags) where T : unmanaged;
    bool SliderScalar<T>(string label, Span<T> values, T min, T max, string format, SliderFlags flags) where T : unmanaged;
    bool InputScalar<T>(string label, Span<T> values, T step, T stepFast, string format, InputTextFlags flags) where T : unmanaged;

    bool InputText(string label, ref string value, int maxSize, InputTextFlags flags);

    bool ColorEdit(string label, ref Color color, ColorEditFlags flags);
    bool ColorPicker(string label, ref Color color, ColorEditFlags flags);

    bool TreeNode(string label, TreeNodeFlags flags);
    void TreePush(string id);
    void TreePop();

    bool BeginMenuBar();
    void EndMenuBar();
    bool BeginMainMenuBar();
    void EndMainMenuBar();
    bool BeginMenu(string label, bool enabled);
    void EndMenu();
    bool MenuItem(string label, string shortcut, bool selected, bool enabled);

    void BeginTooltip();
    void EndToolTip();

    // formatting
    void Separator();
    void SameLine(float offsetFromStartX, float spacing);
    void NewLine();
    void Spacing();
    void Dummy(Vector2 size);
    void Indent(float width);
    void Unindent(float width);
    void BeginGroup();
    void EndGroup();
    bool IsItemHovered(HoveredFlags flags);
    bool IsItemClicked(MouseButton button);
    bool IsItemEdited();
}