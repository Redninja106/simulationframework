using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SimulationFramework.OpenGL;
internal class PathGeometryWriter : GeometryWriter
{
    public float StrokeWidth { get; set; }

    public override uint GetPrimitive()
    {
        return GL_TRIANGLES;
    }

    public override void PushLine(GeometryStream stream, Vector2 from, Vector2 to)
    {
        Vector2 diff = to - from;
        Vector2 perp = new(-diff.Y, diff.X);
        perp = perp.Normalized() * StrokeWidth * .5f;

        stream.WriteVertex(from + perp);
        stream.WriteVertex(from - perp);
        stream.WriteVertex(to + perp);

        stream.WriteVertex(to + perp);
        stream.WriteVertex(to - perp);
        stream.WriteVertex(from - perp);
    }

    public override void PushPolygon(GeometryStream stream, ReadOnlySpan<Vector2> polygon, bool close)
    {
        if (polygon.Length == 2)
        {
            PushLine(stream, polygon[0], polygon[1]);
            return;
        }
        if (polygon.Length < 2)
        {
            return;
        }

        Vector2 firstPoint = polygon[0];
        Vector2 firstOutgoing = polygon[1] - firstPoint;
        Vector2 firstOffset = ComputeOffset(StrokeWidth, close ? firstPoint - polygon[^1] : firstOutgoing, firstOutgoing);

        Vector2 prevPoint = firstPoint;
        Vector2 prevOffset = firstOffset;

        for (int i = 1; i < polygon.Length; i++)
        {
            Vector2 point = polygon[i];
            Vector2 incomingDir = point - prevPoint;
            Vector2 outgoingDir;
            if (i + 1 < polygon.Length)
            {
                outgoingDir = polygon[i + 1] - point;
            }
            else
            {
                if (close)
                {
                    outgoingDir = polygon[0] - point;
                }
                else
                {
                    outgoingDir = incomingDir;
                }
            }

            Vector2 offset = ComputeOffset(StrokeWidth, incomingDir, outgoingDir);

            stream.WriteVertex(point + offset);
            stream.WriteVertex(point - offset);
            stream.WriteVertex(prevPoint + prevOffset);

            stream.WriteVertex(prevPoint + prevOffset);
            stream.WriteVertex(prevPoint - prevOffset);
            stream.WriteVertex(point - offset);

            prevPoint = point;
            prevOffset = offset;
        }

        if (close)
        {
            stream.WriteVertex(firstPoint + firstOffset);
            stream.WriteVertex(firstPoint - firstOffset);
            stream.WriteVertex(prevPoint + prevOffset);

            stream.WriteVertex(prevPoint + prevOffset);
            stream.WriteVertex(prevPoint - prevOffset);
            stream.WriteVertex(firstPoint - firstOffset);
        }

        static Vector2 ComputeOffset(float width, Vector2 incomingDir, Vector2 outgoingDir)
        {
            Vector2 offsetDir = (-incomingDir.Normalized() + outgoingDir.Normalized()).Normalized();
            //h=w/sinx
            float a = Angle.FromVector(offsetDir);
            float b = Angle.FromVector(outgoingDir);
            float dist = (.5f * width) / MathF.Sin(Angle.Distance(a, b));
            return offsetDir * dist * MathF.Sign(Angle.SignedDistance(b, a));
        }
    }

    public override void PushTriangles(GeometryStream stream, ReadOnlySpan<Vector2> triangles)
    {
        for (int i = 0; i < triangles.Length - 2; i++)
        {
            PushPolygon(stream, triangles.Slice(i, 3), true);
        }
    }

    public override void PushRect(GeometryStream stream, Rectangle rect)
    {
        Vector2 tl = new(rect.X, rect.Y);
        Vector2 tlOffset = new(-StrokeWidth * .5f, -StrokeWidth * .5f);
        Vector2 tr = new(rect.X + rect.Width, rect.Y);
        Vector2 trOffset = new(StrokeWidth * .5f, -StrokeWidth * .5f);
        Vector2 bl = new(rect.X, rect.Y + rect.Height);
        Vector2 blOffset = new(-StrokeWidth * .5f, StrokeWidth * .5f);
        Vector2 br = new(rect.X + rect.Width, rect.Y + rect.Height);
        Vector2 brOffset = new(StrokeWidth * .5f, StrokeWidth * .5f);

        // top quad
        stream.WriteVertex(tl - tlOffset);
        stream.WriteVertex(tl + tlOffset);
        stream.WriteVertex(tr + trOffset);

        stream.WriteVertex(tr + trOffset);
        stream.WriteVertex(tr - trOffset);
        stream.WriteVertex(tl - tlOffset);

        // right quad
        stream.WriteVertex(tr - trOffset);
        stream.WriteVertex(tr + trOffset);
        stream.WriteVertex(br + brOffset);

        stream.WriteVertex(br + brOffset);
        stream.WriteVertex(br - brOffset);
        stream.WriteVertex(tr - trOffset);

        // bottom quad
        stream.WriteVertex(br - brOffset);
        stream.WriteVertex(br + brOffset);
        stream.WriteVertex(bl + blOffset);

        stream.WriteVertex(bl + blOffset);
        stream.WriteVertex(bl - blOffset);
        stream.WriteVertex(br - brOffset);

        // left quad
        stream.WriteVertex(bl - blOffset);
        stream.WriteVertex(bl + blOffset);
        stream.WriteVertex(tl + tlOffset);

        stream.WriteVertex(tl + tlOffset);
        stream.WriteVertex(tl - tlOffset);
        stream.WriteVertex(bl - blOffset);
    }

