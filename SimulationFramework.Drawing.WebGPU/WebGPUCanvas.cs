using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using WebGPU;

namespace SimulationFramework.Drawing.WebGPU;
internal class WebGPUCanvas : ICanvas
{
    public ITexture Target => target;
    public CanvasState State => GetCurrentState();

    private WebGPUResources resources;
    private ITextureViewProvider target;

    private CommandEncoder commandEncoder;
    private RenderPassEncoder renderPassEncoder;
    private readonly Stack<WebGPUCanvasState> states = new();
    private WebGPUCanvasState? defaultState;

    private VertexWriter<Vector2> positionWriter;
    private VertexWriter<ushort> indexWriter;

    private RenderPipeline pipeline;
    private ShaderModule shaderModule;

    public WebGPUCanvas(WebGPUResources resources, ITextureViewProvider target)
    {
        this.resources = resources;
        this.target = target;
        commandEncoder = resources.Device.CreateCommandEncoder(default);

        shaderModule = resources.Device.CreateShaderModule(new()
        {
            NextInChain = new ShaderModuleWGSLDescriptor()
            {
                Code = @"

@vertex
fn vs_main(@location(0) position : vec2f ) -> @builtin(position) vec4f {
    return vec4f(position, 0.0, 1.0);
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
            Primitive = new()
            {
                CullMode = CullMode.None,
                FrontFace = FrontFace.CW,
                Topology = PrimitiveTopology.TriangleList,
            },
            Vertex = new()
            {
                Buffers = [
                    // position
                    new VertexBufferLayout()
                    {
                        ArrayStride = (uint)Unsafe.SizeOf<Vector2>(),
                        Attributes = [
                            new VertexAttribute()
                            {
                                Format = VertexFormat.Float32x2,
                                Offset = 0,
                                ShaderLocation = 0,
                            }
                        ],
                    }
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
                            Color = new()
                            {
                                SrcFactor = BlendFactor.One,
                                DstFactor = BlendFactor.Zero,
                                Operation = BlendOperation.Add,
                            },
                            Alpha = new()
                            {
                                SrcFactor = BlendFactor.One,
                                DstFactor = BlendFactor.Zero,
                                Operation = BlendOperation.Add,
                            },
                        },
                        WriteMask = ColorWriteMask.All,
                    }
                ],
            },
            Multisample = new()
            {
                Count = 1,
                Mask = ~0u,
            },
            DepthStencil = null,
        });

        positionWriter = new(resources, BufferUsage.Vertex, 1024);
        indexWriter = new(resources, BufferUsage.Index, 2048);
    }

    private WebGPUCanvasState GetCurrentState()
    {
        defaultState ??= new();
        return states.TryPeek(out var state) ? state : defaultState;
    }

    public void Clear(Color color)
    {
        RenderPassColorAttachment colorAttachment = new()
        {
            View = target.GetView(),
            ClearValue = new()
            {
                R = color.R * (1f / 255f),
                G = color.G * (1f / 255f),
                B = color.B * (1f / 255f),
                A = color.A * (1f / 255f),
            },
            LoadOp = LoadOp.Clear,
            StoreOp = StoreOp.Store,
        };

        RenderPassEncoder renderPassEncoder = commandEncoder.BeginRenderPass(new()
        {
            ColorAttachments = [colorAttachment],
        });

        renderPassEncoder.End();
    }

    public void Dispose()
    {
        commandEncoder.Dispose();
    }

    public void DrawArc(Rectangle bounds, float begin, float end, bool includeCenter)
    {
        throw new NotImplementedException();
    }

    public void DrawLine(Vector2 p1, Vector2 p2)
    {
        throw new NotImplementedException();
    }

    public void DrawPolygon(ReadOnlySpan<Vector2> polygon, bool close = true)
    {
        throw new NotImplementedException();
    }

