using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebGPU;

namespace SimulationFramework.Drawing.WebGPU;
internal class ShaderSnippet
{
    ShaderSnippet()
    {
        ShaderSnippet s = new()
        {
            VertexShaderArgs = [
                ("pos : vec2f", new VertexAttribute(VertexFormat.Float32x2, 0, 0)),
                ("col: vec4f", new()),
            ]
        };

    }

    public string Functions { get; }
    public (string, VertexAttribute)[] VertexShaderArgs { get; init; }
    public string VertexShaderCode { get; }
    public string[] FragmentShaderArgs { get; }
    public string FragmentShaderCode { get; }

    public string[] InterstageVariables { get; }
}