using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

/// <summary>
/// Contains various methods for determining collisions between shapes.
/// </summary>
public class Collision
{
    // poly
    // square/rect/aabb
    // ellipse/circle/oval
    // line/ray
    // point

    public static bool CollidePolygonPolygon(Span<Vector2> polygonA, Span<Vector2> polygonB)
    {
        // separating axis theorem
        throw new NotImplementedException();
    }

    public static bool CollidePolygonRectangle(Span<Vector2> polygon, Rectangle rectangle)
    {
        throw new NotImplementedException();
    }

    public static bool CollidePolygonCircle(Span<Vector2> polygon, Circle circle)
    {
        throw new NotImplementedException();
    }

    public static bool CollidePolygonLine(Span<Vector2> polygon, Vector2 from, Vector2 to)
    {
        throw new NotImplementedException();
    }

    public static bool CollidePolygonPoint(Span<Vector2> polygon, Vector2 point)
    {
        throw new NotImplementedException();
    }

    public static bool CollideRectangleRectangle(Rectangle rectangleA, Rectangle rectangleB)
    {
        throw new NotImplementedException();
    }

    public static bool CollideRectangleCircle(Rectangle rectangle, Circle circle)
    {
        throw new NotImplementedException();
    }

    public static bool CollideRectangleLine(Rectangle rectangle, Vector2 from, Vector2 to)
    {
        throw new NotImplementedException();
    }

    public static bool CollideRectanglePoint(Rectangle rectangle, Vector2 point)
    {
        return rectangle.ContainsPoint(point);
    }

    public static bool CollideCircleCircle(Circle circleA, Circle circleB)
    {
        return Vector2.DistanceSquared(circleA.Position, circleB.Position) <= (circleA.Radius + circleB.Radius) * (circleA.Radius + circleB.Radius);
    }

    public static bool CollideCircleLine(Circle circle, Vector2 from, Vector2 to)
    {
        // http://jeffreythompson.org/collision-detection/line-circle.php
        bool fromInside = CollideCirclePoint(circle, from);
        bool toInside = CollideCirclePoint(circle, from);
        
        // if line begin/end 
        if (fromInside || toInside)
            return true;

        Vector2 diff = from - to;

        float dot = Vector2.Dot(circle.Position - from, to - from) / diff.Length();

        Vector2 closest = from + (dot * (to - from));

        if (!CollideLinePoint(from, to, closest))
            return false;

        var dist = closest - circle.Position;

        if (dist.LengthSquared() <= circle.Radius * circle.Radius)
            return true;

        return false;
    }

    public static bool CollideCirclePoint(Circle circle, Vector2 point)
    {
        return (circle.Position - point).LengthSquared() <= circle.Radius * circle.Radius;
    }

    public static bool CollideLineLine(Vector2 fromA, Vector2 toA, Vector2 fromB, Vector2 toB, out Vector2 point)
    {
        var a1 = toA.Y - fromA.Y;
        var b1 = fromA.X - toA.X;
        var c1 = a1 * fromA.X + b1 * fromA.Y;

        var a2 = toB.Y - fromB.Y;
        var b2 = fromB.X - toB.X;
        var c2 = a2 * fromB.X + b2 * fromB.Y;

        var delta = a1 * b2 - a2 * b1;

        if (delta == 0) 
        {
            point = new Vector2(float.NaN);
            return false;
        }

        point.X = (b2 * c1 - b1 * c2) / delta;
        point.Y = (a1 * c2 - a2 * c1) / delta;

        return true;
    }

    public static bool CollideLinePoint(Vector2 from, Vector2 to, Vector2 point)
    {
        return Vector2.DistanceSquared(from, point) + Vector2.DistanceSquared(point, to) == Vector2.DistanceSquared(from, to);
    }
}