using SimulationFramework.Drawing;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Numerics;
using System.Reflection.Metadata;

// parameter's docs may be inherited
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)

namespace SimulationFramework;

/// <summary>
/// Contains various static methods for working with polygons.
/// </summary>
public static class Polygon
{
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
    /// <param name="point">The point.</param>
    /// <returns><see langword="true"/> when <paramref name="point"/> is inside of <paramref name="polygon"/>; otherwise <see langword="false"/>.</returns>
    /// <exception cref="NotImplementedException"></exception>
    public static bool ContainsPoint(Span<Vector2> polygon, Vector2 point)
    {
        // TODO: IMPLEMENT RAYCASTS FIRST https://stackoverflow.com/questions/217578/how-can-i-determine-whether-a-2d-point-is-within-a-polygon
        throw new NotImplementedException();
    }

    /// <summary>
    /// Determines if a polygon is convex.
    /// </summary>
    /// <param name="polygon">The polygon.</param>
    /// <returns><see langword="true"/> if <paramref name="polygon"/> is convex; otherwise <see langword="false"/></returns>
    public static bool IsConvex(ReadOnlySpan<Vector2> polygon, [NotNullWhen(true)] out bool? isClockwise)
    {
        if (!IsValid(polygon))
            throw new ArgumentException(null, nameof(polygon));

        bool isAllClockwise = IsClockwiseWinding(polygon, 0);
        for (int i = 0; i < polygon.Length; i++)
        {
            bool vertexClockwise = IsClockwiseWinding(polygon, i);
            if (isAllClockwise == vertexClockwise)
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

        static bool LineIntersect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
        {
            // https://en.wikipedia.org/wiki/Line%E2%80%93line_intersection#Given_two_points_on_each_line_segment

            float t_numerator = (p1.X - p3.X) * (p1.Y - p2.Y) - (p1.Y - p3.Y) * (p1.X - p2.X);
            float t_denominator = (p1.X - p2.X) * (p3.Y - p4.Y) - (p1.Y - p2.Y) * (p3.X - p4.X);
            float t = t_numerator / t_denominator;

            return 0 <= t && t <= 1;
        }
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

    // TODO: document
    public static bool IsClosed(ReadOnlySpan<Vector2> polygon)
    {
        return polygon[0] == polygon[^1];
    }

    // TODO: document
    public static bool IsValid(ReadOnlySpan<Vector2> polygon)
    {
        if (RemoveCollinearVertices(polygon, Span<Vector2>.Empty) < 3)
            return false;

        return true;
    }

    public static Vector2[] RemoveCollinearVertices(ReadOnlySpan<Vector2> polygon)
    {
        // TODO: implement
        throw new NotImplementedException();
    }

    public static int RemoveCollinearVertices(ReadOnlySpan<Vector2> polygon, Span<Vector2> result)
    {
        // TODO: implement
        throw new NotImplementedException();
    }

    /// <summary>
    /// Finds the smallest possible rectangle that contains a polygon.
    /// </summary>
    /// <param name="polygon">The polygon to find the bounds of.</param>
    /// <returns>The smallest possible rectangle that contains all the points in <paramref name="polygon"/>.</returns>
    public static Rectangle GetBoundingBox(ReadOnlySpan<Vector2> polygon)
    {
        if (!IsValid(polygon))
            CheckValidity(polygon);

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

    private static void CheckValidity(ReadOnlySpan<Vector2> polygon)
    {
        if (!IsValid(polygon))
            throw new("Invalid polygon!");
    }
}