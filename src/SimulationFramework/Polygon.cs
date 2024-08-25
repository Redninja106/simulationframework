using ImGuiNET;
using SimulationFramework.Drawing;
using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;

// parameter's docs may be inherited
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)

namespace SimulationFramework;

/// <summary>
/// Contains various static methods for working with polygons.
/// </summary>
public static class Polygon
{
    /// <summary>
    /// Creates an <see cref="IGeometry"/> from a polygon.
    /// </summary>
    public static IGeometry ToGeometry(ReadOnlySpan<Vector2> polygon)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Determines if two convex polygons are intersecting.
    /// </summary>
    /// <param name="polygonA">The first polygon. This should be convex and non-self-intersecting.</param>
    /// <param name="polygonB">The second polygon. This should be convex and non-self-intersecting.</param>
    /// <remarks>
    /// This method does <b>not</b> check its arguments for non-convex or self-intersecting polygons. Passing invalid polygons will most likely give incorrect results. 
    /// </remarks>
    /// <returns><see langword="true"/> if <paramref name="polygonA"/> and <paramref name="polygonB"/> are intersecting; otherwise <see langword="false"/>.</returns>
    public static bool CollideConvex(ReadOnlySpan<Vector2> polygonA, ReadOnlySpan<Vector2> polygonB)
    {
        return CollideConvex(polygonA, polygonB, out _);
    }

    /// <inheritdoc cref="CollideConvex(ReadOnlySpan{Vector2}, ReadOnlySpan{Vector2})" /> 
    /// <param name="minimumTranslationValue">
    /// If this method returns true, this is set to the smallest distance which one of the shapes could be moved to resolve the collison. Otherwise, this is <see cref="Vector2.Zero"/>.
    /// </param>
    public static bool CollideConvex(ReadOnlySpan<Vector2> polygonA, ReadOnlySpan<Vector2> polygonB, out Vector2 minimumTranslationValue)
    {
        Span<Vector2> axes = stackalloc Vector2[polygonA.Length + polygonB.Length - 2];
        GetAxes(polygonA, axes);
        GetAxes(polygonB, axes[(polygonA.Length - 1)..]);

        float leastOverlap = float.PositiveInfinity;
        Vector2 leastOverlapAxis = default;

        for (int i = 0; i < axes.Length; i++)
        {
            Project(polygonA, axes[i], out float aMin, out float aMax);
            Project(polygonB, axes[i], out float bMin, out float bMax);

            if ((aMin < bMax && aMin > bMin) || (bMin < aMax && bMin > aMin))
            {
                float overlap = MathF.Min(aMax, bMax) - MathF.Max(aMin, bMin);

                if (overlap < leastOverlap)
                {
                    leastOverlap = overlap;
                    leastOverlapAxis = axes[i];
                }

                continue;
            }

            minimumTranslationValue = default;
            return false;
        }

        minimumTranslationValue = leastOverlapAxis * leastOverlap;
        return true;

        static void GetAxes(ReadOnlySpan<Vector2> shape, Span<Vector2> axes)
        {
            for (int i = 0; i < shape.Length - 1; i++)
            {
                Vector2 side = shape[i] - shape[i + 1];
                axes[i] = new(side.Y, -side.X);
                axes[i] = axes[i].Normalized();
            }
        }

        static void Project(ReadOnlySpan<Vector2> shape, Vector2 axis, out float min, out float max)
        {
            min = float.PositiveInfinity;
            max = float.NegativeInfinity;

            for (int i = 0; i < shape.Length; i++)
            {
                Vector2 vertex = shape[i];

                float dot = Vector2.Dot(vertex, axis);

                min = Math.Min(min, dot);
                max = Math.Max(max, dot);
            }
        }
    }

    /// <summary>
    /// Determines if a polygon contains a point.
    /// </summary>
    /// <param name="polygon">The polygon. May be convex or concave, but not self-intersecting.</param>
    /// <param name="point">The point to test against the polygon.</param>
    /// <returns><see langword="true"/> when <paramref name="point"/> is inside of <paramref name="polygon"/>; otherwise <see langword="false"/>.</returns>
    public static bool ContainsPoint(Span<Vector2> polygon, Vector2 point)
    {
        int intersections = 0;
        for (int i = 0; i < polygon.Length; i++)
        {
            Vector2 from = polygon[i];
            Vector2 to = polygon[i + 1 >= polygon.Length ? 0 : (i + 1)];

            if (RayLineIntersect(point, Vector2.UnitX, from, to, out float _))
            {
                intersections++;
            }
        }

        return intersections % 2 is 1;
    }

