using System.Collections;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace SimulationFramework.OpenGL;

class FillGeometryWriter : GeometryWriter
{
    public override uint GetPrimitive()
    {
        return GL_TRIANGLES;
    }

    public override void PushArc(GeometryStream stream, Rectangle bounds, float begin, float end, bool includeCenter)
    {
        const int steps = 16;

        float increment = (end - begin) / steps;
        float angle = begin;
        for (int i = 0; i < steps; i++)
        {
            stream.WriteVertex(bounds.Center);
            stream.WriteVertex(bounds.Center + Angle.ToVector(angle) * .5f * bounds.Size);
            angle += increment;
            stream.WriteVertex(bounds.Center + Angle.ToVector(angle) * .5f * bounds.Size);
        }
    }

    public override void PushLine(GeometryStream stream, Vector2 from, Vector2 to)
    {
        stream.WriteVertex(from);
        stream.WriteVertex(to);
    }

    public override void PushTriangles(GeometryStream stream, ReadOnlySpan<Vector2> triangles)
    {
        for (int i = 0; i < triangles.Length; i++)
        {
            stream.WriteVertex(triangles[i]);
        }
    }

    public override void PushPolygon(GeometryStream stream, ReadOnlySpan<Vector2> polygon, bool close)
    {
        // TODO: proper triangulation
        var bounds = Polygon.GetBoundingRectangle(polygon);
        for (int i = 0; i < polygon.Length; i++)
        {
            stream.WriteVertex(bounds.Center);
            stream.WriteVertex(polygon[i]);
            stream.WriteVertex(polygon[(i + 1) % polygon.Length]);
        }
    }

    public override void PushRect(GeometryStream stream, Rectangle rect)
    {
        stream.WriteVertex(new(rect.X, rect.Y));
        stream.WriteVertex(new(rect.X + rect.Width, rect.Y));
        stream.WriteVertex(new(rect.X, rect.Y + rect.Height));

        stream.WriteVertex(new(rect.X, rect.Y + rect.Height));
        stream.WriteVertex(new(rect.X + rect.Width, rect.Y));
        stream.WriteVertex(new(rect.X + rect.Width, rect.Y + rect.Height));
    }

    public override void PushRoundedRect(GeometryStream stream, Rectangle rect, float radius)
    {
        throw new NotImplementedException();
    }
}