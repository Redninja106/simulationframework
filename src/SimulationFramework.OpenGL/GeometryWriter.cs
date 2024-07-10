using System;
using System.Numerics;

namespace SimulationFramework.OpenGL;

abstract class GeometryWriter
{
    public abstract uint GetPrimitive();
    public abstract void PushRect(GeometryStream stream, Rectangle rect);
    public abstract void PushLine(GeometryStream stream, Vector2 from, Vector2 to);
    public abstract void PushRoundedRect(GeometryStream stream, Rectangle rect, float radius);
    public abstract void PushPolygon(GeometryStream stream, ReadOnlySpan<Vector2> polygon, bool close);
    public abstract void PushArc(GeometryStream stream, Rectangle bounds, float begin, float end, bool includeCenter);
    public abstract void PushTriangles(GeometryStream stream, ReadOnlySpan<Vector2> triangles);
}
