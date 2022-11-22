using SimulationFramework.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing;

public interface IRenderer
{
    ITexture<Color>? RenderTarget { get; set; }
    ITexture<float>? DepthTarget { get; set; }
    ITexture<byte>? StencilTarget { get; set; }

    CullMode CullMode { get; set; }
    float DepthBias { get; set; }

    void ClearRenderTarget(Color color);
    void ClearDepthTarget(float depth);
    void ClearStencilTarget(byte stencil);

    void SetVertexBuffer<T>(IBuffer<T>? vertexBuffer) where T : unmanaged;
    void SetInstanceBuffer<T>(IBuffer<T>? instanceBuffer) where T : unmanaged;
    void SetIndexBuffer(IBuffer<uint>? indexBuffer);
    
    void SetVertexShader(IShader shader);
    void SetGeometryShader(IShader shader);
    void SetFragmentShader(IShader shader);

    void DrawPrimitives(PrimitiveKind kind, int count, int vertexOffset = 0);
    void DrawIndexedPrimitives(PrimitiveKind kind, int count, int vertexOffset = 0, int indexOffset = 0);
    void DrawInstancedPrimitives(PrimitiveKind kind, int count, int instanceCount, int vertexOffset = 0, int instanceOffset = 0);
    void DrawIndexedInstancedPrimitives(PrimitiveKind kind, int count, int instanceCount, int vertexOffset = 0, int indexOffset = 0, int instanceOffset = 0);

    void SetViewport(Rectangle viewport);
    void Clip(Rectangle? rectangle);

    void PushState();
    void PopState();
    void ResetState();

    void Flush();
}