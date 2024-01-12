using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WebGPU;

namespace SimulationFramework.Drawing.WebGPU.Renderers;
internal class ArcRenderer : Renderer
{
    RectangleRenderer rectRenderer;
    BufferWriter<Arc> arcs;

    public ArcRenderer(GraphicsResources resources, WebGPUCanvas canvas) : base(resources, canvas)
    {
        var shaderModule = resources.Device.CreateShaderModule(new()
        {
            NextInChain = new ShaderModuleWGSLDescriptor(@"
struct Arc {
    position : vec2f
    size : vec2f
    begin : f32
    end : f32
}

struct VsOut {
    @builtin(position) position: vec4f
    @location(0) arc: Arc
}

@group(0) @binding(0) var<storage, read> arcs: array<Arc>;

@vertex 
fn vs_main(@builtin(instance_index) instanceIndex : u32, @builtin(vertex_index) vertexIndex : u32) -> VsOut 
{
    let arc = arcs[instanceIndex];
    
    let x = vertexIndex % 2f;
    let y = (f32)(vertexIndex == 0 || vertexIndex==1 || vertexIndex==3);
    
    let pos = arc.position + arc.size * vec2f(x, y);

    var result: VsOut
    result.position = vec4f(pos, 0f, 1f);
    result.arc = arc;
}

@fragment
fn fs_main(@builtin(position) position: vec4f, @location(0) arc: Arc) -> @location(0) vec4f 
{
    float distSquared = dot(position, arc.position + arc.size * .5f);
    if (distSquared > 1)
    {
        discard;
    }
}
")
        });
    }

    public void RenderArc(Rectangle bounds, float begin, float end, Color color)
    {
        if (!arcs.HasCapacity(1))
            OnFlush();

        arcs.Write([new(bounds, begin, end)]);
    }

    public override void Submit(RenderPassEncoder encoder)
    {
        encoder.Draw(6, (uint)arcs.Count, 0, 0);
    }

    public override void OnFlush()
    {
    }

    protected override RenderPipelineDescriptor GetRenderPipelineDescriptor()
    {
        throw new NotImplementedException();
    }

    struct Arc
    {
        public Rectangle Bounds;
        public float Begin;
        public float End;

        public Arc(Rectangle bounds, float begin, float end)
        {
            this.Bounds = bounds;
            this.Begin = begin;
            this.End = end;
        }
    }
}
