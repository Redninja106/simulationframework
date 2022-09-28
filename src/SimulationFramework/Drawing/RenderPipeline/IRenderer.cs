using SimulationFramework.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.RenderPipeline;

public interface IRenderer
{
    void SetRenderTarget(ITexture renderTarget);
    //void SetRenderTarget(ITexture renderTarget, ITexture<float> depthTarget);

    void Clear(Color color);

    void SetVertexBuffer<T>(IBuffer<T> vertexBuffer) where T : unmanaged;
    //void SetIndexBuffer(IBuffer<int> indexBuffer);

    void SetVertexShader<T>(T vertexShader) where T : struct, IShader;
    void SetFragmentShader<T>(T fragmentShader) where T : struct, IShader;

    void DrawPrimitives(PrimitiveKind kind, int count, int offset);
    void DrawIndexedPrimitives(PrimitiveKind kind, int count, int vertexOffset, int indexOffset);

    void SetViewport(Rectangle viewport);
    //void SetViewport(Rectangle viewport, float minDepth, float maxDepth);
}