    private static bool RayLineIntersect(Vector2 position, Vector2 direction, Vector2 from, Vector2 to, out float t)
    {
        var v1 = position - from;
        var v2 = to - from;
        var v3 = new Vector2(-direction.Y, direction.X);

        var dot = Vector2.Dot(v2, v3);
        if (Math.Abs(dot) < 0.00001f)
        {
            t = float.PositiveInfinity;
            return false;
        }

        var t1 = MathHelper.Cross(v2, v1) / dot;
        var t2 = Vector2.Dot(v1, v3) / dot;

        if (t1 >= 0.0f && (t2 >= 0.0f && t2 < 1.0f))
        {
            t = t1;
            return true;
        }

        t = float.PositiveInfinity;
        return false;
    }

    /// <summary>
    /// Determines if a polygon is convex.
    /// </summary>
    /// <param name="polygon">The polygon.</param>
    /// <returns><see langword="true"/> if <paramref name="polygon"/> is convex; otherwise <see langword="false"/></returns>
    public static bool IsConvex(ReadOnlySpan<Vector2> polygon, [NotNullWhen(true)] out bool? isClockwise)
    {
        // for a convex polygon every point will have the same winding value
        bool isAllClockwise = IsClockwiseWinding(polygon, 0);
        for (int i = 0; i < polygon.Length; i++)
        {
            bool vertexClockwise = IsClockwiseWinding(polygon, i);
            if (isAllClockwise != vertexClockwise)
            {
                return false;
            }
        }

        isClockwise = null;
        return false;

        // determines the winding order at a specific vertex
        static bool IsClockwiseWinding(ReadOnlySpan<Vector2> polygon, int vertexIndex)
        {
            int prevVertexIndex = WrapIndex(vertexIndex - 1, polygon.Length);
            int nextVertexIndex = WrapIndex(vertexIndex + 1, polygon.Length);

            Vector2 from = polygon[prevVertexIndex] - polygon[vertexIndex];
            Vector2 to = polygon[nextVertexIndex] - polygon[vertexIndex];

            // 2d cross product
            float t = to.X * from.Y - to.Y * from.X;
            return t > 0;
        }

        static int WrapIndex(int index, int length)
        {
            return (index < 0 ? index + length : index) % length;
        }
    }

    /// <summary>
    /// Determines if a polygon is self-intersecting.
    /// </summary>
    /// <param name="polygon">The polygon to test for self-intersection.</param>
    /// <returns><see langword="true"/> if <paramref name="polygon"/> is self-intersecting; otherwise <see langword="false"/>.</returns>
    public static bool IsSelfIntersecting(ReadOnlySpan<Vector2> polygon)
    {
        if (polygon.Length < 3)
            return false;

        for (int i = 0; i < polygon.Length - 1; i++)
        {
            for (int j = i + 1; j < polygon.Length - 1; j++)
            {
                if (LineIntersect(polygon[i], polygon[i + 1], polygon[j], polygon[j + 1]))
                {
                    return true;
                }
            }
        }

        return false;
    }

