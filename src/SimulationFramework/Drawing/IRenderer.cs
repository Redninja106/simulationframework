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

    void Submit(IGraphicsQueue deferredQueue);

    void ClearRenderTarget(Color color);
    void ClearDepthTarget(float depth);
    void ClearStencilTarget(byte stencil);

    void SetVertexBuffer<T>(IBuffer<T>? vertexBuffer, int offset = 0) where T : unmanaged;
    void SetInstanceBuffer<T>(IBuffer<T>? instanceBuffer, int offset = 0) where T : unmanaged;
    void SetIndexBuffer(IBuffer<uint>? indexBuffer, int offset = 0);
    
    void SetVertexShader(IShader? shader);
    void SetGeometryShader(IShader? shader);
    void SetFragmentShader(IShader? shader);

    void DrawPrimitives(PrimitiveKind kind, int count);
    void DrawPrimitivesIndexed(PrimitiveKind kind, int count);
    void DrawPrimitivesInstanced(PrimitiveKind kind, int primitives, int instances);
    void DrawPrimitivesIndexedInstanced(PrimitiveKind kind, int primitives, int instances);

    void DrawGeometry(IGeometry geometry);
    void DrawGeometryInstanced(IGeometry geometry, int instanceCount);

    void SetViewport(Rectangle viewport);
    void Clip(Rectangle? rectangle);

    void PushState();
    void PopState();
    void ResetState();

    void Flush();
}