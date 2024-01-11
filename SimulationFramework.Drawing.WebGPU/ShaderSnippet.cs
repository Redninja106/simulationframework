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
                new("pos : vec2f", VertexFormat.Float32x2),
                new("col : vec4f", VertexFormat.Float32x4),
            ],

            InterstageArgs = [
                new("color : vec4f")
            ],

            VertexShaderCode = @"
out.pos = 
",

        };
    }

    public string Functions { get; init; }
    public VertexShaderArg[] VertexShaderArgs { get; init; }
    public string VertexShaderCode { get; init; }
    public string[] FragmentShaderArgs { get; init; }
    public string FragmentShaderCode { get; init; }

    public string[] BindingDeclarations { get; init; } 

    public InterstageArg[] InterstageArgs { get; init; }

}

record VertexShaderArg(string Declaration, VertexFormat Format, int Offset = -1)
{
}

record InterstageArg(string Declaration, string? SpecialAttribute = null);