﻿using ImGuiNET;
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

    [Input(InputSemantic.Vertex)]
    Vector2 position;

    [Input(InputSemantic.Vertex)]
    Vector2 uv;

    [Input(InputSemantic.Vertex, SourceType = typeof(Color))]
    ColorF color;

    [Output]
    ColorF outColor;

    [Output]
    Vector2 outUV;

    [Output(OutputSemantic.Position)]
    Vector4 outPosition;

    public void Main()
    {
        outPosition = Vector4.Transform(new Vector4(position, 0, 1), ProjectionMatrix);
        outColor = color;
        outUV = uv;
    }
}
