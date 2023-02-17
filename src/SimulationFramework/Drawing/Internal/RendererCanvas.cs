using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Internal;
internal class RendererCanvas : ICanvas
{
    public ITexture<Color> Target { get; }
    public CanvasState State { get; }

    private IBatchRenderer batchRenderer;
    private IRenderer renderer;

    public void Clear(Color color)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public void DrawArc(Rectangle bounds, float begin, float end, bool includeCenter)
    {
        throw new NotImplementedException();
    }

    public void DrawLine(Vector2 p1, Vector2 p2)
    {
        batchRenderer.PushLine(p1, p2);
    }

    public void DrawPolygon(Span<Vector2> polygon)
    {
        batchRenderer.PushPolygon(polygon);
    }

    public void DrawRoundedRect(Rectangle rect, float radius)
    {
        if (radius <= 0)
        {
            batchRenderer.PushQuad(rect);
        }

        throw new NotImplementedException();
    }

    public void DrawText(string text, Vector2 position, Alignment alignment = Alignment.TopLeft)
    {
        throw new NotImplementedException();
    }

    public void DrawTexture(ITexture<Color> texture, Rectangle source, Rectangle destination)
    {
        throw new NotImplementedException();
    }

    public void Flush()
    {
        throw new NotImplementedException();
    }

    public Vector2 MeasureText(string text, float maxLength, out int charsMeasured)
    {
        throw new NotImplementedException();
    }

    public void PopState()
    {
        throw new NotImplementedException();
    }

    public void PushState()
    {
        throw new NotImplementedException();
    }

    public void ResetState()
    {
        throw new NotImplementedException();
    }
}

class RendererCanvasState : CanvasState
{
    
}