    static bool LineIntersect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4) => LineIntersect(p1, p2, p3, p4, out _);
    static bool LineIntersect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, out Vector2 position)
    {
        // https://en.wikipedia.org/wiki/Line%E2%80%93line_intersection#Given_two_points_on_each_line_segment
        position = Vector2.Zero;

        float t_numerator = (p1.X - p3.X) * (p3.Y - p4.Y) - (p1.Y - p3.Y) * (p3.X - p4.X);
        float t_denominator = (p1.X - p2.X) * (p3.Y - p4.Y) - (p1.Y - p2.Y) * (p3.X - p4.X);
        
        if (t_numerator > t_denominator || t_denominator <= 0)
        {
            return false;
        }

        float t = t_numerator / t_denominator;

        float u_numerator = (p1.X - p3.X) * (p1.Y - p2.Y) - (p1.Y - p3.Y) * (p1.X - p2.X);
        float u_denominator = (p1.X - p2.X) * (p3.Y - p4.Y) - (p1.Y - p2.Y) * (p3.X - p4.X);


        if (u_numerator > u_denominator || u_denominator <= 0)
        {
            return false;
        }

        position = p1 + t * (p2 - p1);

        return true;
    }


    /// <summary>
    /// Closes a polygon. This method returns a new polygon; <paramref name="polygon"/> is unmodified.
    /// </summary>
    /// <param name="polygon">The polygon to close.</param>
    /// <returns>The new closed polygon.</returns>
    public static Vector2[] Close(ReadOnlySpan<Vector2> polygon)
    {
        // don't allocate a larger array than we need to
        if (IsClosed(polygon))
            return polygon.ToArray();

        var result = new Vector2[polygon.Length + 1];
        Close(polygon, result);
        return result;
    }

    /// <summary>
    /// Closes a polygon. The closed polygon is written to <paramref name="result"/>; <paramref name="polygon"/> is unmodified.
    /// </summary>
    /// <param name="polygon"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static int Close(ReadOnlySpan<Vector2> polygon, Span<Vector2> result)
    {
        polygon.CopyTo(result);
        
        if (!IsClosed(polygon))
        {
            if (result.Length < polygon.Length + 1)
                throw new ArgumentException("'result' must have a length at least one greater than 'polygon'", nameof(result));

            result[polygon.Length] = polygon[0];
            
            return polygon.Length + 1;
        }

        return polygon.Length;
    }
    
    /// <summary>
    /// Determines if a polygon is closed.
    /// </summary>
    /// <returns><see langword="true"/> if the polygon is closed; otherwise <see langword="false"/>.</returns>
    public static bool IsClosed(ReadOnlySpan<Vector2> polygon)
    {
        return polygon[0] == polygon[^1];
    }

    /// <summary>
    /// Finds the smallest possible rectangle that contains a polygon.
    /// </summary>
    /// <param name="polygon">The polygon to find the bounds of.</param>
    /// <returns>The smallest possible rectangle that contains all the points in <paramref name="polygon"/>.</returns>
    public static Rectangle GetBoundingRectangle(ReadOnlySpan<Vector2> polygon)
    {
        Vector2 min = new(float.PositiveInfinity, float.PositiveInfinity);
        Vector2 max = new(float.NegativeInfinity, float.NegativeInfinity);
        
        for (int i = 0; i < polygon.Length; i++)
        {
            var point = polygon[i];
            min = Vector2.Min(min, point);
            max = Vector2.Max(max, point);
        }

        return Rectangle.FromPoints(min, max);
    }

    public static Circle GetBoundingCircle(ReadOnlySpan<Vector2> polygon)
    {
        // TODO: implement welzl's algorithm here
        Vector2 vertexAverage = Vector2.Zero;
        for (int i = 0; i < polygon.Length; i++)
        {
            vertexAverage += polygon[i];
        }
        vertexAverage /= polygon.Length;

        float furthestDistance = float.PositiveInfinity;
        for (int i = 0; i < polygon.Length; i++)
        {
            float distSq = Vector2.DistanceSquared(polygon[i], vertexAverage);
            if (distSq < furthestDistance * furthestDistance)
            {
                furthestDistance = MathF.Sqrt(distSq);
            }
        }

        return new(vertexAverage, furthestDistance);
    }

    public static bool IsClockwise(ReadOnlySpan<Vector2> polygon)
    {
        return SignedArea(polygon) > 0;
    }

    public static float Area(ReadOnlySpan<Vector2> polygon)
    {
        return MathF.Abs(SignedArea(polygon));
    }

    public static float SignedArea(ReadOnlySpan<Vector2> polygon)
    {
        // https://stackoverflow.com/questions/1165647/how-to-determine-if-a-list-of-polygon-points-are-in-clockwise-order
        float result = 0;
        for (int i = 0; i < polygon.Length; i++)
        {
            Vector2 from = polygon[i];
            Vector2 to = polygon[i + 1 == polygon.Length ? 0 : i + 1];

            result += from.X * to.Y - to.X * from.Y;
        }
        return result * .5f;
    }

    public static Vector2[] Triangulate(ReadOnlySpan<Vector2> polygon)
    {
        if (polygon.Length < 3)
        {
            return polygon.ToArray();
        }

        Vector2[] result = new Vector2[(polygon.Length - 2) * 3];
        Triangulate(polygon, result);
        return result;
    }

    public static void Triangulate(ReadOnlySpan<Vector2> polygon, Span<Vector2> triangles)
    {
        if (polygon.Length <= 3) 
        { 
            polygon.CopyTo(triangles);
            return; 
        }

        List<Vector2> tris = [];
        int nextTri = 0;
        bool cw = IsClockwise(polygon);

        PolygonLinkedList list = polygon.Length < 512 
            ? PolygonLinkedList.StackAllocate(polygon, stackalloc PolygonLinkedList.Vertex[polygon.Length])
            : PolygonLinkedList.HeapAllocate(polygon);

        int current;
        while (tris.Count < (polygon.Length - 2) * 3 && list.Length >= 3)
        {
            bool pushedTri = false;
            current = list.First();
            for (int i = 0; i < polygon.Length + 1; i++)
            {
                if (cw ^ IsVertexConvex(list, current))
                {
                    Vector2 p1 = list[current];
                    Vector2 p2 = list[list.Previous(current)];
                    Vector2 p3 = list[list.Next(current)];

                    int current2 = list.Next(list.Next(current));
                    bool pointInTri = false;
                    for (int j = 0; j < list.Length - 3; j++)
                    {
                        Debug.Assert(current2 != current);
                        Debug.Assert(current2 != list.Previous(current));
                        Debug.Assert(current2 != list.Next(current));

                        if (PointInTriangle(list[current2], p1, p2, p3))
                        {
                            pointInTri = true;
                            break;
                        }
                        current2 = list.Next(current2);
                    }

                    if (pointInTri)
                    {
                        current = list.Next(current);
                        continue;
                    }

                    triangles[nextTri++] = p1;
                    triangles[nextTri++] = p2;
                    triangles[nextTri++] = p3;

                    list.Remove(current);

                    if (list.Length < 3)
                    {
                        break;
                    }

                    pushedTri = true;
                }
                current = list.Next(current);
            }
            if (!pushedTri)
            {
                // error case: didn't find an ear
                return;
            }
        }

        static bool IsVertexConvex(PolygonLinkedList list, int index)
        {
            Vector2 vertex = list[index];
            Vector2 prev = list[list.Previous(index)];
            Vector2 next = list[list.Next(index)];

            Vector2 p1 = vertex - prev;
            Vector2 p2 = next - vertex;

            return (MathF.PI + MathF.Atan2(MathHelper.Cross(p1, p2), Vector2.Dot(p1, p2))) < MathF.PI;
        }
    }

    // https://stackoverflow.com/questions/2049582/how-to-determine-if-a-point-is-in-a-2d-triangle
    public static bool PointInTriangle(Vector2 p, Vector2 p0, Vector2 p1, Vector2 p2)
    {
        var s = (p0.X - p2.X) * (p.Y - p2.Y) - (p0.Y - p2.Y) * (p.X - p2.X);
        var t = (p1.X - p0.X) * (p.Y - p0.Y) - (p1.Y - p0.Y) * (p.X - p0.X);

        if ((s < 0) != (t < 0) && s != 0 && t != 0)
            return false;

        var d = (p2.X - p1.X) * (p.Y - p1.Y) - (p2.Y - p1.Y) * (p.X - p1.X);
        return d == 0 || (d < 0) == (s + t <= 0);
    }

    private ref struct PolygonLinkedList
    {
        Span<Vertex> vertices;
        int firstVertex;
        int length;

        public int Length => length;

        public static PolygonLinkedList HeapAllocate(ReadOnlySpan<Vector2> polygon)
        {
            PolygonLinkedList result = new();

            result.vertices = new Vertex[polygon.Length];
            result.Initialize(polygon);

            return result;
        }

        public static PolygonLinkedList StackAllocate(ReadOnlySpan<Vector2> polygon, Span<Vertex> stackMemory)
        {
            Debug.Assert(stackMemory.Length == polygon.Length);

            PolygonLinkedList result = new();

            result.vertices = stackMemory;
            result.Initialize(polygon);

            return result;
        }

        private void Initialize(ReadOnlySpan<Vector2> polygon)
        {
            for (int i = 0; i < polygon.Length; i++)
            {
                vertices[i].position = polygon[i];
                vertices[i].prev = i - 1;
                vertices[i].next = i + 1;
            }
            vertices[0].prev = polygon.Length - 1;
            vertices[polygon.Length - 1].next = 0;

            length = polygon.Length;
        }


        public Vector2 this[int index] => vertices[index].position;

        public void Remove(int index)
        {
            var vertex = vertices[index];

            if (index == firstVertex)
            {
                firstVertex = Next(firstVertex);
            }

            vertices[vertex.next].prev = vertex.prev;
            vertices[vertex.prev].next = vertex.next;

            length--;
        }

        public int Next(int index)
        {
            return vertices[index].next;
        }

        public int Previous(int index)
        {
            return vertices[index].prev;
        }

        public int First()
        {
            return firstVertex;
        }

        public struct Vertex
        {
            public Vector2 position;
            public int next;
            public int prev;
        }
    }
}