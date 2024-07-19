using System;
using System.Numerics;

namespace SimulationFramework.OpenGL;

abstract class GeometryWriter
{
    public abstract uint GetPrimitive();
    public abstract void PushRect(GeometryStream stream, Rectangle rect);
    public abstract void PushLine(GeometryStream stream, Vector2 from, Vector2 to);
    public abstract void PushRoundedRect(GeometryStream stream, Rectangle rect, Vector2 radii);
    public abstract void PushPolygon(GeometryStream stream, ReadOnlySpan<Vector2> polygon, bool close);
    public abstract void PushArc(GeometryStream stream, Rectangle bounds, float begin, float end, bool includeCenter);
    public abstract void PushEllipse(GeometryStream stream, Rectangle bounds);
    public abstract void PushTriangles(GeometryStream stream, ReadOnlySpan<Vector2> triangles);

    public static int CalculateRoundedEdgeQuality(Matrix3x2 transformMatrix, float radius, float arcLength)
    {
        // calculates the number of vertices required to draw a round shape given a transform matrix without sharp edges

        const float scale = 2f;
        const int minVertices = 8;
        const int maxVertices = 256; // limit vertices to 256 to prevent crashes or performance problems.
                                     // This causes very minor artifacts when drawing circles that are orders of magnitude larger than the screen.

        Vector2 x = new(transformMatrix.M11, transformMatrix.M12);
        Vector2 y = new(transformMatrix.M21, transformMatrix.M22);
        float largestLengthSquared = MathF.Max(x.LengthSquared(), y.LengthSquared());

        float factor = radius * arcLength * MathF.Sqrt(largestLengthSquared);
        float vertexCount = scale * MathF.Sqrt(factor);
        return (int)Math.Clamp(vertexCount, minVertices, maxVertices);
    }
}