    public override void PushRoundedRect(GeometryStream stream, Rectangle rect, Vector2 radii)
    {
        radii = Vector2.Min(radii, rect.Size * .5f);

        Vector2 innerRadii = Vector2.Max(radii - new Vector2(StrokeWidth), Vector2.Zero);
        Vector2 outerRadii = Vector2.Max(radii + new Vector2(StrokeWidth), Vector2.Zero);

        int steps = CalculateRoundedEdgeQuality(stream.TransformMatrix, MathF.Max(outerRadii.X, outerRadii.Y) * .5f, MathF.PI * .5f);

        float xEdge = rect.Width * .5f - radii.X;
        float yEdge = rect.Height * .5f - radii.Y;

        Rectangle innerRect = new(rect.X + radii.X, rect.Y + radii.Y, rect.Width - 2 * radii.X, rect.Height - 2 * radii.Y);
        
        if (xEdge > 0)
        {
            stream.WriteVertex(new(rect.X + .5f * rect.Width + xEdge, rect.Y - StrokeWidth));
            stream.WriteVertex(new(rect.X + .5f * rect.Width - xEdge, rect.Y - StrokeWidth));
            stream.WriteVertex(new(rect.X + .5f * rect.Width - xEdge, rect.Y + StrokeWidth));

            stream.WriteVertex(new(rect.X + .5f * rect.Width + xEdge, rect.Y - StrokeWidth));
            stream.WriteVertex(new(rect.X + .5f * rect.Width + xEdge, rect.Y + StrokeWidth));
            stream.WriteVertex(new(rect.X + .5f * rect.Width - xEdge, rect.Y + StrokeWidth));
        }

        if (radii.X > 0 && radii.Y > 0)
        {
            Vector2 focus = innerRect.GetAlignedPoint(Alignment.TopRight);
            WriteCorner(stream, focus, innerRadii, outerRadii, steps, MathF.PI * (3f / 2f), MathF.Tau);
        }

        if (yEdge > 0)
        {
            stream.WriteVertex(new(rect.X + rect.Width - StrokeWidth, rect.Y + .5f * rect.Height + yEdge));
            stream.WriteVertex(new(rect.X + rect.Width - StrokeWidth, rect.Y + .5f * rect.Height - yEdge));
            stream.WriteVertex(new(rect.X + rect.Width + StrokeWidth, rect.Y + .5f * rect.Height - yEdge));

            stream.WriteVertex(new(rect.X + rect.Width - StrokeWidth, rect.Y + .5f * rect.Height + yEdge));
            stream.WriteVertex(new(rect.X + rect.Width + StrokeWidth, rect.Y + .5f * rect.Height + yEdge));
            stream.WriteVertex(new(rect.X + rect.Width + StrokeWidth, rect.Y + .5f * rect.Height - yEdge));
        }

        if (radii.X > 0 && radii.Y > 0)
        {
            Vector2 focus = innerRect.GetAlignedPoint(Alignment.BottomRight);
            WriteCorner(stream, focus, innerRadii, outerRadii, steps, 0, MathF.PI / 2f);
        }

        if (xEdge > 0)
        {
            stream.WriteVertex(new(rect.X + .5f * rect.Width + xEdge, rect.Y + rect.Height - StrokeWidth));
            stream.WriteVertex(new(rect.X + .5f * rect.Width - xEdge, rect.Y + rect.Height - StrokeWidth));
            stream.WriteVertex(new(rect.X + .5f * rect.Width - xEdge, rect.Y + rect.Height + StrokeWidth));

            stream.WriteVertex(new(rect.X + .5f * rect.Width + xEdge, rect.Y + rect.Height - StrokeWidth));
            stream.WriteVertex(new(rect.X + .5f * rect.Width + xEdge, rect.Y + rect.Height + StrokeWidth));
            stream.WriteVertex(new(rect.X + .5f * rect.Width - xEdge, rect.Y + rect.Height + StrokeWidth));
        }

        if (radii.X > 0 && radii.Y > 0)
        {
            Vector2 focus = innerRect.GetAlignedPoint(Alignment.BottomLeft);
            WriteCorner(stream, focus, innerRadii, outerRadii, steps, MathF.PI / 2f, MathF.PI);
        }

        if (yEdge > 0)
        {
            stream.WriteVertex(new(rect.X - StrokeWidth, rect.Y + .5f * rect.Height + yEdge));
            stream.WriteVertex(new(rect.X - StrokeWidth, rect.Y + .5f * rect.Height - yEdge));
            stream.WriteVertex(new(rect.X + StrokeWidth, rect.Y + .5f * rect.Height - yEdge));

            stream.WriteVertex(new(rect.X - StrokeWidth, rect.Y + .5f * rect.Height + yEdge));
            stream.WriteVertex(new(rect.X + StrokeWidth, rect.Y + .5f * rect.Height + yEdge));
            stream.WriteVertex(new(rect.X + StrokeWidth, rect.Y + .5f * rect.Height - yEdge));
        }

        if (radii.X > 0 && radii.Y > 0)
        {
            Vector2 focus = innerRect.GetAlignedPoint(Alignment.TopLeft);
            WriteCorner(stream, focus, innerRadii, outerRadii, steps, MathF.PI, MathF.PI * (3f / 2f));
        }

        static void WriteCorner(GeometryStream stream, Vector2 focus, Vector2 innerRadii, Vector2 outerRadii, int steps, float start, float end)
        {
            float increment = (end - start) / steps;
            float angle = start;
            for (int i = 0; i < steps; i++)
            {
                stream.WriteVertex(focus + innerRadii * Angle.ToVector(angle));
                stream.WriteVertex(focus + outerRadii * Angle.ToVector(angle));
                stream.WriteVertex(focus + outerRadii * Angle.ToVector(angle + increment));

                stream.WriteVertex(focus + innerRadii * Angle.ToVector(angle));
                stream.WriteVertex(focus + innerRadii * Angle.ToVector(angle + increment));
                stream.WriteVertex(focus + outerRadii * Angle.ToVector(angle + increment));

                angle += increment;
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
            stream.WriteVertex(bounds.Center + Angle.ToVector(angle) * (.5f * bounds.Size - new Vector2(StrokeWidth)));
            stream.WriteVertex(bounds.Center + Angle.ToVector(angle) * (.5f * bounds.Size + new Vector2(StrokeWidth)));
            stream.WriteVertex(bounds.Center + Angle.ToVector(angle + increment) * (.5f * bounds.Size - new Vector2(StrokeWidth)));

            stream.WriteVertex(bounds.Center + Angle.ToVector(angle) * (.5f * bounds.Size + new Vector2(StrokeWidth)));
            stream.WriteVertex(bounds.Center + Angle.ToVector(angle + increment) * (.5f * bounds.Size + new Vector2(StrokeWidth)));
            stream.WriteVertex(bounds.Center + Angle.ToVector(angle + increment) * (.5f * bounds.Size - new Vector2(StrokeWidth)));

            angle += increment;
        }
    }

    public override void PushEllipse(GeometryStream stream, Rectangle bounds)
    {
        int steps = CalculateRoundedEdgeQuality(stream.TransformMatrix, MathF.Max(bounds.Width, bounds.Height) * .5f, MathF.Tau);

        float increment = MathF.Tau / steps;
        float angle = 0;
        for (int i = 0; i < steps; i++)
        {
            stream.WriteVertex(bounds.Center + Angle.ToVector(angle) * (.5f * bounds.Size - new Vector2(StrokeWidth)));
            stream.WriteVertex(bounds.Center + Angle.ToVector(angle) * (.5f * bounds.Size + new Vector2(StrokeWidth)));
            stream.WriteVertex(bounds.Center + Angle.ToVector(angle + increment) * (.5f * bounds.Size - new Vector2(StrokeWidth)));

            stream.WriteVertex(bounds.Center + Angle.ToVector(angle) * (.5f * bounds.Size + new Vector2(StrokeWidth)));
            stream.WriteVertex(bounds.Center + Angle.ToVector(angle + increment) * (.5f * bounds.Size + new Vector2(StrokeWidth)));
            stream.WriteVertex(bounds.Center + Angle.ToVector(angle + increment) * (.5f * bounds.Size - new Vector2(StrokeWidth)));

            angle += increment;
        }
    }
}
