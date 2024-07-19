using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

    public override void PushRoundedRect(GeometryStream stream, Rectangle rect, Vector2 radii)
    {
        int steps = CalculateRoundedEdgeQuality(stream.TransformMatrix, MathF.Max(radii.X, radii.Y) * .5f, MathF.PI * .5f);

        float xRadius = MathF.Min(radii.X, rect.Width * .5f);
        float yRadius = MathF.Min(radii.Y, rect.Height * .5f);

        float xEdge = rect.Width * .5f - xRadius;
        float yEdge = rect.Height * .5f - yRadius;

        Rectangle innerRect = new(rect.X + xRadius, rect.Y + yRadius, rect.Width - 2 * xRadius, rect.Height - 2 * yRadius);
        
        if (xEdge > 0)
        {
            stream.WriteVertex(new(rect.X + .5f * rect.Width + xEdge, rect.Y));
            stream.WriteVertex(new(rect.X + .5f * rect.Width - xEdge, rect.Y));
        }

        if (xRadius > 0 && yRadius > 0) 
        {
            Vector2 focus = innerRect.GetAlignedPoint(Alignment.TopRight);
            WriteCorner(stream, focus, new(xRadius, yRadius), steps, MathF.PI * (3f / 2f), MathF.Tau);
        }

        if (yEdge > 0)
        {
            stream.WriteVertex(new(rect.X + rect.Width, rect.Y + .5f * rect.Height + yEdge));
            stream.WriteVertex(new(rect.X + rect.Width, rect.Y + .5f * rect.Height - yEdge));
        }

        if (xRadius > 0 && yRadius > 0)
        {
            Vector2 focus = innerRect.GetAlignedPoint(Alignment.BottomRight);
            WriteCorner(stream, focus, new(xRadius, yRadius), steps, 0, MathF.PI / 2f);
        }

        if (xEdge > 0)
        {
            stream.WriteVertex(new(rect.X + .5f * rect.Width + xEdge, rect.Y + rect.Height));
            stream.WriteVertex(new(rect.X + .5f * rect.Width - xEdge, rect.Y + rect.Height));
        }

        if (xRadius > 0 && yRadius > 0)
        {
            Vector2 focus = innerRect.GetAlignedPoint(Alignment.BottomLeft);
            WriteCorner(stream, focus, new(xRadius, yRadius), steps, MathF.PI / 2f, MathF.PI);
        }

        if (yEdge > 0)
        {
            stream.WriteVertex(new(rect.X, rect.Y + .5f * rect.Height + yEdge));
            stream.WriteVertex(new(rect.X, rect.Y + .5f * rect.Height - yEdge));
        }

        if (xRadius > 0 && yRadius > 0)
        {
            Vector2 focus = innerRect.GetAlignedPoint(Alignment.TopLeft);
            WriteCorner(stream, focus, new(xRadius, yRadius), steps, MathF.PI, MathF.PI * (3f / 2f));
        }

        static void WriteCorner(GeometryStream stream, Vector2 focus, Vector2 radii, int steps, float start, float end)
        {
            float increment = (end - start) / steps;
            float angle = start;
            for (int i = 0; i < steps; i++)
            {
                stream.WriteVertex(focus + radii * Angle.ToVector(angle));
                angle += increment;
                stream.WriteVertex(focus + radii * Angle.ToVector(angle));
            }
        }
    }

    public override void PushArc(GeometryStream stream, Rectangle bounds, float begin, float end, bool includeCenter)
    {
        int steps = CalculateRoundedEdgeQuality(stream.TransformMatrix, MathF.Max(bounds.Width, bounds.Height) * .5f, end - begin);

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

    public override void PushEllipse(GeometryStream stream, Rectangle bounds)
    {
        int steps = CalculateRoundedEdgeQuality(stream.TransformMatrix, MathF.Max(bounds.Width, bounds.Height) * .5f, MathF.Tau);

        float increment = MathF.Tau / steps;
        float angle = 0;
        for (int i = 0; i < steps; i++)
        {
            stream.WriteVertex(bounds.Center + Angle.ToVector(angle) * .5f * bounds.Size);
            angle += increment;
            stream.WriteVertex(bounds.Center + Angle.ToVector(angle) * .5f * bounds.Size);
        }
    }
}
