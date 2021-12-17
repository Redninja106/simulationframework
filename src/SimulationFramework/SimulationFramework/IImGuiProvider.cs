using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

internal interface IImGuiProvider : ISimulationComponent
{
    //void Separator();
    //void SameLine(float offsetFromStartX = 0.0f, float spacing = -1.0f);
    //void NewLine();
    //void Spacing();
    //void Indent(float width = 0.0f);
    //void Unindent(float width = 0.0f);


    void TextUnformatted(string text);

    bool Button(string label, Vector2 size);
    void Image(ISurface surface, Vector2 size, Vector2 uv0, Vector2 uv1, Color tintColor, Color borderColor);


    bool IsItemEdited();

    bool DragFloat(string label, ref float value, float speed, float min, float max, string format, SliderFlags flags);
    bool DragFloat(string label, ref Vector2 value, float speed, float min, float max, string format, SliderFlags flags);
    bool DragFloat(string label, ref Vector3 value, float speed, float min, float max, string format, SliderFlags flags);
    bool DragFloat(string label, ref Vector4 value, float speed, float min, float max, string format, SliderFlags flags);
}