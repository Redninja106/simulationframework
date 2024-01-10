using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebGPU;

namespace SimulationFramework.Drawing.WebGPU;
internal class ShaderAssembler
{
    public ShaderModule GetShaderModule()
    {
        throw new NotImplementedException();
    }



    public ShaderPart? MaskFunc { get; }


    private string GetSource()
    {
        return $@"
struct UniformData {{
    viewportMatrix: mat4x4f
}}

@group(0) @binding(0) var<uniform> uniforms : UniformData;

struct VsOut {{
    @builtin(position) position : vec4f,
    @location(0) color : vec4f
}}

@vertex
fn vs_main(@location(0) position : vec2f, @location(1) color : vec4f) -> VsOut {{
    var out: VsOut;
    var pos = vec4f(position.xy, 0f, 1f);
    pos = uniforms.viewportMatrix * pos;
    out.position = pos;
    out.color = color;

    return out;
}}

@fragment
fn fs_main(vsOut: VsOut) -> @location(0) vec4f {{
    return vsOut.color;
}}
";
    }
}
