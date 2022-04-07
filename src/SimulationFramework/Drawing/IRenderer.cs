using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing;

public interface IRenderer
{
    void SetRenderTarget(ITexture renderTarget);

    void Clear(Color color);

    void SetShader(IShader shader);
    void SetVertexBuffer<T>(IBuffer<T> buffer) where T : unmanaged;
    void SetIndexBuffer(IBuffer<int> buffer);

    void DrawPrimitives(PrimitiveKind kind, int count, int offset);
    void DrawIndexedPrimitives(PrimitiveKind kind, int count, int vertexOffset, int indexOffset);
}
