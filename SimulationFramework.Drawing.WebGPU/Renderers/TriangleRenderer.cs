using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WebGPU;

namespace SimulationFramework.Drawing.WebGPU.Renderers;
internal class TriangleRenderer : Renderer
{
    private WebGPUCanvas canvas;
    
    private BufferWriter<Vector2> positionWriter;
    private BufferWriter<Color> colorWriter;

    private int submittedIndex;

    private RenderPipeline pipeline;
    private ShaderModule shaderModule;
    private BindGroup bindGroup;

    public TriangleRenderer(GraphicsResources resources, WebGPUCanvas canvas)
        : base(resources, canvas)
    {
        this.canvas = canvas;

        canvas.uniformBuffer = resources.Device.CreateBuffer(new()
        {
            Size = (ulong)Unsafe.SizeOf<WebGPUCanvas.FrameConstUniforms>(),
            Usage = BufferUsage.Uniform | BufferUsage.CopyDst,
        });

        shaderModule = resources.Device.CreateShaderModule(new()
        {
            NextInChain = new ShaderModuleWGSLDescriptor()
            {
                Code = @"
struct UniformData {
    viewportMatrix: mat4x4f
}

@group(0) @binding(0) var<uniform> uniforms : UniformData;

struct VsOut {
    @builtin(position) position : vec4f,
    @location(0) color : vec4f
}

@vertex
fn vs_main(@location(0) position : vec2f, @location(1) color : vec4f) -> VsOut {
    var out: VsOut;
    var pos = vec4f(position.xy, 0f, 1f);
    pos = uniforms.viewportMatrix * pos;
    out.position = pos;
    out.color = color;

    return out;
}

@fragment
fn fs_main(vsOut: VsOut) -> @location(0) vec4f {
    return vsOut.color;
}
"
            }
        });

        bindGroup = resources.Device.CreateBindGroup(new()
        {
            Layout = pipeline.GetBindGroupLayout(0),
            Entries = [
                new BindGroupEntry()
                {
                    Binding = 0,
                    Buffer = canvas.uniformBuffer,
                    Offset = 0,
                    Size = canvas.uniformBuffer.Size
                },
            ]
        });

        positionWriter = new(resources, BufferUsage.Vertex, 1024 * 64);
        colorWriter = new(resources, BufferUsage.Vertex, 1024 * 64);
    }

    protected override RenderPipelineDescriptor GetRenderPipelineDescriptor()
    {
        return new()
        {
            Primitive = new PrimitiveState(PrimitiveTopology.TriangleList, IndexFormat.Undefined, FrontFace.CW, CullMode.None),
            Vertex = new()
            {
                Buffers = [
                    // position
                    new VertexBufferLayout((uint)Unsafe.SizeOf<Vector2>(), VertexStepMode.Vertex, [
                            new VertexAttribute(VertexFormat.Float32x2, 0, 0)
                    ]),
                    // color
                    new VertexBufferLayout((uint)Unsafe.SizeOf<Color>(), VertexStepMode.Vertex, [
                            new VertexAttribute(VertexFormat.Unorm8x4, 0, 1)
                    ]),
                ],
                EntryPoint = "vs_main",
                Module = shaderModule,
            },
            Fragment = new()
            {
                EntryPoint = "fs_main",
                Module = shaderModule,
                Targets = [
                    new ColorTargetState()
                    {
                        Format = Resources.Surface.GetPreferredFormat(Resources.Adapter),
                        Blend = new()
                        {
                            Color = new BlendComponent(BlendOperation.Add, BlendFactor.One, BlendFactor.Zero),
                            Alpha = new BlendComponent(BlendOperation.Add, BlendFactor.One, BlendFactor.Zero),
                        },
                        WriteMask = ColorWriteMask.All,
                    }
                ],
            },
            Multisample = new MultisampleState(1, ~0u, false),
            DepthStencil = null,
        };
    }
    public void RenderTriangles(ReadOnlySpan<Vector2> polygon, ReadOnlySpan<Color> colors)
    {
        if (!positionWriter.HasCapacity(polygon.Length))
        {
            canvas.Flush();
        }

        positionWriter.Write(polygon);
        colorWriter.Write(colors);
    }


    public override void Submit(RenderPassEncoder renderPass)
    {
        var positionBuffer = positionWriter.GetBuffer();
        var colorBuffer = colorWriter.GetBuffer();

        renderPass.SetBindGroup(0, bindGroup, []);
        renderPass.SetViewport(0, 0, canvas.Target.Width, canvas.Target.Height, 0, 1);
        renderPass.SetPipeline(pipeline);
        renderPass.SetVertexBuffer(0, positionBuffer, 0, positionBuffer.Size);
        renderPass.SetVertexBuffer(1, colorBuffer, 0, colorBuffer.Size);
        renderPass.Draw((uint)positionWriter.Count, 1, (uint)submittedIndex, 0);
        submittedIndex = positionWriter.Count;
    }

    public override void OnFlush()
    {
        submittedIndex = 0;

        positionWriter.Upload();
        positionWriter.Reset();

        colorWriter.Upload();
        colorWriter.Reset();
    }
}
