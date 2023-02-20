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

    [Uniform]
    public Matrix4x4 ProjectionMatrix;

    [Input]
    Vector2 position;

    [Input]
    Vector2 uv;

    [Input]
    uint color;

    [Output]
    ColorF outColor;

    [Output]
    Vector2 outUV;

    [Output(OutputSemantic.Position)]
    Vector4 outPosition;

    public void Main()
    {
        outPosition = Vector4.Transform(new Vector4(position, 0, 1), ProjectionMatrix);
        ShaderIntrinsics.Hlsl(@"
        __output.outColor.b = ((__input.color & 0xFF000000) >> 24) / 255;
        __output.outColor.g = ((__input.color & 0x00FF0000) >> 16) / 255;
        __output.outColor.r = ((__input.color & 0x0000FF00) >> 8) / 255;
        __output.outColor.a = ((__input.color & 0x000000FF) >> 0) / 255;
"); 
        //outColor.R = ((color & 0xFF000000) >> 24) / 255f;
        //outColor.G = ((color & 0x00FF0000) >> 16) / 255f;
        //outColor.B = ((color & 0x0000FF00) >> 8) / 255f;
        //outColor.A = ((color & 0x000000FF) >> 0) / 255f;

        outUV = uv;
    }
}
