using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.OpenGL;
internal class PathGeometryWriter : GeometryWriter
{
    public float Width { get; set; }

    public override uint GetPrimitive()
    {
        return GL_TRIANGLES;
    }

    public override void PushLine(GeometryStream stream, Vector2 from, Vector2 to)
    {
        Vector2 diff = to - from;
        Vector2 perp = new(-diff.Y, diff.X);
        perp = perp.Normalized() * Width * .5f;

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
        Vector2 firstOffset = ComputeOffset(Width, close ? firstPoint - polygon[^1] : firstOutgoing, firstOutgoing);

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

            Vector2 offset = ComputeOffset(Width, incomingDir, outgoingDir);

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
        Vector2 tlOffset = new(-Width * .5f, -Width * .5f);
        Vector2 tr = new(rect.X + rect.Width, rect.Y);
        Vector2 trOffset = new(Width * .5f, -Width * .5f);
        Vector2 bl = new(rect.X, rect.Y + rect.Height);
        Vector2 blOffset = new(-Width * .5f, Width * .5f);
        Vector2 br = new(rect.X + rect.Width, rect.Y + rect.Height);
        Vector2 brOffset = new(Width * .5f, Width * .5f);

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

    public override void PushRoundedRect(GeometryStream stream, Rectangle rect, float radius)
    {
        throw new NotImplementedException();
    }

    public override void PushArc(GeometryStream stream, Rectangle bounds, float begin, float end, bool includeCenter)
    {
        throw new NotImplementedException();
    }
}
