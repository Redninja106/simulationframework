using SimulationFramework.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing;

public interface IRenderer
{
    ITexture RenderTarget { get; set; }
    ITexture<float> DepthTarget { get; set; }
    ITexture<byte> StencilTarget { get; set; }

    void Clear(Color? color, float? depth, byte? stencil);

    void SetVertexBuffer<T>(IBuffer<T>? vertexBuffer) where T : unmanaged;
    void SetIndexBuffer(IBuffer<uint>? indexBuffer);

    void SetVertexShader<T>(T vertexShader) where T : struct, IShader;
    void SetFragmentShader<T>(T fragmentShader) where T : struct, IShader;

    void DrawPrimitives(PrimitiveKind kind, int count, int offset);
    void DrawIndexedPrimitives(PrimitiveKind kind, int count, int vertexOffset, int indexOffset);

    sealed void SetViewport(float x, float y, float w, float h) => SetViewport(new(x, y, w, h));
    sealed void SetViewport(Rectangle viewport) => SetViewport(viewport, 0.0f, 1.0f);
    sealed void SetViewport(float x, float y, float w, float h, float minDepth, float maxDepth) => SetViewport(new(x, y, w, h), minDepth, maxDepth);
    void SetViewport(Rectangle viewport, float minDepth, float maxDepth);

    sealed void Clip(float x, float y, float w, float h) => Clip(new(x, y, w, h));
    void Clip(Rectangle? rectangle);

    void PushState();
    void PopState();
    void ResetState();

}