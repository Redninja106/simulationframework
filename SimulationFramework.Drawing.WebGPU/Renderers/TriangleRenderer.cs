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
    private BufferWriter<Vector2> positionWriter;
    private BufferWriter<ushort> indexWriter;

    private RenderPipeline pipeline;
    private ShaderModule shaderModule;
    private BindGroup bindGroup;
    struct UniformData
    {
        public Matrix4x4 viewportMatrix;
    }

    private UniformData uniformData;
    private Buffer uniformBuffer;

    private GraphicsResources resources;

    public TriangleRenderer(GraphicsResources resources)
    {
        this.resources = resources;

        uniformBuffer = resources.Device.CreateBuffer(new()
        {
            Size = (ulong)Unsafe.SizeOf<UniformData>(),
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
                    Buffer = uniformBuffer,
                    Offset = 0,
                    Size = uniformBuffer.Size
                },
            ]
        });

        positionWriter = new(resources, BufferUsage.Vertex, 1024);
        indexWriter = new(resources, BufferUsage.Index, 2048);
    }

    public override bool RequiresFlush()
    {
        return indexWriter.Count == indexWriter.Capacity;
    }

    public void RenderTriangles(ReadOnlySpan<Vector2> polygon, ReadOnlySpan<ushort> indices)
    {
        if (!(positionWriter.HasCapacity(polygon.Length) && indexWriter.HasCapacity(indices.Length)))
        {
            FinishPass();
        }

        var baseIndex = positionWriter.Count;
        positionWriter.Write(polygon);

        var indexSpan = indexWriter.GetWritableSpan(indices.Length);
        for (int i = 0; i < indexSpan.Length; i++)
        {
            indexSpan[i] = (ushort)(indices[i] + baseIndex);
        }
    }

    public override void FinishPass()
    {
        uniformData.viewportMatrix = Matrix4x4.CreateTranslation(-Target.Width / 2f, -Target.Height / 2f, 0) * Matrix4x4.CreateOrthographic(Target.Width, -Target.Height, 0, 1);
        resources.Queue.WriteBuffer(uniformBuffer, 0, [uniformData]);

        var positionBuffer = positionWriter.Upload();
        var indexBuffer = indexWriter.Upload();

        var rpEncoder = commandEncoder.BeginRenderPass(new()
        {
            ColorAttachments = [new RenderPassColorAttachment()
            {
                LoadOp = LoadOp.Load,
                StoreOp = StoreOp.Store,
                View = target.GetView(),
            }],
        });

        rpEncoder.SetBindGroup(0, bindGroup, []);
        rpEncoder.SetViewport(0, 0, target.Width, target.Height, 0, 1);
        rpEncoder.SetPipeline(pipeline);
        rpEncoder.SetVertexBuffer(0, positionBuffer, 0, positionBuffer.Size);
        rpEncoder.SetIndexBuffer(indexBuffer, IndexFormat.Uint16, 0, indexBuffer.Size);
        rpEncoder.DrawIndexed((uint)indexWriter.Count, 1, 0, 0, 0);

        rpEncoder.End();
        rpEncoder.Dispose();

        positionWriter.Reset();
        indexWriter.Reset();
    }
}
