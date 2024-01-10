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
   

    private GraphicsResources resources;

    public TriangleRenderer(GraphicsResources resources, WebGPUCanvas canvas)
    {
        this.canvas = canvas;
        this.resources = resources;

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
};

@group(0) @binding(0) var<uniform> uniforms : UniformData;

@vertex
fn vs_main(@location(0) position : vec2f) -> @builtin(position) vec4f {
    var pos = vec4f(position.xy, 0f, 1f);

    pos = uniforms.viewportMatrix * pos;

    return pos;
}

@fragment
fn fs_main() -> @location(0) vec4f {
    return vec4f(1.0, 0.0, 0.0, 1.0);
}
"
            }
        });

        pipeline = resources.Device.CreateRenderPipeline(new()
        {
            Primitive = new PrimitiveState(PrimitiveTopology.TriangleList, IndexFormat.Undefined, FrontFace.CW, CullMode.None),
            Vertex = new()
            {
                Buffers = [
                    // position
                    new VertexBufferLayout((uint)Unsafe.SizeOf<Vector2>(), VertexStepMode.Vertex, [
                            new VertexAttribute(VertexFormat.Float32x2, 0, 0)
                    ])
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
                        Format = resources.Surface.GetPreferredFormat(resources.Adapter),
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
    }

    public void RenderTriangles(ReadOnlySpan<Vector2> polygon)
    {
        if (!positionWriter.HasCapacity(polygon.Length))
        {
            canvas.Flush();
        }

        var baseIndex = positionWriter.Count;
        positionWriter.Write(polygon);
    }

    private int submitCount;

    public override void Submit(RenderPassEncoder renderPass)
    {
        var positionBuffer = positionWriter.GetBuffer();

        renderPass.SetBindGroup(0, bindGroup, []);
        renderPass.SetViewport(0, 0, canvas.Target.Width, canvas.Target.Height, 0, 1);
        renderPass.SetPipeline(pipeline);
        renderPass.SetVertexBuffer(0, positionBuffer, 0, positionBuffer.Size);
        renderPass.Draw((uint)positionWriter.Count, 1, (uint)submittedIndex, 0);
        submittedIndex = positionWriter.Count;
    }

    public override void OnFlush()
    {
        submittedIndex = 0;

        positionWriter.Upload();
        positionWriter.Reset();
    }
}
