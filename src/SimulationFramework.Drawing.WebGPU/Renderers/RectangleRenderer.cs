using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebGPU;

namespace SimulationFramework.Drawing.WebGPU.Renderers;
internal class RectangleRenderer : Renderer
{
    private BufferWriter<Rectangle> rectWriter;

    private int drawnRectCount;
    protected readonly int capacity = 1024 * 16;

    private ShaderModule module;
    
    private BindGroup bindGroup;

    public RectangleRenderer(GraphicsResources resources, WebGPUCanvas canvas) : base(resources, canvas)
    {
        rectWriter = new(resources, BufferUsage.Vertex, capacity);

        bindGroup = Resources.Device.CreateBindGroup(new()
        {
            Layout = RenderPipeline.GetBindGroupLayout(0),
            Entries = [
                new BindGroupEntry()
                {
                    Binding = 0,
                    Buffer = Canvas.uniformBuffer,
                    Offset = 0,
                    Size = Canvas.uniformBuffer.Size
                },
            ]
        });
    }

    protected override void Init(GraphicsResources resources)
    {
        module = resources.Device.CreateShaderModule(new()
        {
            NextInChain = new ShaderModuleWGSLDescriptor()
            {
                Code = @"
struct VsOut {
    @builtin(position) position : vec4f,
    @location(0) color : vec4f,
}

@vertex 
fn vs_main(@builtin(vertex_index) vertexIndex : u32, @location(0) rect : vec4f, @location(1) color : vec4f) -> VsOut 
{
    var result : VsOut;

    let xPos = f32(vertexIndex) % 2f;
    let yPos = floor(f32(vertexIndex) / 2f);

    result.position = vec4f(rect.xy + rect.zw * vec2f(xPos, yPos), 0f, 1f);
    result.color = color;

    return result;
}

@fragment
fn fs_main(out : VsOut) -> @location(0) vec4f {
    return out.color;
}
"
            }
        });


    }

    protected override RenderPipelineDescriptor GetRenderPipelineDescriptor()
    {
        return new()
        {
            Primitive = new(PrimitiveTopology.TriangleStrip, IndexFormat.Undefined, FrontFace.CW, CullMode.None),
            Vertex = new()
            {
                Buffers = [
                    new VertexBufferLayout()
                    {
                        ArrayStride = (ulong)Unsafe.SizeOf<Rectangle>(),
                        StepMode = VertexStepMode.Instance,
                        Attributes = [
                            new global::WebGPU.VertexAttribute(VertexFormat.Float32x4, 0, 0)
                        ]
                    },
                ],
                Module = this.module,
                EntryPoint = "vs_main",
            },
            Multisample = new(1, ~0u, false),
            Fragment = new()
            {
                Targets = [
                    new ColorTargetState()
                    {
                        Format = this.Resources.Surface.GetPreferredFormat(Resources.Adapter),
                        WriteMask = ColorWriteMask.All,
                        Blend = new()
                        {
                            Color = new(BlendOperation.Add, BlendFactor.One, BlendFactor.Zero),
                            Alpha = new(BlendOperation.Add, BlendFactor.One, BlendFactor.Zero),
                        }
                    }
                ],
                Module = this.module,
                EntryPoint = "fs_main",
            }
        };
    }

    public virtual void RenderRectangle(Rectangle rectangle)
    {
        rectWriter.Write([rectangle]);
    }

    public override void Submit(RenderPassEncoder encoder)
    {
        var rectBuffer = rectWriter.GetBuffer();

        encoder.SetBindGroup(0, bindGroup, []);
        encoder.SetViewport(0, 0, Canvas.Target.Width, Canvas.Target.Height, 0, 1);
        encoder.SetPipeline(RenderPipeline);

        encoder.SetVertexBuffer(0, rectBuffer, 0, rectBuffer.Size);
        encoder.Draw(4, (uint)rectWriter.Count, 0, (uint)drawnRectCount);
        drawnRectCount += rectWriter.Count;
    }

    public override void OnFlush()
    {
        rectWriter.Upload();
        rectWriter.Reset();
        drawnRectCount = 0;
    }
}
