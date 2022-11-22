using ImGuiNET;
using SimulationFramework.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.ImGuiNET;

internal struct ImGuiVertexShader : IShader
{
    // https://github.com/ocornut/imgui/blob/588421986784df1ae3df16305d90cecdb07e9951/backends/imgui_impl_dx11.cpp#L384

    [ShaderUniform]
    public Matrix4x4 ProjectionMatrix;

    [ShaderInput]
    Vector2 position;

    [ShaderInput]
    Vector2 uv;

    [ShaderInput]
    Color color;

    [ShaderOutput]
    ColorF outColor;

    [ShaderOutput]
    Vector2 outUV;

    [ShaderOutput(OutputSemantic.Position)]
    Vector4 outPosition;

    public void Main()
    {
        outPosition = Vector4.Transform(new Vector4(position, 0, 1), ProjectionMatrix);
        outColor = color.ToColorF();
        outUV = uv;
    }
}
