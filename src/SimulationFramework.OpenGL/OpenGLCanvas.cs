using SimulationFramework.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.OpenGL;
internal class OpenGLCanvas : ICanvas
{
    public ITexture Target => throw new NotImplementedException();
    public CanvasState State => throw new NotImplementedException();

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
        throw new NotImplementedException();
    }

    public void DrawPolygon(ReadOnlySpan<Vector2> polygon, bool close = true)
    {
        throw new NotImplementedException();
    }

    public void DrawRect(Rectangle rect)
    {
        throw new NotImplementedException();
    }

    public void DrawRoundedRect(Rectangle rect, float radius)
    {
        throw new NotImplementedException();
    }

    public void DrawText(ReadOnlySpan<char> text, Vector2 position, Alignment alignment = Alignment.TopLeft, TextBounds origin = TextBounds.BestFit)
    {
        throw new NotImplementedException();
    }

    public void DrawTexture(ITexture texture, Rectangle source, Rectangle destination)
    {
        throw new NotImplementedException();
    }

    public void Flush()
    {
        throw new NotImplementedException();
    }

    public Vector2 MeasureText(ReadOnlySpan<char> text, float maxLength, out int charsMeasured, TextBounds bounds = TextBounds.BestFit)
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
