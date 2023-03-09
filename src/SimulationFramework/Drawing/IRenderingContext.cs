using SimulationFramework.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public interface IRenderingContext : IDisposable
{
    IGraphicsQueue Queue { get; }
    
    ITexture<Color>? RenderTarget { get; set; }
    ITexture<float>? DepthTarget { get; set; }
    ITexture<byte>? StencilTarget { get; set; }

    CullMode CullMode { get; set; }
    bool Wireframe { get; set; }

    ColorF BlendConstant { get; set; }

    byte StencilTestValue { get; set; }

    void Submit(IGraphicsQueue deferredQueue);

    void SetBlendEnabled(bool enabled);
    void SetBlendMode(BlendMode sourceBlend, BlendMode destinationBlend, BlendOperation operation = BlendOperation.Add);
    void SetAlphaBlendMode(BlendMode sourceBlend, BlendMode destinationBlend, BlendOperation operation = BlendOperation.Add);

    void SetStencilMode(DepthStencilComparison comparison, StencilOperation pass, StencilOperation fail);
    void SetStencilMasks(byte readMask, byte writeMask);

    void SetDepthMode(DepthStencilComparison comparison, bool readOnly, float depthBias);

    void ClearRenderTarget(Color color);
    void ClearDepthTarget(float depth);
    void ClearStencilTarget(byte stencil);

    void SetVertexBuffer<T>(IBuffer<T>? vertexBuffer) where T : unmanaged;
    void SetInstanceBuffer<T>(IBuffer<T>? instanceBuffer) where T : unmanaged;
    void SetIndexBuffer<T>(IBuffer<T>? indexBuffer) where T : unmanaged;
    
    void SetVertexShader(IShader? shader);
    void SetGeometryShader(IShader? shader);
    void SetFragmentShader(IShader? shader);

    void DrawPrimitives(PrimitiveKind kind, int vertexCount, int vertexOffset = 0);
    void DrawIndexedPrimitives(PrimitiveKind kind, int indexCount, int indexOffset = 0, int vertexOffset = 0);

    void DrawInstancedPrimitives(PrimitiveKind kind, int vertexCount, int instanceCount, int vertexOffset = 0, int instanceOffset = 0);
    void DrawIndexedInstancedPrimitives(PrimitiveKind kind, int indexCount, int instanceCount, int indexOffset = 0, int instanceOffset = 0);

    void DrawInstancedPrimitives(PrimitiveKind kind, IBuffer<DrawCommand> commands, int commandOffset = 0, int? commandCount = null);
    void DrawIndexedInstancedPrimitives(PrimitiveKind kind, IBuffer<IndexedDrawCommand> commands, int commandOffset = 0, int? commandCount = null);

    void DrawGeometry(IGeometry geometry);
    void DrawInstancedGeometry(IGeometry geometry, int instanceCount);

    void SetViewport(Rectangle bounds, float minDepth = 0, float maxDepth = 1);
    void SetClipRectangle(Rectangle? rectangle);

    void ResetState();
    void Flush();
}

public struct DrawCommand
{
    public int VertexCount { get; set; }
    public int InstanceCount { get; set; }

    public int VertexOffset { get; set; }
    public int InstanceOffset { get; set; }
}

public struct IndexedDrawCommand
{
    public int IndexCount { get; set; }
    public int InstanceCount { get; set; }

    public int IndexOffset { get; set; }
    public int VertexOffset { get; set; }
    public int InstanceOffset { get; set; }
}