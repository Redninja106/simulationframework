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
    
    void UseBuffers<T>(IBuffer<T> vertexBuffer, IBuffer<int> indexBuffer) where T : unmanaged;

    void UseShader(IShader shader);

    void DrawPrimitives(PrimitiveKind kind, int count, int offset);
    void DrawIndexedPrimitives(PrimitiveKind kind, int count, int vertexOffset, int indexOffset);
}
