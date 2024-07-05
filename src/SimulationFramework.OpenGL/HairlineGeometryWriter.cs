using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.OpenGL;
internal class HairlineGeometryWriter : GeometryWriter
{
    public override uint GetPrimitive()
    {
        return GL_LINES;
    }

    public override void PushLine(GeometryStream stream, Vector2 from, Vector2 to)
    {
        stream.WriteVertex(from);
        stream.WriteVertex(to);
    }

    public override void PushPolygon(GeometryStream stream, ReadOnlySpan<Vector2> polygon, bool close)
    {
        for (int i = 0; i < polygon.Length - 1; i++)
        {
            stream.WriteVertex(polygon[i]);
            stream.WriteVertex(polygon[i + 1]);
        }

        if (close)
        {
            stream.WriteVertex(polygon[^1]);
            stream.WriteVertex(polygon[0]);
        }
    }

    public override void PushRect(GeometryStream stream, Rectangle rect)
    {
        stream.WriteVertex(new(rect.X, rect.Y));
        stream.WriteVertex(new(rect.X + rect.Width, rect.Y));

        stream.WriteVertex(new(rect.X + rect.Width, rect.Y));
        stream.WriteVertex(new(rect.X + rect.Width, rect.Y + rect.Height));

        stream.WriteVertex(new(rect.X + rect.Width, rect.Y + rect.Height));
        stream.WriteVertex(new(rect.X, rect.Y + rect.Height));

        stream.WriteVertex(new(rect.X, rect.Y + rect.Height));
        stream.WriteVertex(new(rect.X, rect.Y));
    }

    public override void PushTriangles(GeometryStream stream, ReadOnlySpan<Vector2> triangles)
    {
        int triCount = triangles.Length / 3;
        for (int i = 0; i < triCount; i++)
        {
            stream.WriteVertex(triangles[i * 3 + 0]);
            stream.WriteVertex(triangles[i * 3 + 1]);

            stream.WriteVertex(triangles[i * 3 + 1]);
            stream.WriteVertex(triangles[i * 3 + 2]);

            stream.WriteVertex(triangles[i * 3 + 2]);
            stream.WriteVertex(triangles[i * 3 + 0]);
        }
    }

    public override void PushRoundedRect(GeometryStream stream, Rectangle rect, float radius)
    {
        throw new NotImplementedException();
    }

    public override void PushArc(GeometryStream stream, Rectangle bounds, float begin, float end, bool includeCenter)
    {
        const int steps = 32;

        float increment = (end - begin) / steps;
        float angle = begin;
        for (int i = 0; i < steps; i++)
        {
            stream.WriteVertex(bounds.Center + Angle.ToVector(angle) * .5f * bounds.Size);
            angle += increment;
            stream.WriteVertex(bounds.Center + Angle.ToVector(angle) * .5f * bounds.Size);
        }

        if (Angle.Normalize(begin) != Angle.Normalize(end) && includeCenter)
        {
            stream.WriteVertex(bounds.Center + Angle.ToVector(end) * .5f * bounds.Size);
            stream.WriteVertex(bounds.Center);
            stream.WriteVertex(bounds.Center);
            stream.WriteVertex(bounds.Center + Angle.ToVector(begin) * .5f * bounds.Size);
        }
        else
        {
            stream.WriteVertex(bounds.Center + Angle.ToVector(end) * .5f * bounds.Size);
            stream.WriteVertex(bounds.Center + Angle.ToVector(begin) * .5f * bounds.Size);
        }
    }
}
