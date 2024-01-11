using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WebGPU;

namespace SimulationFramework.Drawing.WebGPU.Renderers;
internal class RectangleRenderer : Renderer
{
    private BufferWriter<Rectangle> rectWriter;
    private int drawnRectCount;

    private ShaderModule module;
    private RenderPipeline pipeline;

    public RectangleRenderer(GraphicsResources resources)
    {
        rectWriter = new(resources, BufferUsage.Vertex, 1024 * 16);

        module = resources.Device.CreateShaderModule(new()
        {
            NextInChain = new ShaderModuleWGSLDescriptor()
            {
                Code = @"
struct Rectangle {
    pos: vec2f
    size: vec2f
}

struct VsOut {
    @builtin(position) position : vec2f
}

@vertex 
fn vs_main(@builtin(vertex_index) vertexIndex : u32, @location(0) rect : Rectangle) -> VsOut 
{
    var result : VsOut;

    result.position.x = rect.pos.x + rect.size.x * (vertexIndex % 2);
    result.position.y = rect.pos.y + rect.size.y * ((vertexIndex+1) % 2);
    
    return result;
}
"
            }
        });

        pipeline = resources.Device.CreateRenderPipeline(new()
        {
            Primitive = new(PrimitiveTopology.TriangleStrip, IndexFormat.Undefined, FrontFace.CW, CullMode.None),
            Vertex = new()
            {
                Buffers =
                [
                    new VertexBufferLayout()
                    {
                        ArrayStride = (ulong)Unsafe.SizeOf<Rectangle>(),
                        StepMode = VertexStepMode.Instance,
                        Attributes = [
                            new VertexAttribute(VertexFormat.Float32x4, 0, 0)
                        ]
                    }
                ]
            },
            Multisample = new(1, ~0u, false),
            Fragment = new()
            {
                Targets = [
                    new ColorTargetState() 
                    {
                        Format = resources.Surface.GetPreferredFormat(resources.Adapter),
                        WriteMask = ColorWriteMask.All,
                        Blend = new()
                        {
                            Color = new(BlendOperation.Add, BlendFactor.One, BlendFactor.Zero),
                            Alpha = new(BlendOperation.Add, BlendFactor.One, BlendFactor.Zero),
                        }
                    }
                ]
            }
        });
    }

    public void RenderRectangle(Rectangle rectangle)
    {
        rectWriter.Write([rectangle]);
    }

    public override void Submit(RenderPassEncoder encoder)
    {
        var rectBuffer = rectWriter.GetBuffer();

        encoder.SetVertexBuffer(0, rectBuffer, 0, rectBuffer.Size);
        encoder.Draw(6, (uint)rectWriter.Count, 0, (uint)drawnRectCount);
        drawnRectCount += rectWriter.Count;
    }

    public override void OnFlush()
    {
        rectWriter.Upload();
        rectWriter.Reset();
        drawnRectCount = 0;
    }
}
