using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Pipelines;

public interface IRenderer
{
    void SetRenderTarget(ITexture renderTarget);

    void Clear(Color color);

    void VertexBuffer<T>(IBuffer<T> vertexBuffer) where T : unmanaged;
    void IndexBuffer(IBuffer<int> indexBuffer);

    void UseShader(IShader shader);

    void DrawPrimitives(PrimitiveKind kind, int count, int offset);
    void DrawIndexedPrimitives(PrimitiveKind kind, int count, int vertexOffset, int indexOffset);

    void SetViewport(Rectangle viewport);
    //void SetViewport(Rectangle viewport, float minDepth, float maxDepth);
}
