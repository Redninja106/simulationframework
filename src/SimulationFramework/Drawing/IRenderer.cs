using SimulationFramework.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public interface IRenderer
{
    IGraphicsQueue Queue { get; set; }
    
    ITexture<Color>? RenderTarget { get; set; }
    ITexture<float>? DepthTarget { get; set; }
    ITexture<byte>? StencilTarget { get; set; }

    CullMode CullMode { get; set; }
    bool Wireframe { get; set; }

    float DepthBias { get; set; }
    bool WriteDepth { get; set; }
    DepthStencilComparison DepthComparison { get; set; }

    byte StencilReferenceValue { get; set; }
    byte StencilReadMask { get; set; }
    byte StencilWriteMask { get; set; }
    DepthStencilComparison StencilComparison { get; set; }
    StencilOperation StencilFailOperation { get; set; }
    StencilOperation StencilPassDepthFailOperation { get; set; }
    StencilOperation StencilPassOperation { get; set; }

    bool BlendEnabled { get; set; }
    ColorF BlendConstant { get; set; }

    void Submit(IGraphicsQueue deferredQueue);

    void BlendState(BlendMode sourceBlend, BlendMode destinationBlend, BlendOperation operation = BlendOperation.Add);
    void BlendState(BlendMode sourceBlend, BlendMode destinationBlend, BlendMode sourceBlendAlpha, BlendMode destinationBlendAlpha, BlendOperation operation = BlendOperation.Add, BlendOperation operationAlpha = BlendOperation.Add);

    void ClearRenderTarget(Color color);
    void ClearDepthTarget(float depth);
    void ClearStencilTarget(byte stencil);

    void SetVertexBuffer<T>(IBuffer<T>? vertexBuffer) where T : unmanaged;
    void SetInstanceBuffer<T>(IBuffer<T>? instanceBuffer) where T : unmanaged;
    void SetIndexBuffer(IBuffer<uint> indexBuffer);
    
    void SetVertexShader(IShader? shader);
    void SetGeometryShader(IShader? shader);
    void SetFragmentShader(IShader? shader);

    void DrawPrimitives(PrimitiveKind kind, int vertexCount, int vertexOffset = 0);
    void DrawPrimitives(PrimitiveKind kind, int vertexCount, int instanceCount, int vertexOffset = 0, int instanceOffset = 0);
    void DrawPrimitives(PrimitiveKind kind, IBuffer<DrawCommand> commands, int commandOffset = 0);

    void DrawIndexedPrimitives(PrimitiveKind kind, int indexCount, int indexOffset = 0, int vertexOffset = 0);
    void DrawIndexedPrimitives(PrimitiveKind kind, int indexCount, int instanceCount, int indexOffset = 0, int instanceOffset = 0);
    void DrawIndexedPrimitives(PrimitiveKind kind, IBuffer<IndexedDrawCommand> commands, int commandOffset = 0);

    void DrawGeometry(IGeometry geometry);
    void DrawGeometryInstanced(IGeometry geometry, int instanceCount);

    void SetViewport(Rectangle viewport);
    void Clip(Rectangle? rectangle);

    void PushState();
    void PopState();
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