    private void DrawTriangles(ReadOnlySpan<Vector2> polygon, ReadOnlySpan<ushort> indices)
    {
        if (!(positionWriter.HasCapacity(polygon.Length) && indexWriter.HasCapacity(indices.Length)))
        {
            Flush();
        }

        var baseIndex = positionWriter.Count;
        positionWriter.Write(polygon);

        var indexSpan = indexWriter.GetWritableSpan(indices.Length);
        for (int i = 0; i < indexSpan.Length; i++)
        {
            indexSpan[i] = (ushort)(indices[i] + baseIndex);
        }
    }

    public void DrawRect(Rectangle rect)
    {
        Span<Vector2> vertices = [
            new(rect.X, rect.Y),
            new(rect.X + rect.Width, rect.Y),
            new(rect.X, rect.Y + rect.Height),
            new(rect.X + rect.Width, rect.Y + rect.Height),
            ];

        Span<ushort> indices = [0,1,2,1,3,2];

        DrawTriangles(vertices, indices);
    }

    public void DrawRoundedRect(Rectangle rect, float radius)
    {
        throw new NotImplementedException();
    }

    public void DrawText(ReadOnlySpan<char> text, Vector2 position, Alignment alignment = Alignment.TopLeft, TextBounds origin = TextBounds.BestFit)
    {
        throw new NotImplementedException();
    }

    public void DrawTexture(ITexture texture, Rectangle source, Rectangle destination)
    {
        throw new NotImplementedException();
    }

    public void Flush()
    {
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

        rpEncoder.SetViewport(0, 0, target.Width, target.Height, 0, 1);
        rpEncoder.SetPipeline(pipeline);
        rpEncoder.SetVertexBuffer(0, positionBuffer, 0, positionBuffer.Size);
        rpEncoder.SetIndexBuffer(indexBuffer, IndexFormat.Uint16, 0, indexBuffer.Size);
        rpEncoder.DrawIndexed((uint)indexWriter.Count, 1, 0, 0, 0);

        rpEncoder.End();
        rpEncoder.Dispose();

        positionWriter.Reset();
        indexWriter.Reset();

        using var commandBuffer = commandEncoder.Finish(default);
        resources.Queue.Submit([commandBuffer]);
        commandEncoder.Dispose();
        commandEncoder = resources.Device.CreateCommandEncoder(default);
    }

    public Vector2 MeasureText(ReadOnlySpan<char> text, float maxLength, out int charsMeasured, TextBounds bounds = TextBounds.BestFit)
    {
        throw new NotImplementedException();
    }

    public void PopState()
    {
        if (states.Count <= 0)
            throw new InvalidOperationException("No state to pop!");

        states.Pop();
    }

    public void PushState()
    {
        var state = new WebGPUCanvasState(GetCurrentState());
        states.Push(state);
    }

    public void ResetState()
    {
        GetCurrentState().Reset();
    }
}

class VertexWriter<T> where T : unmanaged
{
    private readonly WebGPUResources resources;
    private readonly Buffer buffer;
    private readonly T[] bufferData;

    public int Count { get; private set; }
    public int Capacity => bufferData.Length;

    public VertexWriter(WebGPUResources resources, BufferUsage usage, int capacity = 512)
    {
        this.resources = resources;
        
        bufferData = new T[capacity];
        
        buffer = resources.Device.CreateBuffer(new()
        {
            MappedAtCreation = false,
            Size = (ulong)(Unsafe.SizeOf<T>() * bufferData.Length),
            Usage = BufferUsage.CopyDst | usage,
        });
    }

    public bool HasCapacity(int count)
    {
        return this.Count + count < Capacity;
    }

    public void Write(ReadOnlySpan<T> elements)
    {
        var destSpan = bufferData.AsSpan(Count, elements.Length);
        elements.CopyTo(destSpan);
        Count += elements.Length;
    }

    public Span<T> GetWritableSpan(int length)
    {
        var result = bufferData.AsSpan(Count, length);
        Count += length;
        return result;
    }

    public Buffer Upload()
    {
        resources.Queue.WriteBuffer(buffer, 0, bufferData);
        return buffer;
    }

    public void Reset()
    {
        Count = 0;
    }
}