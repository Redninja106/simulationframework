using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Canvas;

/// <summary>
/// 
/// </summary>
/// <param name="Stops"></param>
/// <param name="Transform"></param>
/// <param name="TileMode"></param>
public abstract record Gradient(GradientStop[] Stops, Matrix3x2 Transform, TileMode TileMode)
{
    public abstract void Accept(IGradientVisitor inspector);

    public static Gradient CreateLinear(float fromX, float fromY, float toX, float toY, params Color[] colors) => CreateLinear(new Vector2(fromX, fromY), new Vector2(toX, toY), colors);
    public static Gradient CreateLinear(Vector2 from, Vector2 to, params Color[] colors) => CreateLinear(from, to, ColorsToStops(colors));
    public static Gradient CreateLinear(Vector2 from, Vector2 to, params GradientStop[] colors) => CreateLinear(from, to, colors, Matrix3x2.Identity);
    public static Gradient CreateLinear(Vector2 from, Vector2 to, Color[] stops, Matrix3x2 transform, TileMode tileMode = TileMode.Clamp) => CreateLinear(from, to, ColorsToStops(stops), transform, tileMode);
    public static Gradient CreateLinear(Vector2 from, Vector2 to, GradientStop[] stops, Matrix3x2 transform, TileMode tileMode = TileMode.Clamp) => new LinearGradient(from, to, stops, transform, tileMode);

    public static Gradient CreateRadial(float x, float y, float radius, params Color[] colors) => CreateRadial(new Vector2(x, y), radius, colors);
    public static Gradient CreateRadial(Vector2 position, float radius, params Color[] colors) => CreateRadial(position, radius, ColorsToStops(colors));
    public static Gradient CreateRadial(Vector2 position, float radius, params GradientStop[] colors) => CreateRadial(position, radius, colors, Matrix3x2.Identity);
    public static Gradient CreateRadial(Vector2 position, float radius, Color[] stops, Matrix3x2 transform, TileMode tileMode = TileMode.Clamp) => CreateRadial(position, radius, ColorsToStops(stops), transform, tileMode);
    public static Gradient CreateRadial(Vector2 position, float radius, GradientStop[] stops, Matrix3x2 transform, TileMode tileMode = TileMode.Clamp) => new RadialGradient(position, radius, stops, transform, tileMode);

    private static GradientStop[] ColorsToStops(Color[] colors)
    {
        var stops = new GradientStop[colors.Length];

        for (var i = 0; i < colors.Length; i++)
        {
            stops[i] = new GradientStop(colors[i], i / (float)colors.Length);
        }

        return stops;
    }
}